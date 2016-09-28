//-----------------------------------------------------------------------
// <copyright file="TextTester.cs">
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

namespace ChannelAdam.TestFramework
{
    using System;
    using System.Linq;
    using System.Reflection;

    using DiffPlex;
    using DiffPlex.DiffBuilder;
    using DiffPlex.DiffBuilder.Model;
    using Logging;
    using Reflection;
    using Text;
    using Text.Abstractions;

    public class TextTester
    {
        #region Fields

        private readonly ISimpleLogger logger;
        private readonly ILogAsserter logAssert;

        private string actualText;
        private string expectedText;
        private ITextDifferenceFormatter differenceFormatter;
        private DiffPaneModel differences;

        #endregion

        #region Constructors

        public TextTester(ILogAsserter logAsserter) : this(new SimpleConsoleLogger(), logAsserter)
        {
        }

        public TextTester(ISimpleLogger logger, ILogAsserter logAsserter) : this(logger, logAsserter, new DefaultTextDifferenceFormatter())
        {
        }

        public TextTester(ILogAsserter logAsserter, ITextDifferenceFormatter differenceFormatter) : this(new SimpleConsoleLogger(), logAsserter, differenceFormatter)
        {
        }

        public TextTester(ISimpleLogger logger, ILogAsserter logAsserter, ITextDifferenceFormatter differenceFormatter)
        {
            this.logger = logger;
            this.logAssert = logAsserter;
            this.differenceFormatter = differenceFormatter;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the actual text property is changed.
        /// </summary>
        public event EventHandler<TextChangedEventArgs> ActualTextChangedEvent;

        /// <summary>
        /// Occurs when expected text property is changed.
        /// </summary>
        public event EventHandler<TextChangedEventArgs> ExpectedTextChangedEvent;

        #endregion

        #region Properties

        public string ActualText
        {
            get
            {
                return this.actualText;
            }

            private set
            {
                this.actualText = value;
                this.OnActualTextChanged(value);
            }
        }

        public string ExpectedText
        {
            get
            {
                return this.expectedText;
            }

            private set
            {
                this.expectedText = value;
                this.OnExpectedTextChanged(value);
            }
        }

        public DiffPaneModel Differences
        {
            get { return this.differences; }
        }

        #endregion

        #region Public Methods

        #region Arrange Actual Text

        /// <summary>
        /// Arrange the actual text from an embedded resource in the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly that contains the resource.</param>
        /// <param name="resourceName">The name of the resource.</param>
        public void ArrangeActualText(Assembly assembly, string resourceName)
        {
            this.ArrangeActualText(EmbeddedResource.GetAsString(assembly, resourceName));
        }

        /// <summary>
        /// Arrange the actual text from the given string.
        /// </summary>
        /// <param name="text">The string to set as the actual text.</param>
        public void ArrangeActualText(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            this.ActualText = text;
        }

        #endregion

        #region Arrange Expected Text

        /// <summary>
        /// Arrange the expected text from an embedded resource in the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="resourceName">Name of the resource.</param>
        public void ArrangeExpectedText(Assembly assembly, string resourceName)
        {
            this.ArrangeExpectedText(EmbeddedResource.GetAsString(assembly, resourceName));
        }

        /// <summary>
        /// Arrange the expected text from the given string.
        /// </summary>
        /// <param name="text">The text string.</param>
        public void ArrangeExpectedText(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            this.ExpectedText = text;
        }

        #endregion

        #region Assertions

        /// <summary>
        /// Assert the actual text against the expected text.
        /// </summary>
        public virtual void AssertActualTextEqualsExpectedText()
        {
            this.logger.Log("Asserting actual and expected text are equal");

            var isEqual = this.IsEqual(this.ExpectedText, this.ActualText);
            if (!isEqual)
            {
                var report = this.differenceFormatter.FormatDifferences(this.Differences);
                this.logger.Log("The differences are: " + Environment.NewLine + report);
            }

            this.logAssert.IsTrue("The text is as expected", isEqual);
            this.logger.Log("The text is as expected");
        }

        #endregion

        #region Utility Methods

        public bool IsEqual()
        {
            return this.IsEqual(this.ExpectedText, this.ActualText);
        }

        /// <summary>
        /// Determines if the given actual and expected text is equivalent.
        /// </summary>
        /// <param name="expected">The expected text.</param>
        /// <param name="actual">The actual text.</param>
        /// <returns>
        /// The text differences.
        /// </returns>
        public virtual bool IsEqual(string expected, string actual)
        {
            var differ = new Differ();
            var inlineBuilder = new InlineDiffBuilder(differ);
            this.differences = inlineBuilder.BuildDiffModel(expected, actual);

            return this.differences.Lines.All(l => l.Type == ChangeType.Unchanged);
        }

        #endregion

        #endregion

        #region Protected Change Methods

        protected virtual void OnExpectedTextChanged(string value)
        {
            this.ExpectedTextChangedEvent?.Invoke(this, new TextChangedEventArgs(value));
        }

        protected virtual void OnActualTextChanged(string value)
        {
            this.ActualTextChangedEvent?.Invoke(this, new TextChangedEventArgs(value));
        }

        #endregion
    }
}
