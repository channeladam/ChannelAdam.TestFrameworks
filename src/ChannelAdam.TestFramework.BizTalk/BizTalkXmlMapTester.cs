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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using System.Xml.Xsl;

    using ChannelAdam.TestFramework.Xml;
    using ChannelAdam.Xml;

    using Logging;
    using Microsoft.XLANGs.BaseTypes;
    using Microsoft.BizTalk.TestTools;

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
                ValidateInputXml(map);
            }

            this.logger.Log("Executing the map " + map.GetType().Name);
            string outputXml = PerformTransform(map, this.InputXml);
            this.logAssert.IsTrue("There was output from the map", !string.IsNullOrWhiteSpace(outputXml));

            base.SetActualOutputXmlFromXmlString(outputXml);
            this.logger.Log();
            this.logger.Log("Map completed");

            if (validateOutputXml)
            {
                ValidateActualOutputXml(map);
            }
        }

        public void ValidateInputXml(TransformBase map)
        {
            var schemas = map.SourceSchemas;
            var attrs = GetSchemaReferenceAttributes(map);
            this.logger.Log("Validating the input XML");
            ValidateXml(InputXml.ToString(), schemas, attrs);
        }

        public void ValidateActualOutputXml(TransformBase map)
        {
            var schemas = map.TargetSchemas;
            var attrs = GetSchemaReferenceAttributes(map);
            this.logger.Log("Validating the actual output XML");
            ValidateXml(ActualOutputXml.ToString(), schemas, attrs);
        }

        public XLANGMessage CreateMockXLangMessage(string xmlToMock)
        {
            var xlangMessage = new Moq.Mock<XLANGMessage>();
            var xlangPart = new Moq.Mock<XLANGPart>();
            xlangPart.Setup(f => f.RetrieveAs(typeof(XmlDocument))).Returns(xmlToMock);
            xlangMessage.Setup(x => x[0]).Returns(xlangPart.Object);
            return xlangMessage.Object;
        }

        #endregion

        #region Private Methods

        private static IEnumerable<SchemaReferenceAttribute> GetSchemaReferenceAttributes(TransformBase map)
        {
            return System.Attribute.GetCustomAttributes(
                map.GetType(),
                typeof(Microsoft.XLANGs.BaseTypes.SchemaReferenceAttribute)).Cast<Microsoft.XLANGs.BaseTypes.SchemaReferenceAttribute>();
        }

        private static void ValidateXml(string xmlToValidate, IEnumerable<string> schemas, IEnumerable<SchemaReferenceAttribute> attrs)
        {
            var validationErrors = string.Empty;

            var schemaSet = new XmlSchemaSet();

            foreach (var schema in schemas)
            {
                schemaSet.Add(LoadSchema(schema, attrs));
            }

            var xmlValidator = new XmlValidator();
            xmlValidator.ValidateXml(xmlToValidate, schemaSet);
        }

        private static XmlSchema LoadSchema(string schemaName, IEnumerable<SchemaReferenceAttribute> attrs)
        {
            Type schemasType = (from a in attrs where a.Reference == schemaName select a).First().Type;
            var schemaBase = Activator.CreateInstance(schemasType) as Microsoft.XLANGs.BaseTypes.SchemaBase;

            if (schemaBase == null)
            {
                throw new Exception("Could not cast to a SchemaBase");
            }

            return schemaBase.Schema;
        }

        private static XmlNamespaceManager CreateNamespaceManagerFromXmlString(string xml)
        {
            XmlReader reader = XmlReader.Create(new StringReader(xml));
            XmlNameTable nameTable = reader.NameTable;
            return new XmlNamespaceManager(nameTable);
        }

        /// <summary>
        /// Perform the transform using the map.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="inputXml"></param>
        /// <remarks>
        /// This fixes a bug in the Microsoft code and provides the actual exception message so devs can know why a map test failed...
        /// Reference: https://shadabanwer.wordpress.com/2013/06/14/map-unit-test-does-not-work-in-biztalk-2013-because-testablemapbase-class-is-not-correct/comment-page-1/#comment-72
        /// </remarks>
        /// <returns>The resulting XML output from the map.</returns>
        private string PerformTransform(TransformBase map, XNode inputXml)
        {
            try
            {
                var transform = new XslCompiledTransform();
                using (var stylesheet = new XmlTextReader(new StringReader(map.XmlContent)))
                {
                    transform.Load(stylesheet, new XsltSettings(true, true), null);
                }

                using (var input = XmlReader.Create(new StringReader(inputXml.ToString())))
                {
                    var sb = new StringBuilder();
                    var settings = new XmlWriterSettings
                    {
                        Indent = true
                    };

                    using (var results = XmlWriter.Create(sb, settings))
                    {
                        transform.Transform(input, map.TransformArgs, results);
                    }

                    return sb.ToString();
                }
            }
            catch (Exception exception)
            {
                var sb = new StringBuilder("An error occurred while executing the transform");

                for (var currentException = exception; currentException != null; currentException = currentException.InnerException)
                {
                    sb.Append(Environment.NewLine)
                      .Append(currentException.ToString());
                }

                throw new BizTalkTestAssertFailException(sb.ToString());
            }
        }

        #endregion
    }
}