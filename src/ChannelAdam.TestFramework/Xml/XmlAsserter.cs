//-----------------------------------------------------------------------
// <copyright file="XmlAsserter.cs">
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
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using Abstractions;
    using ChannelAdam.Logging;

    public class XmlAsserter
    {
        private readonly ISimpleLogger logger;
        private readonly ILogAsserter logAssert;

        #region Constructors

        public XmlAsserter(ILogAsserter logAsserter) : this(new SimpleConsoleLogger(), logAsserter)
        {
        }

        public XmlAsserter(ISimpleLogger logger, ILogAsserter logAsserter)
        {
            this.logger = logger;
            this.logAssert = logAsserter;
        }

        #endregion

        public void XPathValueEquals(string description, string xpath, XNode rootElement, string expected)
        {
            this.XPathValueEquals(description, xpath, rootElement, null, expected);
        }

        public void XPathValueEquals(string description, string xpath, XNode rootElement, XmlNamespaceManager namespaceManager, string expected)
        {
            var elements = rootElement.XPathSelectElements(xpath, namespaceManager);
            if (!elements.Any())
            {
                this.logAssert.Fail("{0}: Could not find XPath {1}", description, xpath);
            }
            else
            {
                this.logAssert.AreEqual(description, elements.First().Value, expected);
            }
        }

        public void XPathValuesAreEqual(string description, string xpath, XNode expectedElements, XNode actualElements)
        {
            this.XPathValuesAreEqual(description, xpath, expectedElements, xpath, actualElements);
        }

        public void XPathValuesAreEqual(string description, string expectedXpath, XNode expectedElements, string actualXpath, XNode actualElements)
        {
            var expectedElement = expectedElements.XPathSelectElement(expectedXpath);
            var actualElement = actualElements.XPathSelectElement(actualXpath);

            if (expectedElement == null)
            {
                this.logger.Log("Asserting {0} is: null", description);
                if (actualElement == null)
                {
                    return;
                }
                else
                {
                    this.logAssert.Fail("Actual element should have been null, but had a value of {0}", actualElement.Value);
                }
            }

            if (actualElement == null)
            {
                this.logAssert.Fail("Actual element should NOT have been null. Expected value was: {0}", expectedElement.Value);
            }

            var expected = expectedElement.Value;
            var actual = actualElement.Value;

            this.logAssert.AreEqual(description, expected, actual);
        }

        public void XPathValuesAreEqual(string description, string expectedXpath, XNode expectedElements, XmlNamespaceManager expectedNamespaceManager, string actualXpath, XNode actualElements, XmlNamespaceManager actualNamespaceManager)
        {
            var expected = expectedElements.XPathSelectElement(expectedXpath, expectedNamespaceManager).Value;
            var actual = actualElements.XPathSelectElement(actualXpath, actualNamespaceManager).Value;

            this.logAssert.AreEqual(description, expected, actual);
        }

        public void AreEqual(XElement expectedXml, XElement actualXml)
        {
            this.AreEqual(expectedXml, actualXml, null);
        }

        public void AreEqual(XElement expectedXml, XElement actualXml, IXmlFilter xmlFilter)
        {
            var tester = new XmlTester(this.logAssert);
            tester.ArrangeExpectedXml(expectedXml);
            tester.ArrangeActualXml(actualXml);
            tester.AssertActualXmlEqualsExpectedXml(xmlFilter);
        }
    }
}
