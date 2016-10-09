//-----------------------------------------------------------------------
// <copyright file="BizTalkXmlMapExecutor.cs">
//     Copyright (c) 2016 Adam Craven. All rights reserved.
// </copyright>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

namespace ChannelAdam.TestFramework.BizTalk.Helpers
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Xsl;

    using Microsoft.XLANGs.BaseTypes;
    using System.Collections.Generic;
    using System.Linq;

    public static class BizTalkXmlMapExecutor
    {
        #region Public Methods

        /// <summary>
        /// Perform the transform using the map.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="inputXml"></param>
        /// <returns>The resulting XML output from the map.</returns>
        public static string PerformTransform(TransformBase map, XNode inputXml)
        {
            return PerformTransform(map, map?.TransformArgs, inputXml);
        }

        /// <summary>
        /// Perform the transform using the map.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="xsltExtensionObjectOverrides">Overrides the extension objects in the map with the specified types.</param>
        /// <param name="inputXml"></param>
        /// <returns>The resulting XML output from the map.</returns>
        public static string PerformTransform(TransformBase map, IEnumerable<XsltExtensionObjectDescriptor> xsltExtensionObjectOverrides, XNode inputXml)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));

            XsltArgumentList xsltArguments = map.TransformArgs;      // map.TransformArgs is immutable and created each time the property is called ;)

            if (xsltExtensionObjectOverrides != null)
            {
                ApplyXsltExtensionObjectOverridesToXsltArguments(xsltArguments, xsltExtensionObjectOverrides, map);
            }

            return PerformTransform(map, xsltArguments, inputXml);
        }

        #endregion

        #region Private Methods

        private static void ApplyXsltExtensionObjectOverridesToXsltArguments(XsltArgumentList transformArgs, IEnumerable<XsltExtensionObjectDescriptor> xsltExtensionObjectOverrides, TransformBase map)
        {
            var xsltArgumentListContent = XElement.Parse(map.XsltArgumentListContent);

            foreach (var item in xsltExtensionObjectOverrides)
            {
                var name = FindNamespaceForXsltExtensionObjectType(xsltArgumentListContent, item.ClassType);
                transformArgs.RemoveExtensionObject(name);
                transformArgs.AddExtensionObject(name, item.Instance);
            }
        }

        private static string FindNamespaceForXsltExtensionObjectType(XElement xsltArgumentListContent, Type classType)
        {
            return xsltArgumentListContent.Descendants("ExtensionObject")
                    .Where(el => (string)el.Attribute("ClassName") == classType.FullName)
                    .Select(el => el.Attribute("Namespace").Value)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Perform the transform using the map.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="xsltArguments"></param>
        /// <param name="inputXml"></param>
        /// <returns>The resulting XML output from the map.</returns>
        private static string PerformTransform(TransformBase map, XsltArgumentList xsltArguments, XNode inputXml)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));
            if (inputXml == null) throw new ArgumentNullException(nameof(inputXml));

            try
            {
                XslCompiledTransform transform = BizTalkMapSchemaUtility.LoadStylesheetFromMap(map);

                using (var input = XmlReader.Create(new StringReader(inputXml.ToString())))
                {
                    var sb = new StringBuilder();
                    var settings = new XmlWriterSettings
                    {
                        Indent = true
                    };

                    using (var results = XmlWriter.Create(sb, settings))
                    {
                        transform.Transform(input, xsltArguments, results);
                    }

                    return sb.ToString();
                }
            }
            catch (Exception exception)
            {
                var sb = new StringBuilder("An error occurred while executing the transform:" + Environment.NewLine);

                var currentException = exception;
                while (currentException != null)
                {
                    sb.AppendLine(currentException.ToString());

                    currentException = currentException.InnerException;
                }

                throw new InvalidDataException(sb.ToString());
            }
        }

        #endregion
    }
}