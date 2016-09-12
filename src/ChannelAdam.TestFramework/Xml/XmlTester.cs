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
    using System;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using Core.Reflection;
    using Core.Xml;
    using Logging;
    using Org.XmlUnit.Builder;
    using Org.XmlUnit.Diff;

    /// <summary>
    /// A helper class for testing differences between two XML sources.
    /// </summary>
    public class XmlTester
    {
        #region Fields

        private readonly ISimpleLogger logger;
        private readonly ILogAsserter logAssert;
        private readonly IComparisonFormatter comparisonFormatter;

        private XElement actualXml;
        private XElement expectedXml;

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
            this.logger = logger;
            this.logAssert = logAsserter;
            this.comparisonFormatter = comparisonFormatter;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the actual XML property is changed.
        /// </summary>
        public event EventHandler<XmlChangedEventArgs> ActualXmlChangedEvent;

        /// <summary>
        /// Occurs when expected XML property is changed.
        /// </summary>
        public event EventHandler<XmlChangedEventArgs> ExpectedXmlChangedEvent;

        #endregion

        #region Properties

        public XElement ActualXml
        {
            get
            {
                return this.actualXml;
            }

            private set
            {
                this.actualXml = value;
                this.OnActualXmlChanged(value);
            }
        }

        public XElement ExpectedXml
        {
            get
            {
                return this.expectedXml;
            }

            private set
            {
                this.expectedXml = value;
                this.OnExpectedXmlChanged(value);
            }
        }

        #endregion

        #region Public Methods

        #region Arrange Actual XML

        /// <summary>
        /// Arrange the actual XML from an embedded resource in the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly that contains the resource.</param>
        /// <param name="resourceName">The name of the resource.</param>
        public void ArrangeActualXml(Assembly assembly, string resourceName)
        {
            this.ArrangeActualXml(EmbeddedResource.GetAsString(assembly, resourceName));
        }

        /// <summary>
        /// Arrange the actual XML from the given XElement.
        /// </summary>
        /// <param name="xmlElement">The XElement to set as the input.</param>
        public void ArrangeActualXml(XElement xmlElement)
        {
            if (xmlElement == null)
            {
                throw new ArgumentNullException(nameof(xmlElement));
            }

            this.ArrangeActualXml(xmlElement.ToString());      // Clone it...
        }

        /// <summary>
        /// Arrange the actual XML by serialising the given object into XML.
        /// </summary>
        /// <param name="valueToSerialise">The object to serialise as the actual XML.</param>
        public void ArrangeActualXml(object valueToSerialise)
        {
            this.ArrangeActualXml(valueToSerialise.SerialiseToXml());
        }

        /// <summary>
        /// Arrange the actual XML by serialising the given object into XML.
        /// </summary>
        /// <param name="valueToSerialise">The object to serialise as the actual XML.</param>
        /// <param name="xmlRootAttribute">The XML root attribute.</param>
        public void ArrangeActualXml(object valueToSerialise, XmlRootAttribute xmlRootAttribute)
        {
            this.ArrangeActualXml(valueToSerialise.SerialiseToXml(xmlRootAttribute));
        }

        /// <summary>
        /// Arrange the actual XML by serialising the given object into XML.
        /// </summary>
        /// <param name="valueToSerialise">The object to serialise as the actual XML.</param>
        /// <param name="xmlAttributeOverrides">The XML attribute overrides.</param>
        public void ArrangeActualXml(object valueToSerialise, XmlAttributeOverrides xmlAttributeOverrides)
        {
            this.ArrangeActualXml(valueToSerialise.SerialiseToXml(xmlAttributeOverrides));
        }

        /// <summary>
        /// Arrange the actual XML from the given XML string.
        /// </summary>
        /// <param name="xmlValue">The XML string.</param>
        public void ArrangeActualXml(string xmlValue)
        {
            this.ActualXml = xmlValue.ToXElement();
        }

        #endregion

        #region Arrange Expected XML

        /// <summary>
        /// Arrange the expected XML from an embedded resource in the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="resourceName">Name of the resource.</param>
        public void ArrangeExpectedXml(Assembly assembly, string resourceName)
        {
            this.ArrangeExpectedXml(EmbeddedResource.GetAsString(assembly, resourceName));
        }

        /// <summary>
        /// Arrange the expected XML from the given XElement.
        /// </summary>
        /// <param name="xmlElement">The XML element.</param>
        public void ArrangeExpectedXml(XElement xmlElement)
        {
            if (xmlElement == null)
            {
                throw new ArgumentNullException(nameof(xmlElement));
            }

            this.ArrangeExpectedXml(xmlElement.ToString());     // Clone it...
        }

        /// <summary>
        /// Arrange the expected XML by serialising the given object into XML.
        /// </summary>
        /// <param name="valueToSerialise">The value to serialise as the expected XML.</param>
        public void ArrangeExpectedXml(object valueToSerialise)
        {
            this.ArrangeExpectedXml(valueToSerialise.SerialiseToXml());
        }

        /// <summary>
        /// Arrange the expected XML by serialising the given object into XML.
        /// </summary>
        /// <param name="valueToSerialise">The value to serialise as the expected XML.</param>
        /// <param name="xmlRootAttribute">The XML root attribute.</param>
        public void ArrangeExpectedXml(object valueToSerialise, XmlRootAttribute xmlRootAttribute)
        {
            this.ArrangeExpectedXml(valueToSerialise.SerialiseToXml(xmlRootAttribute));
        }

        /// <summary>
        /// Arrange the expected XML by serialising the given object into XML.
        /// </summary>
        /// <param name="valueToSerialise">The value to serialise as the expected XML.</param>
        /// <param name="xmlAttributeOverrides">The XML attribute overrides.</param>
        public void ArrangeExpectedXml(object valueToSerialise, XmlAttributeOverrides xmlAttributeOverrides)
        {
            this.ArrangeExpectedXml(valueToSerialise.SerialiseToXml(xmlAttributeOverrides));
        }

        /// <summary>
        /// Arrange the expected XML from the given XML string.
        /// </summary>
        /// <param name="xmlValue">The XML string.</param>
        public void ArrangeExpectedXml(string xmlValue)
        {
            this.ExpectedXml = xmlValue.ToXElement();
        }

        #endregion

        #region Assertions

        /// <summary>
        /// Assert the actual XML against the expected XML.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "*", Justification = "Ignored.")]
        public void AssertActualXmlEqualsExpectedXml()
        {
            Diff differences;

            this.logger.Log("Asserting actual and expected XML are equal");

            var isIdentical = this.IsIdentical(out differences);
            if (!isIdentical)
            {
                var report = differences.ToString();
                this.logger.Log("The differences are: " + Environment.NewLine + report);
            }

            this.logAssert.IsTrue("The XML is as expected", isIdentical);
            this.logger.Log("The XML is as expected");
        }

        #endregion

        #region Utility Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "As designed.")]
        public bool IsIdentical(out Diff differences)
        {
            return this.IsIdentical(this.ActualXml, this.ExpectedXml, out differences);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "As designed.")]
        public bool IsIdentical(XElement actual, XElement expected, out Diff differences)
        {
            return this.IsIdentical(actual.ToXmlNode(), expected.ToXmlNode(), out differences);
        }

        /// <summary>
        /// Determines if the given actual and expected XML is identical.
        /// </summary>
        /// <param name="actual">The actual.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="differences">The differences.</param>
        /// <returns>
        /// The XML differences.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode", Justification = "As designed.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "As designed.")]
        public bool IsIdentical(XmlNode actual, XmlNode expected, out Diff differences)
        {
            differences = DiffBuilder.Compare(Input.FromNode(expected))   // https://github.com/xmlunit/user-guide/wiki/DiffBuilder
                                    .IgnoreComments()
                                    .CheckForSimilar()      // ignore child order, namespace prefixes etc - https://github.com/xmlunit/user-guide/wiki/DifferenceEvaluator#default-differenceevaluator
                                    .WithComparisonFormatter(this.comparisonFormatter)
                                    .WithTest(Input.FromNode(actual))
                                    .Build();

            return !differences.HasDifferences();
        }

        #endregion

        #endregion

        #region Protected Change Methods

        protected virtual void OnExpectedXmlChanged(XElement value)
        {
            this.ExpectedXmlChangedEvent?.Invoke(this, new XmlChangedEventArgs(value));
        }

        protected virtual void OnActualXmlChanged(XElement value)
        {
            this.ActualXmlChangedEvent?.Invoke(this, new XmlChangedEventArgs(value));
        }

        #endregion
    }
}
