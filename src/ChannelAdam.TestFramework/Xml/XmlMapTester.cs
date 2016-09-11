//-----------------------------------------------------------------------
// <copyright file="XmlMapTesterBase.cs">
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
    using ChannelAdam.Core.Reflection;
    using ChannelAdam.Core.Xml;
    using ChannelAdam.Logging;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;

    public class XmlMapTester
    {
        #region Fields

        private readonly ISimpleLogger _logger;
        private readonly XmlTester _xmlTester;

        #endregion

        #region Constructor / Destructor

        protected XmlMapTester(ILogAsserter logAsserter) : this(new SimpleConsoleLogger(), logAsserter)
        {
        }

        protected XmlMapTester(ISimpleLogger logger, ILogAsserter logAsserter)
        {
            _logger = logger;
            _xmlTester = new XmlTester(logAsserter);
            _xmlTester.ActualXmlChangedEvent += _xmlTester_ActualXmlChangedEvent;
            _xmlTester.ExpectedXmlChangedEvent += _xmlTester_ExpectedXmlChangedEvent;
        }

        ~XmlMapTester()
        {
            _xmlTester.ActualXmlChangedEvent -= _xmlTester_ActualXmlChangedEvent;
            _xmlTester.ExpectedXmlChangedEvent -= _xmlTester_ExpectedXmlChangedEvent;
        }

        #endregion

        #region Properties

        public XElement InputXml { get; private set; }

        public XElement ActualOutputXml
        {
            get
            {
                return _xmlTester.ActualXml;
            }

            set
            {
                _xmlTester.ArrangeActualXml(value);
            }
        }

        public XElement ExpectedOutputXml
        {
            get
            {
                return _xmlTester.ExpectedXml;
            }
        }

        #endregion

        #region Public Methods

        #region Arrange Input XML

        /// <summary>
        /// Arrange the input XML from an embedded resource in the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        public void ArrangeInputXml(Assembly assembly, string resourceName)
        {
            ArrangeInputXml(EmbeddedResource.GetAsString(assembly, resourceName));
        }

        /// <summary>
        /// Arrange the input XML from the given XElement.
        /// </summary>
        /// <param name="xElement"></param>
        public void ArrangeInputXml(XElement xElement)
        {
            ArrangeInputXml(xElement.ToString());       // Clone it...
        }

        /// <summary>
        /// Arrange the input XML by serialising the given object into XML.
        /// </summary>
        /// <param name="objectToSerialise"></param>
        public void ArrangeInputXml(object objectToSerialise)
        {
            ArrangeInputXml(objectToSerialise.SerialiseToXml());
        }

        /// <summary>
        /// Arrange the input XML from the given XML string.
        /// </summary>
        /// <param name="xmlString"></param>
        public void ArrangeInputXml(string xmlString)
        {
            _logger.Log();
            _logger.Log($"The input XML for the map is: {Environment.NewLine}{xmlString}");
            InputXml = xmlString.ToXElement();
        }

        #endregion

        #region Arrange Expected Output XML

        /// <summary>
        /// Arrange the expected output XML from an embedded resource in the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        public void ArrangeExpectedOutputXml(Assembly assembly, string resourceName)
        {
            _xmlTester.ArrangeExpectedXml(assembly, resourceName);
        }

        /// <summary>
        /// Arrange the expected output XML from the given XElement.
        /// </summary>
        /// <param name="xElement"></param>
        public void ArrangeExpectedOutputXml(XElement xElement)
        {
            _xmlTester.ArrangeExpectedXml(xElement);
        }

        /// <summary>
        /// Arrange the expected output XML by serialising the given object into XML.
        /// </summary>
        /// <param name="objectToSerialise"></param>
        public void ArrangeExpectedOutputXml(object objectToSerialise)
        {
            _xmlTester.ArrangeExpectedXml(objectToSerialise);
        }

        /// <summary>
        /// Arrange the expected output XML from the given XML string.
        /// </summary>
        /// <param name="xmlString"></param>
        public void ArrangeExpectedOutputXml(string xmlString)
        {
            _xmlTester.ArrangeExpectedXml(xmlString);
        }

        #endregion

        #region Set Actual Output XML

        public void SetActualOutputXmlFromXmlFile(string filename)
        {
            SetActualOutputXmlFromXmlString(File.ReadAllText(filename));
        }

        public void SetActualOutputXmlFromXmlString(string xmlString)
        {
            _xmlTester.ArrangeActualXml(xmlString);
        }

        #endregion

        #region Assertions

        /// <summary>
        /// Assert the Actual Output XML against the Expected Output XML.
        /// </summary>
        public void AssertActualOutputXmlEqualsExpectedOutputXml()
        {
            _xmlTester.AssertActualXmlEqualsExpectedXml();
        }

        #endregion

        #endregion

        #region Private Methods

        private void _xmlTester_ExpectedXmlChangedEvent(object sender, XmlChangedEventArgs e)
        {
            _logger.Log();
            _logger.Log($"The expected output XML of the map is: {Environment.NewLine}{e.Xml}");
        }

        private void _xmlTester_ActualXmlChangedEvent(object sender, XmlChangedEventArgs e)
        {
            _logger.Log();
            _logger.Log($"The actual output XML from the map is: {Environment.NewLine}{e.Xml}");
        }

        #endregion
    }
}
