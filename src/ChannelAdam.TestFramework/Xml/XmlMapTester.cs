//-----------------------------------------------------------------------
// <copyright file="XmlMapTester.cs">
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

namespace ChannelAdam.TestFramework.Xml
{
    using System;
    using System.Reflection;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using ChannelAdam.Logging;
    using ChannelAdam.Reflection;
    using ChannelAdam.Xml;

    public class XmlMapTester : XmlMapTesterBase
    {
        #region Constructor / Destructor

        protected XmlMapTester(ILogAsserter logAsserter) : base(logAsserter)
        {
        }

        protected XmlMapTester(ISimpleLogger logger, ILogAsserter logAsserter) : base(logger, logAsserter)
        {
        }

        #endregion

        #region Properties

        public XElement InputXml { get; private set; }

        #endregion

        #region Public Methods

        #region Arrange Input XML

        /// <summary>
        /// Arrange the input XML from an embedded resource in the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly that contains the resource.</param>
        /// <param name="resourceName">The name of the resource.</param>
        public void ArrangeInputXml(Assembly assembly, string resourceName)
        {
            this.ArrangeInputXml(EmbeddedResource.GetAsString(assembly, resourceName));
        }

        /// <summary>
        /// Arrange the input XML from the given XElement.
        /// </summary>
        /// <param name="xmlElement">The XElement to use as input.</param>
        public void ArrangeInputXml(XElement xmlElement)
        {
            if (xmlElement == null)
            {
                throw new ArgumentNullException(nameof(xmlElement));
            }

            this.ArrangeInputXml(xmlElement.ToString());       // Clone it...
        }

        /// <summary>
        /// Arrange the input XML by serialising the given object into XML.
        /// </summary>
        /// <param name="valueToSerialise">The object to serialise as input.</param>
        public void ArrangeInputXml(object valueToSerialise)
        {
            this.ArrangeInputXml(valueToSerialise.SerialiseToXml());
        }

        /// <summary>
        /// Arrange the input XML by serialising the given object into XML.
        /// </summary>
        /// <param name="valueToSerialise">The object to serialise as input.</param>
        /// <param name="xmlRootAttribute">The XML root attribute.</param>
        public void ArrangeInputXml(object valueToSerialise, XmlRootAttribute xmlRootAttribute)
        {
            this.ArrangeInputXml(valueToSerialise.SerialiseToXml(xmlRootAttribute));
        }

        /// <summary>
        /// Arrange the input XML by serialising the given object into XML.
        /// </summary>
        /// <param name="valueToSerialise">The object to serialise as input.</param>
        /// <param name="xmlAttributeOverrides">The XML attribute overrides.</param>
        public void ArrangeInputXml(object valueToSerialise, XmlAttributeOverrides xmlAttributeOverrides)
        {
            this.ArrangeInputXml(valueToSerialise.SerialiseToXml(xmlAttributeOverrides));
        }

        /// <summary>
        /// Arrange the input XML from the given XML string.
        /// </summary>
        /// <param name="xmlValue">The XML to use as input.</param>
        public void ArrangeInputXml(string xmlValue)
        {
            Logger.Log();
            Logger.Log($"The input XML for the map is: {Environment.NewLine}{xmlValue}");
            this.InputXml = xmlValue.ToXElement();
        }

        #endregion

        #endregion
    }
}
