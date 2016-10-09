using System;
using ChannelAdam.TestFramework.BizTalk;
using ChannelAdam.TestFramework.BizTalk.Helpers;
using ChannelAdam.TestFramework.MSTest;
using SampleBizTalkMapHelpers;
using SampleBizTalkMaps.Maps;
using TechTalk.SpecFlow;
using System.Collections.Generic;
using Moq;
using SampleBizTalkMapHelpers.Abstractions;

namespace BehaviourSpecs
{
    [Binding]
    public class BizTalkMappingUnitSteps : MoqTestFixture
    {
        private static readonly Type ThisType = typeof(BizTalkMappingUnitSteps);
        private static readonly string TestDataNamespacePrefix = ThisType.Namespace + ".TestData.";
        private static readonly string TestDataXmlInput = TestDataNamespacePrefix + "XmlInput.xml";
        private static readonly string TestDataExpectedXmlOutput = TestDataNamespacePrefix + "ExpectedXmlOutput.xml";
        private static readonly string TestDataFlatFileInput = TestDataNamespacePrefix + "FlatFileInput.csv";
        private static readonly string TestDataExpectedFlatFileOutput = TestDataNamespacePrefix + "ExpectedFlatFileOutput.csv";

        private BizTalkXmlToXmlMapTester xmlToXmlMapTester;
        private BizTalkXmlToFlatFileMapTester xmlToFlatFileMapTester;
        private BizTalkFlatFileToXmlMapTester flatFileToXmlMapTester;
        private BizTalkFlatFileToFlatFileMapTester flatFileToFlatFileMapTester;

        private Mock<IGuidHelper> mockGuidHelper;
        private List<XsltExtensionObjectDescriptor> mocksOfExternalAssemblyClassesUsedInMaps;

        #region Before / After

        [BeforeScenario]
        public void BeforeScenario()
        {
            this.xmlToXmlMapTester = new BizTalkXmlToXmlMapTester(base.LogAssert);
            this.xmlToFlatFileMapTester = new BizTalkXmlToFlatFileMapTester(base.LogAssert);
            this.flatFileToXmlMapTester = new BizTalkFlatFileToXmlMapTester(base.LogAssert);
            this.flatFileToFlatFileMapTester = new BizTalkFlatFileToFlatFileMapTester(base.LogAssert);

            CreateMockGuidHelper();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Logger.Log();
            Logger.Log("Verifying all mock expectations were met");
            MyMockRepository.VerifyAll();
        }

        #endregion

        #region Given

        [Given(@"xml input for an xml to xml map")]
        public void GivenXmlInputForAnXmlToXmlMap()
        {
            this.xmlToXmlMapTester.ArrangeInputXml(ThisType.Assembly, TestDataXmlInput);
            this.xmlToXmlMapTester.ArrangeExpectedOutputXml(ThisType.Assembly, TestDataExpectedXmlOutput);
        }

        [Given(@"xml input for an xml to flat file map")]
        public void GivenXmlInputForAnXmlToFlatFileMap()
        {
            this.xmlToFlatFileMapTester.ArrangeInputXml(ThisType.Assembly, TestDataXmlInput);
            this.xmlToFlatFileMapTester.ArrangeExpectedOutputFlatFileContents(ThisType.Assembly, TestDataExpectedFlatFileOutput);
        }

        [Given(@"flat file input for a flat file to xml map")]
        public void GivenFlatFileInputForAFlatFileToXmlMap()
        {
            this.flatFileToXmlMapTester.ArrangeInputFlatFileContents(ThisType.Assembly, TestDataFlatFileInput);
            this.flatFileToXmlMapTester.ArrangeExpectedOutputXml(ThisType.Assembly, TestDataExpectedXmlOutput);
        }

        [Given(@"flat file input for a flat file to flat file map")]
        public void GivenFlatFileInputForAFlatFileToFlatFileMap()
        {
            this.flatFileToFlatFileMapTester.ArrangeInputFlatFileContents(ThisType.Assembly, TestDataFlatFileInput);
            this.flatFileToFlatFileMapTester.ArrangeExpectedOutputFlatFileContents(ThisType.Assembly, TestDataExpectedFlatFileOutput);
        }

        #endregion

        #region When

        [When(@"the xml to xml map is performed")]
        public void WhenTheXmlToXmlMapIsPerformed()
        {
            this.xmlToXmlMapTester.TestMap(new MapXmlToXml(), this.mocksOfExternalAssemblyClassesUsedInMaps);
        }

        [When(@"the xml to flat file map is performed")]
        public void WhenTheXmlToFlatFileMapIsPerformed()
        {
            this.xmlToFlatFileMapTester.TestMap(new MapXmlToFlatFile(), this.mocksOfExternalAssemblyClassesUsedInMaps);
        }

        [When(@"the flat file to xml map is performed")]
        public void WhenTheFlatFileToXmlMapIsPerformed()
        {
            this.flatFileToXmlMapTester.TestMap(new MapFlatFileToXml(), this.mocksOfExternalAssemblyClassesUsedInMaps);
        }

        [When(@"the flat file to flat file map is performed")]
        public void WhenTheFlatFileToFlatFileMapIsPerformed()
        {
            this.flatFileToFlatFileMapTester.TestMap(new MapFlatFileToFlatFile(), this.mocksOfExternalAssemblyClassesUsedInMaps);
        }

        #endregion

        #region Then

        [Then(@"the xml output from an xml to xml map is correct")]
        public void ThenTheXmlOutputFromAnXmlToXmlMapIsCorrect()
        {
            this.xmlToXmlMapTester.AssertActualOutputXmlEqualsExpectedOutputXml();
        }

        [Then(@"the flat file output from the xml to flat file map is correct")]
        public void ThenTheFlatFileOutputFromTheXmlToFlatFileMapIsCorrect()
        {
            this.xmlToFlatFileMapTester.AssertActualOutputFlatFileContentsEqualsExpectedOutputFlatFileContents();
        }

        [Then(@"the xml output from the flat file to xml map is correct")]
        public void ThenTheXmlOutputFromTheFlatFileToXmlMapIsCorrect()
        {
            this.flatFileToXmlMapTester.AssertActualOutputXmlEqualsExpectedOutputXml();
        }

        [Then(@"the flat file output from a flat file to flat file map is correct")]
        public void ThenTheFlatFileOutputFromAFlatFileToFlatFileMapIsCorrect()
        {
            this.flatFileToFlatFileMapTester.AssertActualOutputFlatFileContentsEqualsExpectedOutputFlatFileContents();
        }

        #endregion

        #region Private Methods

        private void CreateMockGuidHelper()
        {
            Logger.Log("Setting up mock to return 'FAKE_GUID' every time GuidHelper.NewGuid() is called");

            this.mockGuidHelper = base.MyMockRepository.Create<IGuidHelper>();
            this.mockGuidHelper.Setup(m => m.NewGuid())
                .Returns("FAKE_GUID")
                .Verifiable();

            // Mock the external assembly GuidHelper class used in the map ;)
            this.mocksOfExternalAssemblyClassesUsedInMaps = new List<XsltExtensionObjectDescriptor>
                {
                    new XsltExtensionObjectDescriptor(typeof(GuidHelper), this.mockGuidHelper.Object)
                };
        }

        #endregion
    }
}
