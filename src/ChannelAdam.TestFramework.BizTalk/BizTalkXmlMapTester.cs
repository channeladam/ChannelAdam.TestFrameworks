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

    using Microsoft.XLANGs.BaseTypes;
    using Microsoft.BizTalk.TestTools;
    using Logging;

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

        public void TestMap(TransformBase map)
        {
            ValidateInputXml(map);

            var inputFileName = Path.GetTempFileName();
            CreateFile(inputFileName, InputXml.ToString());

            var outputFileName = Path.GetTempFileName();

            this.logger.Log("Executing the map " + map.GetType().Name);
            PerformTransform(map, inputFileName, outputFileName);

            this.logAssert.IsTrue("Output file exists", File.Exists(outputFileName));
            this.logger.Log("Map complete. Output file is: {0}", outputFileName);

            base.SetActualOutputXmlFromXmlFile(outputFileName);

            ValidateActualOutputXml(map);
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
            return System.Attribute.GetCustomAttributes(map.GetType(), typeof(Microsoft.XLANGs.BaseTypes.SchemaReferenceAttribute)).Cast<Microsoft.XLANGs.BaseTypes.SchemaReferenceAttribute>();
        }

        private static void ValidateXml(string xmlToValidate, IEnumerable<string> schemas, IEnumerable<SchemaReferenceAttribute> attrs)
        {
            string validationErrors = string.Empty;

            var schemaSet = new XmlSchemaSet();

            foreach (var schema in schemas)
            {
                schemaSet.Add(LoadSchema(schema, attrs));
            }

            var doc = XDocument.Parse(xmlToValidate);

            doc.Validate(schemaSet, (o, e) => { validationErrors += e.Message + Environment.NewLine; });

            if (validationErrors.Any())
            {
                throw new XmlSchemaValidationException(validationErrors);
            }
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

        private static XmlNamespaceManager CreateNamespaceManagerFromFileXmlContents(string filename)
        {
            string xmlString = File.ReadAllText(filename);
            return CreateNamespaceManagerFromXmlString(xmlString);
        }

        private static XmlNamespaceManager CreateNamespaceManagerFromXmlString(string xml)
        {
            XmlReader reader = XmlReader.Create(new StringReader(xml));
            XmlNameTable nameTable = reader.NameTable;
            return new XmlNamespaceManager(nameTable);
        }

        /// <summary>
        /// Creates a file with the given output filename from the given embedded resource in the given assembly.
        /// </summary>
        /// <param name="fileName">Name of the output file.</param>
        /// <param name="contents">Contents of the file to create.</param>
        private static void CreateFile(string fileName, string contents)
        {
            var outputDirectoryPath = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(outputDirectoryPath))
            {
                Directory.CreateDirectory(outputDirectoryPath);
            }

            File.WriteAllText(fileName, contents);
        }

        /// <summary>
        /// Perform the transform using the map.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="inputXmlFile"></param>
        /// <param name="outputXmlFile"></param>
        /// <remarks>
        /// This fixes a bug in the Microsoft code and provides the actual exception message so devs can know why a map test failed...
        /// Reference: https://shadabanwer.wordpress.com/2013/06/14/map-unit-test-does-not-work-in-biztalk-2013-because-testablemapbase-class-is-not-correct/comment-page-1/#comment-72
        /// </remarks>
        private void PerformTransform(TransformBase map, string inputXmlFile, string outputXmlFile)
        {
            try
            {
                var transform = new XslCompiledTransform();
                using (var stylesheet = new XmlTextReader(new StringReader(map.XmlContent)))
                {
                    transform.Load(stylesheet, new XsltSettings(true, true), null);
                }

                using (var input = XmlReader.Create(inputXmlFile))
                {
                    input.MoveToContent();

                    var settings = new XmlWriterSettings
                    {
                        Indent = true
                    };

                    using (var results = XmlWriter.Create(outputXmlFile, settings))
                    {
                        transform.Transform(input, map.TransformArgs, results);
                    }
                }
            }
            catch (Exception exception)
            {
                string errorMessage = string.Format("Unable to write output instance to the following <file:///{0}>.", outputXmlFile);

                var builder = new StringBuilder(errorMessage);

                for (var currentException = exception; currentException != null; currentException = currentException.InnerException)
                {
                    builder
                        .Append(Environment.NewLine)
                        .Append(currentException.ToString());
                }

                throw new BizTalkTestAssertFailException(builder.ToString());
            }
        }

        #endregion
    }
}
