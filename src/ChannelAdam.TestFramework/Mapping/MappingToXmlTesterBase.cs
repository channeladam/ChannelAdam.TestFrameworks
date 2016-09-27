//-----------------------------------------------------------------------
// <copyright file="MappingToXmlTesterBase.cs">
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

namespace ChannelAdam.TestFramework.Mapping
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using Abstractions;
    using ChannelAdam.Logging;
    using Xml;
    using Xml.Abstractions;

    public abstract class MappingToXmlTesterBase : IHasExpectedOutputXml, IHasActualOutputXml
    {
        #region Fields

        private readonly XmlTester xmlTester;

        #endregion

        #region Constructor / Destructor

        protected MappingToXmlTesterBase(ILogAsserter logAsserter) : this(new SimpleConsoleLogger(), logAsserter)
        {
        }

        protected MappingToXmlTesterBase(ISimpleLogger logger, ILogAsserter logAsserter)
        {
            this.Logger = logger;
            this.LogAssert = logAsserter;
            this.xmlTester = new XmlTester(logAsserter);
            this.xmlTester.ActualXmlChangedEvent += this.XmlTester_ActualXmlChangedEvent;
            this.xmlTester.ExpectedXmlChangedEvent += this.XmlTester_ExpectedXmlChangedEvent;
        }

        ~MappingToXmlTesterBase()
        {
            this.xmlTester.ActualXmlChangedEvent -= this.XmlTester_ActualXmlChangedEvent;
            this.xmlTester.ExpectedXmlChangedEvent -= this.XmlTester_ExpectedXmlChangedEvent;
        }

        #endregion

        #region Properties

        public XElement ActualOutputXml
        {
            get { return this.xmlTester.ActualXml; }

            set { this.xmlTester.ArrangeActualXml(value); }
        }

        public XElement ExpectedOutputXml
        {
            get { return this.xmlTester.ExpectedXml; }
        }

        protected ISimpleLogger Logger { get; private set; }

        protected ILogAsserter LogAssert { get; private set; }

        #endregion

        #region Public Methods

        #region Arrange Expected Output XML

        /// <summary>
        /// Arrange the expected output XML from an embedded resource in the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly that contains the resource.</param>
        /// <param name="resourceName">The resource name.</param>
        public void ArrangeExpectedOutputXml(Assembly assembly, string resourceName)
        {
            this.xmlTester.ArrangeExpectedXml(assembly, resourceName);
        }

        /// <summary>
        /// Arrange the expected output XML from the given XElement.
        /// </summary>
        /// <param name="xmlElement">The XElement to use as expected output.</param>
        public void ArrangeExpectedOutputXml(XElement xmlElement)
        {
            this.xmlTester.ArrangeExpectedXml(xmlElement);
        }

        /// <summary>
        /// Arrange the expected output XML by serialising the given object into XML.
        /// </summary>
        /// <param name="valueToSerialise">The object to serialise as XML to be used as the expected output.</param>
        public void ArrangeExpectedOutputXml(object valueToSerialise)
        {
            this.xmlTester.ArrangeExpectedXml(valueToSerialise);
        }

        /// <summary>
        /// Arrange the expected output XML by serialising the given object into XML.
        /// </summary>
        /// <param name="valueToSerialise">The object to serialise as XML to be used as the expected output.</param>
        /// <param name="xmlRootAttribute">The XML root attribute.</param>
        public void ArrangeExpectedOutputXml(object valueToSerialise, XmlRootAttribute xmlRootAttribute)
        {
            this.xmlTester.ArrangeExpectedXml(valueToSerialise, xmlRootAttribute);
        }

        /// <summary>
        /// Arrange the expected output XML by serialising the given object into XML.
        /// </summary>
        /// <param name="valueToSerialise">The object to serialise as XML to be used as the expected output.</param>
        /// <param name="xmlAttributeOverrides">The XML attribute overrides.</param>
        public void ArrangeExpectedOutputXml(object valueToSerialise, XmlAttributeOverrides xmlAttributeOverrides)
        {
            this.xmlTester.ArrangeExpectedXml(valueToSerialise, xmlAttributeOverrides);
        }

        /// <summary>
        /// Arrange the expected output XML from the given XML string.
        /// </summary>
        /// <param name="xmlValue">The XML to be used as the expected output.</param>
        public void ArrangeExpectedOutputXml(string xmlValue)
        {
            this.xmlTester.ArrangeExpectedXml(xmlValue);
        }

        #endregion

        #region Set Actual Output XML

        /// <summary>
        /// Sets the actual output XML from XML file.
        /// </summary>
        /// <param name="fileName">The filename.</param>
        public void SetActualOutputXmlFromXmlFile(string fileName)
        {
            this.SetActualOutputXmlFromXmlString(File.ReadAllText(fileName));
        }

        /// <summary>
        /// Sets the actual output XML from XML string.
        /// </summary>
        /// <param name="xmlValue">The XML string.</param>
        public void SetActualOutputXmlFromXmlString(string xmlValue)
        {
            this.xmlTester.ArrangeActualXml(xmlValue);
        }

        #endregion

        #region Assertions

        /// <summary>
        /// Assert the Actual Output XML against the Expected Output XML.
        /// </summary>
        public void AssertActualOutputXmlEqualsExpectedOutputXml()
        {
            this.xmlTester.AssertActualXmlEqualsExpectedXml();
        }

        /// <summary>
        /// Assert the Actual Output XML against the Expected Output XML, ignoring the elements specified by the given XML filter.
        /// </summary>
        /// <param name="xmlFilter">The XML filter to be applied to ignore specified elements from the assertion.</param>
        public void AssertActualOutputXmlEqualsExpectedOutputXml(IXmlFilter xmlFilter)
        {
            this.xmlTester.AssertActualXmlEqualsExpectedXml(xmlFilter);
        }

        #endregion

        #endregion

        #region Private Methods

        private void XmlTester_ActualXmlChangedEvent(object sender, XmlChangedEventArgs e)
        {
            this.Logger.Log();
            this.Logger.Log($"The actual output XML from the map is: {Environment.NewLine}{e.Xml}");
        }

        private void XmlTester_ExpectedXmlChangedEvent(object sender, XmlChangedEventArgs e)
        {
            this.Logger.Log();
            this.Logger.Log($"The expected output XML of the map is: {Environment.NewLine}{e.Xml}");
        }

        #endregion
    }
}
