//-----------------------------------------------------------------------
// <copyright file="XmlTester.cs">
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
    using Core.Reflection;
    using Core.Xml;
    using Logging;

    using Org.XmlUnit.Builder;
    using Org.XmlUnit.Diff;

    using System;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;

    public class XmlTester
    {
        #region Fields

        private readonly ISimpleLogger _logger;
        private readonly ILogAsserter _logAsserter;
        private readonly IComparisonFormatter _comparisonFormatter;

        private XElement _actualXml;
        private XElement _expectedXml;

        #endregion

        #region Constructors

        public XmlTester(ILogAsserter logAsserter) : this(new SimpleConsoleLogger(), logAsserter)
        {
        }

        public XmlTester(ISimpleLogger logger, ILogAsserter logAsserter) : this(logger, logAsserter, new DefaultComparisonFormatter())
        {
        }

        public XmlTester(ILogAsserter logAsserter, IComparisonFormatter comparisonFormatter) : this(new SimpleConsoleLogger(), logAsserter, comparisonFormatter)
        {
        }

        public XmlTester(ISimpleLogger logger, ILogAsserter logAsserter, IComparisonFormatter comparisonFormatter)
        {
            _logger = logger;
            _logAsserter = logAsserter;
            _comparisonFormatter = comparisonFormatter;
        }

        #endregion

        #region Events

        public event EventHandler<XmlChangedEventArgs> ActualXmlChangedEvent;
        public event EventHandler<XmlChangedEventArgs> ExpectedXmlChangedEvent;

        #endregion

        #region Properties

        public XElement ActualXml
        {
            get { return _actualXml; }
            private set
            {
                _actualXml = value;
                OnActualXmlChanged(value);
            }
        }

        public XElement ExpectedXml
        {
            get { return _expectedXml; }
            private set
            {
                _expectedXml = value;
                OnExpectedXmlChanged(value);
            }
        }

        #endregion

        #region Public Methods

        #region Arrange Actual XML

        /// <summary>
        /// Arrange the actual XML from an embedded resource in the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        public void ArrangeActualXml(Assembly assembly, string resourceName)
        {
            ArrangeActualXml(EmbeddedResource.GetAsString(assembly, resourceName));
        }

        /// <summary>
        /// Arrange the actual XML from the given XElement.
        /// </summary>
        /// <param name="xElement"></param>
        public void ArrangeActualXml(XElement xElement)
        {
            ArrangeActualXml(xElement.ToString());      // Clone it...
        }

        /// <summary>
        /// Arrange the actual XML by serialising the given object into XML.
        /// </summary>
        /// <param name="objectToSerialise"></param>
        public void ArrangeActualXml(object objectToSerialise)
        {
            ArrangeActualXml(objectToSerialise.SerialiseToXml());
        }

        /// <summary>
        /// Arrange the actual XML from the given XML string.
        /// </summary>
        /// <param name="xmlString"></param>
        public void ArrangeActualXml(string xmlString)
        {
            ActualXml = xmlString.ToXElement();
        }

        #endregion

        #region Arrange Expected XML

        /// <summary>
        /// Arrange the expected XML from an embedded resource in the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        public void ArrangeExpectedXml(Assembly assembly, string resourceName)
        {
            ArrangeExpectedXml(EmbeddedResource.GetAsString(assembly, resourceName));
        }

        /// <summary>
        /// Arrange the expected XML from the given XElement.
        /// </summary>
        /// <param name="xElement"></param>
        public void ArrangeExpectedXml(XElement xElement)
        {
            ArrangeExpectedXml(xElement.ToString());     // Clone it...
        }

        /// <summary>
        /// Arrange the expected XML by serialising the given object into XML.
        /// </summary>
        /// <param name="objectToSerialise"></param>
        public void ArrangeExpectedXml(object objectToSerialise)
        {
            ArrangeExpectedXml(objectToSerialise.SerialiseToXml());
        }

        /// <summary>
        /// Arrange the expected XML from the given XML string.
        /// </summary>
        /// <param name="xmlString"></param>
        public void ArrangeExpectedXml(string xmlString)
        {
            ExpectedXml = xmlString.ToXElement();
        }

        #endregion

        #region Change Methods

        protected virtual void OnExpectedXmlChanged(XElement value)
        {
            ExpectedXmlChangedEvent?.Invoke(this, new XmlChangedEventArgs(value));
        }

        protected virtual void OnActualXmlChanged(XElement value)
        {
            ActualXmlChangedEvent?.Invoke(this, new XmlChangedEventArgs(value));
        }

        #endregion

        #region Assertions

        /// <summary>
        /// Assert the actual XML against the expected XML.
        /// </summary>
        public void AssertActualXmlEqualsExpectedXml()
        {
            Diff differences;

            _logger.Log("Asserting actual and expected XML are equal");

            bool identical = IsIdentical(out differences);
            if (!identical)
            {
                string report = differences.ToString();
                _logger.Log("The differences are: " + Environment.NewLine + report);
            }

            _logAsserter.IsTrue("The XML is as expected", identical);
            _logger.Log("The XML is as expected");
        }

        #endregion

        #region Utility Methods

        public bool IsIdentical(out Diff differences)
        {
            return IsIdentical(ActualXml, ExpectedXml, out differences);
        }

        public bool IsIdentical(XElement actual, XElement expected, out Diff differences)
        {
            return IsIdentical(actual.ToXmlNode(), expected.ToXmlNode(), out differences);
        }

        /// <summary>
        /// Determines if the given actual and expected xml is identical.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="differences"></param>
        /// <returns>The xml differences.</returns>
        public bool IsIdentical(XmlNode actual, XmlNode expected, out Diff differences)
        {
            differences = DiffBuilder.Compare(Input.FromNode(expected))   // https://github.com/xmlunit/user-guide/wiki/DiffBuilder
                                    .IgnoreComments()
                                    .CheckForSimilar()      // ignore child order, namespace prefixes etc - https://github.com/xmlunit/user-guide/wiki/DifferenceEvaluator#default-differenceevaluator
                                    .WithComparisonFormatter(_comparisonFormatter)
                                    .WithTest(Input.FromNode(actual))
                                    .Build();

            return !differences.HasDifferences();
        }

        #endregion

        #endregion
    }
}
