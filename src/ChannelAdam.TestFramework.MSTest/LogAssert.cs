//-----------------------------------------------------------------------
// <copyright file="LogAssert.cs">
//     Copyright (c) 2014 Adam Craven. All rights reserved.
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

namespace ChannelAdam.TestFramework.MSTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ChannelAdam.Logging;

    using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Assertion helper that outputs the specified name of the object being asserted - for better traceability in the test output.
    /// </summary>
    public class LogAssert : ChannelAdam.TestFramework.ILogAsserter
    {
        private ISimpleLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogAssert"/> class.
        /// </summary>
        public LogAssert()
        {
            this.logger = new SimpleConsoleLogger();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogAssert"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public LogAssert(ISimpleLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Asserts that the value is <see cref="True" />.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="actual">The actual value to test.</param>
        public void IsTrue(string itemName, bool actual)
        {
            this.logger.Log("Asserting {0} is True", itemName);
            MSTest.Assert.IsTrue(actual, itemName + " is True");
        }

        /// <summary>
        /// Asserts that the value is <see cref="False" />.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="actual">The actual value to test.</param>
        /// <remarks>
        /// Outputs the name of the object being asserted for better traceability in the test output.
        /// </remarks>
        public void IsFalse(string itemName, bool actual)
        {
            this.logger.Log("Asserting {0} is False", itemName);
            MSTest.Assert.IsFalse(actual, itemName + " is False");
        }

        /// <summary>
        /// Asserts that the value is <see cref="null" />.
        /// </summary>
        /// <typeparam name="T">The type of the object being asserted.</typeparam>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="actual">The actual value to assert.</param>
        /// <remarks>
        /// Outputs the name of the object being asserted for better traceability in the test output.
        /// </remarks>
        public void IsNull<T>(string itemName, T actual)
        {
            this.logger.Log("Asserting {0} is Null", itemName);
            MSTest.Assert.IsNull(actual, itemName + " is Null");
        }

        /// <summary>
        /// Asserts that the value is NOT <see cref="null" />.
        /// </summary>
        /// <typeparam name="T">The type of the object being asserted.</typeparam>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="actual">The actual value to assert.</param>
        /// <remarks>
        /// Outputs the name of the object being asserted for better traceability in the test output.
        /// </remarks>
        public void IsNotNull<T>(string itemName, T actual)
        {
            this.logger.Log("Asserting {0} is NOT Null", itemName);
            MSTest.Assert.IsNotNull(actual, itemName + " is NOT Null");
        }

        /// <summary>
        /// Asserts the given values are equal.
        /// </summary>
        /// <typeparam name="T">The type of the object being asserted.</typeparam>
        /// <param name="itemName">Name of the field.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <remarks>
        /// Outputs the name of the object being asserted for better traceability in the test output.
        /// </remarks>
        public void AreEqual<T>(string itemName, T expected, T actual)
        {
            this.logger.Log("Asserting {0} is equal to: {1}", itemName, expected);
            MSTest.Assert.AreEqual<T>(expected, actual, itemName);
        }

        /// <summary>
        /// Asserts the given values are equal.
        /// </summary>
        /// <typeparam name="T">The type of the object being asserted.</typeparam>
        /// <param name="itemName">Name of the field.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <param name="ignoreCase">If set to <c>true</c> then the case is ignored.</param>
        /// <remarks>
        /// Outputs the name of the object being asserted for better traceability in the test output.
        /// </remarks>
        public void AreEqual<T>(string itemName, T expected, T actual, bool ignoreCase)
        {
            string ignore = ignoreCase ? " (ignoring case)" : string.Empty;
            this.logger.Log("Asserting {0} is equal to{1}: {2}", itemName, ignore, expected);
            MSTest.Assert.AreEqual<T>(expected, actual, itemName, ignoreCase);
        }

        /// <summary>
        /// Asserts the given values are NOT equal.
        /// </summary>
        /// <typeparam name="T">The type of the object being asserted.</typeparam>
        /// <param name="itemName">Name of the field.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <remarks>
        /// Outputs the name of the object being asserted for better traceability in the test output.
        /// </remarks>
        public void AreNotEqual<T>(string itemName, T expected, T actual)
        {
            this.logger.Log("Asserting {0} is NOT equal to: {1}", itemName, expected);
            MSTest.Assert.AreNotEqual<T>(expected, actual, itemName);
        }

        /// <summary>
        /// Asserts the given values are NOT equal.
        /// </summary>
        /// <typeparam name="T">The type of the object being asserted.</typeparam>
        /// <param name="itemName">Name of the field.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <param name="ignoreCase">If set to <c>true</c> then the case is ignored.</param>
        /// <remarks>
        /// Outputs the name of the object being asserted for better traceability in the test output.
        /// </remarks>
        public void AreNotEqual<T>(string itemName, T expected, T actual, bool ignoreCase)
        {
            string ignore = ignoreCase ? " (ignoring case)" : string.Empty;
            this.logger.Log("Asserting {0} is NOT equal to{1}: {2}", itemName, ignore, expected);
            MSTest.Assert.AreNotEqual<T>(expected, actual, itemName, ignoreCase);
        }

        /// <summary>
        /// Asserts that the given actual string contains the given expected text.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="expectedText">The expected text to be contained within the actual text.</param>
        /// <param name="actualText">The actual text that should contain the expected text.</param>
        /// <remarks>
        /// Outputs the name of the object being asserted for better traceability in the test output.
        /// </remarks>
        public void StringContains(string itemName, string expectedText, string actualText)
        {
            this.logger.Log("Asserting the text '{0}' contains the text '{1}'", itemName, actualText, expectedText);

            if (actualText == null)
            {
                MSTest.Assert.Fail("Actual text is null and does NOT contain the expected text '{0}'", expectedText);
            }
            else
            {
                MSTest.Assert.IsTrue(actualText.Contains(expectedText), "Actual text '{0}' does NOT contain expected text '{1}'", actualText, expectedText);
            }
        }

        /// <summary>
        /// Asserts that two object variables refer to the same underlying object.
        /// </summary>
        /// <typeparam name="T">The type of the object being asserted.</typeparam>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <remarks>
        /// Outputs the name of the object being asserted for better traceability in the test output.
        /// </remarks>
        public void AreSame<T>(string itemName, T expected, T actual)
        {
            this.logger.Log("Asserting {0} is same as: {1}", itemName, expected);
            MSTest.Assert.AreSame(expected, actual, itemName);
        }

        /// <summary>
        /// Asserts that two object variables do NOT refer to the same underlying object.
        /// </summary>
        /// <typeparam name="T">The type of the object being asserted.</typeparam>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <remarks>
        /// Outputs the name of the object being asserted for better traceability in the test output.
        /// </remarks>
        public void AreNotSame<T>(string itemName, T expected, T actual)
        {
            this.logger.Log("Asserting {0} is NOT same as: {1}", itemName, expected);
            MSTest.Assert.AreNotSame(expected, actual, itemName);
        }

        /// <summary>
        /// Asserts that the given object is an instance of the given type.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="actual">The actual object.</param>
        /// <remarks>
        /// Outputs the name of the object being asserted for better traceability in the test output.
        /// </remarks>
        public void IsInstanceOfType(string itemName, Type expectedType, object actual)
        {
            if (expectedType == null)
            {
                throw new ArgumentNullException("expectedType");
            }

            this.logger.Log("Asserting {0} is an instance of: {1}", itemName, expectedType.Name);
            MSTest.Assert.IsInstanceOfType(actual, expectedType, itemName);
        }

        /// <summary>
        /// Asserts that the given object is NOT an instance of the given type.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="actual">The actual object.</param>
        /// <remarks>
        /// Outputs the name of the object being asserted for better traceability in the test output.
        /// </remarks>
        public void IsNotInstanceOfType(string itemName, Type expectedType, object actual)
        {
            if (expectedType == null)
            {
                throw new ArgumentNullException("expectedType");
            }

            this.logger.Log("Asserting {0} is NOT an instance of: {1}", itemName, expectedType.Name);
            MSTest.Assert.IsNotInstanceOfType(actual, expectedType, itemName);
        }

        /// <summary>
        /// Indicates that an assertion cannot be verified.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void Inconclusive(string message, params object[] args)
        {
            this.logger.Log("Inconclusive: " + message, args);
            MSTest.Assert.Inconclusive(message, args);
        }

        /// <summary>
        /// Fails with the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void Fail(string message, params object[] args)
        {
            this.logger.Log("Failing: " + message, args);
            MSTest.Assert.Fail(message, args);
        }
    }
}
