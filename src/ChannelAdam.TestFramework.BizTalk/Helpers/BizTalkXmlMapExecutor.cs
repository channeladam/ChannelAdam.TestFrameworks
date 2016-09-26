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
            try
            {
                XslCompiledTransform transform = LoadStylesheetFromMap(map);

                using (var input = XmlReader.Create(new StringReader(inputXml.ToString())))
                {
                    var sb = new StringBuilder();
                    var settings = new XmlWriterSettings
                    {
                        Indent = true
                    };

                    using (var results = XmlWriter.Create(sb, settings))
                    {
                        transform.Transform(input, map.TransformArgs, results);
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

                throw new ApplicationException(sb.ToString());
            }
        }

        private static XslCompiledTransform LoadStylesheetFromMap(TransformBase map)
        {
            var transform = new XslCompiledTransform();
            using (var stylesheet = new XmlTextReader(new StringReader(map.XmlContent)))
            {
                transform.Load(stylesheet, new XsltSettings(true, true), null);
            }

            return transform;
        }

        #endregion
    }
}