//-----------------------------------------------------------------------
// <copyright file="IgnoreElementTagNameDifferenceEvaluator.cs">
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

namespace ChannelAdam.TestFramework.Xml
{
    using System;

    using Org.XmlUnit.Diff;

    public static class IgnoreElementTagNameDifferenceEvaluator
    {
        /// <summary>
        /// A difference evaluator for XmlUnit that ignores the "element tag name" error.
        /// </summary>
        /// <param name="comparison">The comparison.</param>
        /// <param name="outcome">The outcome of the comparison.</param>
        /// <returns>The new outcome of the comparison.</returns>
        public static ComparisonResult Evaluate(Comparison comparison, ComparisonResult outcome)
        {
            if (comparison == null)
            {
                throw new ArgumentNullException(nameof(comparison));
            }

            // Only evaluate differences!
            if (outcome != ComparisonResult.DIFFERENT)
            {
                return outcome;
            }

            // Only check for the element tag name error.
            if (comparison.Type == ComparisonType.ELEMENT_TAG_NAME)
            {
                var expectedNode = comparison.ControlDetails.Target;
                var actualNode = comparison.TestDetails.Target;

                if (expectedNode.LocalName == actualNode.LocalName &&
                    expectedNode.NamespaceURI == actualNode.NamespaceURI)
                {
                    return ComparisonResult.SIMILAR;
                }
            }

            return outcome;
        }
    }
}