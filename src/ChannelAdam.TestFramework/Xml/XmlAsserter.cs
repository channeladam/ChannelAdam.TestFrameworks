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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using ChannelAdam.Logging;

    public class XmlAsserter
    {
        private ISimpleLogger logger;
        private ILogAsserter logAssert;

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

        public void XPathValueEquals(string description, string xpath, XElement rootElement, string expected)
        {
            XPathValueEquals(description, xpath, rootElement, null, expected);
        }

        public void XPathValueEquals(string description, string xpath, XElement rootElement, XmlNamespaceManager namespaceManager, string expected)
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

        public void XPathValuesAreEqual(string description, string xpath, XElement expectedElements, XElement actualElements)
        {
            XPathValuesAreEqual(description, xpath, expectedElements, xpath, actualElements);
        }

        public void XPathValuesAreEqual(string description, string expectedXpath, XElement expectedElements, string actualXpath, XElement actualElements)
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

        public void XPathValuesAreEqual(string description, string expectedXpath, XElement expectedElements, XmlNamespaceManager expectedNamespaceManager, string actualXpath, XElement actualElements, XmlNamespaceManager actualNamespaceManager)
        {
            var expected = expectedElements.XPathSelectElement(expectedXpath, expectedNamespaceManager).Value;
            var actual = actualElements.XPathSelectElement(actualXpath, actualNamespaceManager).Value;

            this.logAssert.AreEqual(description, expected, actual);
        }

        // TODO: rename these methods.
        //
        //public void DecendantsAreEqual(XElement expectedXml, XElement actualXml, string rootNodeLocalName)
        //{
        //    DecendantsAreEqual(expectedXml, actualXml, rootNodeLocalName, new List<string>());
        //}

        //public void DecendantsAreEqual(XElement expectedXml, XElement actualXml, string rootNodeLocalName, string excludeNodeLocalName)
        //{
        //    DecendantsAreEqual(expectedXml, actualXml, rootNodeLocalName, new List<string> { excludeNodeLocalName });
        //}

        //public void DecendantsAreEqual(XElement expectedXml, XElement actualXml, string rootNodeLocalName, List<string> excludeNodeLocalNames)
        //{
        //    var expected = expectedXml.DescendantsAndSelf().Single(e => e.Name.LocalName == rootNodeLocalName);
        //    var actual = actualXml.DescendantsAndSelf().Single(e => e.Name.LocalName == rootNodeLocalName);

        //    AreEqual(expected, actual, excludeNodeLocalNames);
        //}

        public void AreEqual(XElement expectedXml, XElement actualXml)
        {
            AreEqual(expectedXml, actualXml, new List<string>());
        }

        public void AreEqual(XElement expectedXml, XElement actualXml, string excludeNodeLocalName)
        {
            AreEqual(expectedXml, actualXml, new List<string> { excludeNodeLocalName });
        }

        public void AreEqual(XElement expectedXml, XElement actualXml, List<string> excludeNodeLocalNames)
        {
            var expected = new XElement(expectedXml);
            var actual = new XElement(actualXml);

            expected.DescendantsAndSelf().Where(p => excludeNodeLocalNames.Contains(p.Name.LocalName)).Remove();
            actual.DescendantsAndSelf().Where(p => excludeNodeLocalNames.Contains(p.Name.LocalName)).Remove();

            var tester = new XmlTester(this.logAssert);
            tester.ArrangeExpectedXml(expected);
            tester.ArrangeActualXml(actual);
            tester.AssertActualXmlEqualsExpectedXml();
        }
    }
}
