//-----------------------------------------------------------------------
// <copyright file="ILogAsserter.cs">
//     Copyright (c) 2014-2016 Adam Craven. All rights reserved.
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

    /// <summary>
    /// Interface for a test class that performs assertions and logs out information to enhance test output readability.
    /// </summary>
    public interface ILogAsserter
    {
        /// <summary>
        /// Asserts that the value is <see cref="true" />.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="actual">The actual value to test.</param>
        void IsTrue(string itemName, bool actual);

        /// <summary>
        /// Asserts that the value is <see cref="false" />.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="actual">The actual value to test.</param>
        /// <remarks>
        /// Outputs the name of the object being asserted for better traceability in the test output.
        /// </remarks>
        void IsFalse(string itemName, bool actual);

        /// <summary>
        /// Asserts that the value is <see cref="null" />.
        /// </summary>
        /// <typeparam name="T">The type of the object on which to do the assertion.</typeparam>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="actual">The actual value to assert.</param>
        /// <remarks>
        /// Outputs the name of the object being asserted for better traceability in the test output.
        /// </remarks>
        void IsNull<T>(string itemName, T actual);

        /// <summary>
        /// Asserts that the value is NOT <see cref="null" />.
        /// </summary>
        /// <typeparam name="T">The type of the object on which to do the assertion.</typeparam>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="actual">The actual value to assert.</param>
        /// <remarks>
        /// Outputs the name of the object being asserted for better traceability in the test output.
        /// </remarks>
        void IsNotNull<T>(string itemName, T actual);

        /// <summary>
        /// Asserts the given values are equal.
        /// </summary>
        /// <typeparam name="T">The type of the object on which to do the assertion.</typeparam>
        /// <param name="itemName">Name of the field.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        void AreEqual<T>(string itemName, T expected, T actual);

        /// <summary>
        /// Asserts the given values are equal.
        /// </summary>
        /// <typeparam name="T">The type of the object on which to do the assertion.</typeparam>
        /// <param name="itemName">Name of the field.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <param name="ignoreCase">If set to <c>true</c> then the case is ignored.</param>
        void AreEqual<T>(string itemName, T expected, T actual, bool ignoreCase);

        /// <summary>
        /// Asserts the given values are NOT equal.
        /// </summary>
        /// <typeparam name="T">The type of the object on which to do the assertion.</typeparam>
        /// <param name="itemName">Name of the field.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        void AreNotEqual<T>(string itemName, T expected, T actual);

        /// <summary>
        /// Asserts the given values are NOT equal.
        /// </summary>
        /// <typeparam name="T">The type of the object on which to do the assertion.</typeparam>
        /// <param name="itemName">Name of the field.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <param name="ignoreCase">If set to <c>true</c> then the case is ignored.</param>
        void AreNotEqual<T>(string itemName, T expected, T actual, bool ignoreCase);

        /// <summary>
        /// Asserts that the given actual string contains the given expected text.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="expectedText">The expected text to be contained within the actual text.</param>
        /// <param name="actualText">The actual text that should contain the expected text.</param>
        void StringContains(string itemName, string expectedText, string actualText);

        /// <summary>
        /// Asserts that two object variables refer to the same underlying object.
        /// </summary>
        /// <typeparam name="T">The type of the object on which to do the assertion.</typeparam>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        void AreSame<T>(string itemName, T expected, T actual);

        /// <summary>
        /// Asserts that two object variables do NOT refer to the same underlying object.
        /// </summary>
        /// <typeparam name="T">The type of the object on which to do the assertion.</typeparam>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        void AreNotSame<T>(string itemName, T expected, T actual);

        /// <summary>
        /// Asserts that the given object is an instance of the given type.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="actual">The actual object.</param>
        void IsInstanceOfType(string itemName, Type expectedType, object actual);

        /// <summary>
        /// Asserts that the given object is NOT an instance of the given type.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="actual">The actual object.</param>
        void IsNotInstanceOfType(string itemName, Type expectedType, object actual);

        /// <summary>
        /// Indicates that an assertion cannot be verified.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        void Inconclusive(string message, params object[] args);

        /// <summary>
        /// Fails with the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        void Fail(string message, params object[] args);
    }
}
