//-----------------------------------------------------------------------
// <copyright file="ExpectedExceptionDescriptor.cs">
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

namespace ChannelAdam.TestFramework
{
    using System;

    using ChannelAdam.Logging;

    /// <summary>
    /// Describes attributes of an expected exception.
    /// </summary>
    public class ExpectedExceptionDescriptor
    {
        private ISimpleLogger logger;
        private Type expectedType;
        private string messageShouldContainText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedExceptionDescriptor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ExpectedExceptionDescriptor(ISimpleLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets or sets the message should contain text.
        /// </summary>
        /// <value>
        /// The message should contain text.
        /// </value>
        public string MessageShouldContainText
        { 
            get
            {
                return this.messageShouldContainText;
            }

            set
            {
                this.messageShouldContainText = value;
                this.logger.Log("Setting ExpectedException.MessageShouldContainText: '{0}'", this.messageShouldContainText);
            }
        }

        /// <summary>
        /// Gets or sets the expected type of the exception.
        /// </summary>
        /// <value>
        /// The expected type of the exception.
        /// </value>
        public Type ExpectedType
        {
            get
            {
                return this.expectedType;
            }

            set
            {
                this.expectedType = value;
                this.logger.Log("Setting ExpectedException.ExpectedType: '{0}'", this.expectedType == null ? "null" : this.expectedType.Name);
            }
        }

        /// <summary>
        /// Gets a value indicating whether there is an expected exception defined.
        /// </summary>
        /// <value>
        /// <c>true</c> if there is an expected exception; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpected
        {
            get
            {
                return this.expectedType != null || !string.IsNullOrWhiteSpace(this.messageShouldContainText);
            }
        }
    }
}