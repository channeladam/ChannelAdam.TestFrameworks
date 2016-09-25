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
    using System.IO;
    using System.Linq;

    using ChannelAdam.TestFramework.Xml;

    using Logging;
    using Microsoft.XLANGs.BaseTypes;
    using Microsoft.BizTalk.TestTools;
    using Reflection;
    using System.Reflection;
    using Microsoft.BizTalk.TOM;
    using Helpers;
    using System.Xml.Linq;

    public class BizTalkFlatFileToXmlMapTester : XmlMapTesterBase
    {
        #region Constructors

        public BizTalkFlatFileToXmlMapTester(ILogAsserter logAsserter) : base(logAsserter)
        {
        }

        public BizTalkFlatFileToXmlMapTester(ISimpleLogger logger, ILogAsserter logAsserter) : base(logger, logAsserter)
        {
        }

        #endregion

        #region Properties

        public string InputFlatFileContents { get; private set; }

        #endregion

        #region Public Methods

        #region Arrange Input

        /// <summary>
        /// Arrange the input flat file from an embedded resource in the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly that contains the resource.</param>
        /// <param name="resourceName">The name of the resource.</param>
        public void ArrangeInputFlatFile(Assembly assembly, string resourceName)
        {
            this.ArrangeInputFlatFile(EmbeddedResource.GetAsString(assembly, resourceName));
        }

        /// <summary>
        /// Arrange the input flat file from the given string.
        /// </summary>
        /// <param name="value">The string to use as input.</param>
        public void ArrangeInputFlatFile(string value)
        {
            Logger.Log();
            Logger.Log($"The input flat file contents for the map is: {Environment.NewLine}{value}");
            this.InputFlatFileContents = value;
        }

        #endregion Arrange Input

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
            XNode inputXml = ConvertFlatFileContentsToXml(map, this.InputFlatFileContents);

            if (validateInput)
            {
                BizTalkXmlMapTestValidator.ValidateInputXml(map, inputXml, this.Logger);
            }

            Logger.Log("Executing the map " + map.GetType().Name);
            string outputXml = BizTalkXmlMapTestExecutor.PerformTransform(map, inputXml);
            LogAssert.IsTrue("There was output from the map", !string.IsNullOrWhiteSpace(outputXml));


            base.SetActualOutputXmlFromXmlString(outputXml);
            Logger.Log();
            Logger.Log("Map completed");

            if (validateOutput)
            {
                BizTalkXmlMapTestValidator.ValidateActualOutputXml(map, this.ActualOutputXml, this.Logger);
            }
        }

        #endregion

        #region Private Methods

        private static XNode ConvertFlatFileContentsToXml(TransformBase map, string inputFlatFileContents)
        {
            ITOMErrorInfo[] validationErrors;
            string errorMessage = null;
            string convertedXml = null;

            var tempInputFlatFileFilename = Path.GetTempFileName();

            try
            {
                CMapperSchemaTree sourceSchemaTree = CreateSchemaTree(map);

                string sourceSchemaClassName = map.SourceSchemas[0];
                if (!sourceSchemaTree.LoadFromDotNetPath(sourceSchemaClassName, null, out errorMessage))
                {
                    throw new BizTalkTestAssertFailException($"An error occurred while loading the source schema type '{sourceSchemaClassName}': {errorMessage}");
                }

                File.WriteAllText(tempInputFlatFileFilename, inputFlatFileContents);

                if (!sourceSchemaTree.ValidateInstance(tempInputFlatFileFilename, OutputInstanceType.Native, sourceSchemaClassName, out validationErrors, out convertedXml))
                {
                    var messages = validationErrors.Select(e => $"Line:{e.LineNumber} Position:{e.LinePosition} {(e.IsWarning ? "Warning: " : "Error: ")} {e.ErrorInfo}");
                    var message = string.Join(". " + Environment.NewLine, messages);
                    throw new BizTalkTestAssertFailException($"An error occurred while parsing/converting the contents of the flat file to XML: {Environment.NewLine}{message}");
                }
            }
            finally
            {
                if (File.Exists(tempInputFlatFileFilename))
                {
                    File.Delete(tempInputFlatFileFilename);
                }
            }

            return XElement.Parse(convertedXml);
        }

        private static CMapperSchemaTree CreateSchemaTree(TransformBase map)
        {
            var schemaLoader = new BizTalkMapSchemaLoader(map);
            var schemaResolveHandler = new SchemaResolver();
            schemaResolveHandler.CustomResolution(schemaLoader);

            var schemaTree = new CMapperSchemaTree();
            schemaTree.SetSchemaResolveHandler(schemaResolveHandler);

            return schemaTree;
        }

        #endregion
    }
}