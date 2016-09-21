namespace ChannelAdam.TestFramework.BehaviourSpecs
{
    using TechTalk.SpecFlow;
    using ChannelAdam.TestFramework.MSTest;
    using ChannelAdam.TestFramework.Xml;

    [Binding]
    public class XmlTestingUnitSteps : MoqTestFixture {

        #region Fields

        private XmlTester xmlTester;
        private bool isEqual;

        #endregion

        #region Before/After

        [BeforeScenario]
        public void BeforeScenario()
        {
            this.xmlTester = new XmlTester(base.LogAssert);
        }

        #endregion

        #region Given

        [Given(@"two xml samples with the same namespace urls but different namespace prefixes")]
        public void GivenTwoXmlSamplesWithTheSameNamespaceUrlsButDifferentNamespacePrefixes()
        {
            this.xmlTester.ArrangeExpectedXml(@"<a xmlns=""http://wwww.com""><b>hi</b><c>c</c></a>");
            this.xmlTester.ArrangeActualXml(@"<ns0:a xmlns:ns0=""http://wwww.com""><ns0:c>c</ns0:c><ns0:b>hi</ns0:b></ns0:a>");
        }

        [Given(@"two xml samples with the same child nodes but in a different order")]
        public void GivenTwoXmlSamplesWithTheSameChildNodesButInADifferentOrder()
        {
            this.xmlTester.ArrangeExpectedXml(@"<a><b>hi</b><c>c</c></a>");
            this.xmlTester.ArrangeActualXml(@"<a><c>c</c><b>hi</b></a>");
        }

        [Given(@"two xml samples with the different elements")]
        public void GivenTwoXmlSamplesWithTheDifferentElements()
        {
            this.xmlTester.ArrangeExpectedXml(@"<a><b>hi</b></a>");
            this.xmlTester.ArrangeActualXml(@"<a><c>c</c></a>");
        }

        [Given(@"two xml samples with the same elements but a different value")]
        public void GivenTwoXmlSamplesWithTheSameElementsButADifferentValue()
        {
            this.xmlTester.ArrangeExpectedXml(@"<a><b>hi</b><c>c</c></a>");
            this.xmlTester.ArrangeActualXml(@"<a><b>oh</b><c>c</c></a>");
        }

        #endregion

        #region When

        [When(@"the two xml samples are compared")]
        public void WhenTheTwoXmlSamplesAreCompared()
        {
            Logger.Log("Xml #1: {0}", this.xmlTester.ExpectedXml);
            Logger.Log("Xml #2: {0}", this.xmlTester.ActualXml);
            this.isEqual = xmlTester.IsEqual();
        }

        #endregion

        #region Then

        [Then(@"the two xml samples are treated as equal")]
        public void ThenTheTwoXmlSamplesAreTreatedAsEqual()
        {
            LogAssert.IsTrue("Xml samples are equal", this.isEqual);
        }

        [Then(@"the two xml samples are treated as different")]
        public void ThenTheTwoXmlSamplesAreTreatedAsDifferent()
        {
            LogAssert.IsTrue("Xml samples are different", !this.isEqual);
        }

        #endregion
    }
}