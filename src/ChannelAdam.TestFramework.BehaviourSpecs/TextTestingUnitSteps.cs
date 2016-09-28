namespace ChannelAdam.TestFramework.BehaviourSpecs
{
    using TechTalk.SpecFlow;
    using ChannelAdam.TestFramework.MSTest;
    using ChannelAdam.TestFramework.Xml;
    using Text;

    [Binding]
    [Scope(Feature = "TextTesting")]
    public class TextTestingUnitSteps : MoqTestFixture
    {
        #region Fields

        private TextTester textTester;
        private bool isEqual;

        #endregion

        #region Before/After

        [BeforeScenario]
        public void BeforeScenario()
        {
            this.textTester = new TextTester(base.LogAssert);
        }

        #endregion

        #region Given

        [Given(@"two text samples with the same words")]
        public void GivenTwoTextSamplesWithTheSameWords()
        {
            this.textTester.ArrangeExpectedText(
@"The brown bull
was seen laughing
over the blue moon
with a bottle of whiskey
attached to its horn.");

            this.textTester.ArrangeActualText(this.textTester.ExpectedText);
        }

        [Given(@"two text samples with the different words")]
        public void GivenTwoTextSamplesWithTheDifferentWords()
        {
            this.textTester.ArrangeExpectedText(
@"The brown bull
was seen laughing
over the blue moon
with a bottle of whiskey
attached to its horn.");

            this.textTester.ArrangeActualText(
@"The brown bull
was not seen laughing
over the blue moon
with a dry bottle of whiskey
attached to its horn.");
        }

        #endregion

        #region When

        [When(@"the two text samples are compared")]
        public void WhenTheTwoTextSamplesAreCompared()
        {
            Logger.Log("Text #1: {0}", this.textTester.ExpectedText);
            Logger.Log("Text #2: {0}", this.textTester.ActualText);
            this.isEqual = textTester.IsEqual();
        }

        #endregion

        #region Then

        [Then(@"the two text samples are treated as the same")]
        public void ThenTheTwoTextSamplesAreTreatedAsTheSame()
        {
            this.textTester.AssertActualTextEqualsExpectedText();
        }

        [Then(@"the two text samples are treated as different")]
        public void ThenTheTwoTextSamplesAreTreatedAsDifferent()
        {
            LogAssert.IsTrue("Text samples are different", !this.isEqual);

            this.ExpectedException.MessageShouldContainText = "The text is as expected";
            Try(() => this.textTester.AssertActualTextEqualsExpectedText());

            AssertExpectedException();
        }

        #endregion
    }
}