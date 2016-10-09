//-----------------------------------------------------------------------
// <copyright file="BizTalkFlatFileToXmlMapTester.cs">
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

namespace ChannelAdam.TestFramework.BizTalk
{
    using System;
    using System.Xml.Linq;

    using Helpers;
    using Logging;
    using Mapping;
    using Microsoft.XLANGs.BaseTypes;
    using System.Collections.Generic;

    public class BizTalkFlatFileToXmlMapTester : MappingFromFlatFileToXmlTester
    {
        #region Constructors

        public BizTalkFlatFileToXmlMapTester(ILogAsserter logAsserter) : base(logAsserter)
        {
        }

        public BizTalkFlatFileToXmlMapTester(ISimpleLogger logger, ILogAsserter logAsserter) : base(logger, logAsserter)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the map and performs validation on both the input flat file contents and output XML.
        /// </summary>
        /// <param name="map">The map.</param>
        public void TestMap(TransformBase map)
        {
            TestMap(map, true, true);
        }

        /// <summary>
        /// Tests the map and performs validation on both the input flat file contents and output XML.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="xsltExtensionObjectOverrides">The overrides for the XSLT arguments - allowing you to mock external assembly methods.</param>
        public void TestMap(TransformBase map, IEnumerable<XsltExtensionObjectDescriptor> xsltExtensionObjectOverrides)
        {
            TestMap(map, xsltExtensionObjectOverrides, true, true);
        }

        /// <summary>
        /// Tests the map.
        /// </summary>
        /// <param name="map">The map to execute.</param>
        /// <param name="validateInput">if set to <c>true</c> then the input flat file contents and converted XML is validated.</param>
        /// <param name="validateOutputXml">if set to <c>true</c> then the output XML is validated.</param>
        public void TestMap(TransformBase map, bool validateInput, bool validateOutputXml)
        {
            TestMap(map, null, validateInput, validateOutputXml);
        }

        /// <summary>
        /// Tests the map.
        /// </summary>
        /// <param name="map">The map to execute.</param>
        /// <param name="xsltExtensionObjectOverrides">The overrides for the XSLT arguments - allowing you to mock external assembly methods.</param>
        /// <param name="validateInput">if set to <c>true</c> then the input flat file contents and converted XML is validated.</param>
        /// <param name="validateOutputXml">if set to <c>true</c> then the output XML is validated.</param>
        public void TestMap(TransformBase map, IEnumerable<XsltExtensionObjectDescriptor> xsltExtensionObjectOverrides, bool validateInput, bool validateOutputXml)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));

            XNode inputXml = BizTalkXmlFlatFileAdapter.ConvertInputFlatFileContentsToInputXml(map, this.InputFlatFileContents);

            if (validateInput)
            {
                BizTalkXmlMapTestValidator.ValidateInputXml(map, inputXml, this.Logger);
            }

            Logger.Log("Executing the BizTalk map (flat file to XML) " + map.GetType().Name);
            string outputXml = BizTalkXmlMapExecutor.PerformTransform(map, xsltExtensionObjectOverrides, inputXml);
            LogAssert.IsTrue("XML output from the BizTalk map exists", !string.IsNullOrWhiteSpace(outputXml));
            Logger.Log();
            Logger.Log("BizTalk map (flat file to XML) execution completed");

            base.SetActualOutputXmlFromXmlString(outputXml);

            if (validateOutputXml)
            {
                BizTalkXmlMapTestValidator.ValidateOutputXml(map, this.ActualOutputXml, this.Logger);
            }
        }

        #endregion
    }
}