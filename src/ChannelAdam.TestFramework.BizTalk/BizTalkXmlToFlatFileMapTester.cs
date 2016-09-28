//-----------------------------------------------------------------------
// <copyright file="BizTalkXmlToFlatFileMapTester.cs">
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

    using Logging;
    using Helpers;
    using Mapping;
    using Microsoft.XLANGs.BaseTypes;

    public class BizTalkXmlToFlatFileMapTester : MappingFromXmlToFlatFileTester
    {
        #region Constructors

        public BizTalkXmlToFlatFileMapTester(ILogAsserter logAsserter) : base(logAsserter)
        {
        }

        public BizTalkXmlToFlatFileMapTester(ISimpleLogger logger, ILogAsserter logAsserter) : base(logger, logAsserter)
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
        /// <param name="validateInputXml">if set to <c>true</c> then the input XML is validated.</param>
        /// <param name="validateOutput">if set to <c>true</c> then the output is validated.</param>
        public void TestMap(TransformBase map, bool validateInputXml, bool validateOutput)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));

            if (validateInputXml)
            {
                BizTalkXmlMapTestValidator.ValidateInputXml(map, this.InputXml, this.Logger);
            }

            Logger.Log("Executing the map " + map.GetType().Name);
            string outputXmlString = BizTalkXmlMapExecutor.PerformTransform(map, this.InputXml);
            LogAssert.IsTrue("There was output from the map", !string.IsNullOrWhiteSpace(outputXmlString));
            Logger.Log();
            Logger.Log("Map completed");

            var actualOutputXml = XElement.Parse(outputXmlString);

            if (validateOutput)
            {
                BizTalkXmlMapTestValidator.ValidateOutputXml(map, actualOutputXml, this.Logger);
            }

            var schemaTree = BizTalkMapSchemaUtility.CreateSchemaTreeAndLoadSchema(map, map.TargetSchemas[0]);
            if (schemaTree.IsStandardXML)
            {
                throw new InvalidOperationException("The map does not have a schema for converting to a flat file.");
            }
            else
            {
                this.ActualOutputFlatFileContents = BizTalkXmlFlatFileAdapter.ConvertOutputXmlToOutputFlatFileContents(map, actualOutputXml, validateOutput);
                this.Logger.Log();
                this.Logger.Log($"The actual output flat file contents from the map is: {Environment.NewLine}{this.ActualOutputFlatFileContents}");
            }
        }

        #endregion
    }
}