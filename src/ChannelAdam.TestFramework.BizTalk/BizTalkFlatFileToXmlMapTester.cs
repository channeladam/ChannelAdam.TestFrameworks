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
    using System.Reflection;
    using System.Xml.Linq;

    using Helpers;
    using Logging;
    using Mapping;
    using Microsoft.XLANGs.BaseTypes;
    using Reflection;

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
        /// <param name="validateInput">if set to <c>true</c> then the input flat file contents and converted XML is validated.</param>
        /// <param name="validateOutput">if set to <c>true</c> then the output is validated.</param>
        public void TestMap(TransformBase map, bool validateInput, bool validateOutput)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));

            XNode inputXml = BizTalkXmlFlatFileAdapter.ConvertInputFlatFileContentsToInputXml(map, this.InputFlatFileContents);

            if (validateInput)
            {
                BizTalkXmlMapTestValidator.ValidateInputXml(map, inputXml, this.Logger);
            }

            Logger.Log("Executing the map " + map.GetType().Name);
            string outputXml = BizTalkXmlMapExecutor.PerformTransform(map, inputXml);
            LogAssert.IsTrue("There was output from the map", !string.IsNullOrWhiteSpace(outputXml));
            Logger.Log();
            Logger.Log("Map completed");

            base.SetActualOutputXmlFromXmlString(outputXml);

            if (validateOutput)
            {
                BizTalkXmlMapTestValidator.ValidateOutputXml(map, this.ActualOutputXml, this.Logger);
            }
        }

        #endregion
    }
}