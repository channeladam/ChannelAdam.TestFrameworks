//-----------------------------------------------------------------------
// <copyright file="BizTalkXmlMapTester.cs">
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
    using ChannelAdam.Logging;
    using ChannelAdam.TestFramework.Xml;

    using Helpers;

    using Microsoft.XLANGs.BaseTypes;

    public class BizTalkXmlMapTester : XmlMapTester
    {
        #region Fields

        private ISimpleLogger logger;
        private ILogAsserter logAssert;

        #endregion

        #region Constructors

        public BizTalkXmlMapTester(ILogAsserter logAsserter) : this(new SimpleConsoleLogger(), logAsserter)
        {
        }

        public BizTalkXmlMapTester(ISimpleLogger logger, ILogAsserter logAsserter) : base(logger, logAsserter)
        {
            this.logger = logger;
            this.logAssert = logAsserter;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests the map and performs validation on both the input and output XML.
        /// </summary>
        /// <param name="map">The map.</param>
        public void TestMap(TransformBase map)
        {
            TestMap(map, true, true);
        }

        /// <summary>
        /// Tests the map.
        /// </summary>
        /// <param name="map">The map to execute.</param>
        /// <param name="validateInputXml">if set to <c>true</c> then the input XML is validated.</param>
        /// <param name="validateOutputXml">if set to <c>true</c> then the output XML is validated.</param>
        public void TestMap(TransformBase map, bool validateInputXml, bool validateOutputXml)
        {
            if (validateInputXml)
            {
                BizTalkXmlMapTestValidator.ValidateInputXml(map, this.InputXml, this.Logger);
            }

            this.logger.Log("Executing the map " + map.GetType().Name);
            string outputXml = BizTalkXmlMapTestExecutor.PerformTransform(map, this.InputXml);
            this.logAssert.IsTrue("There was output from the map", !string.IsNullOrWhiteSpace(outputXml));

            base.SetActualOutputXmlFromXmlString(outputXml);
            this.logger.Log();
            this.logger.Log("Map completed");

            if (validateOutputXml)
            {
                BizTalkXmlMapTestValidator.ValidateActualOutputXml(map, this.ActualOutputXml, this.Logger);
            }
        }

        #endregion
    }
}