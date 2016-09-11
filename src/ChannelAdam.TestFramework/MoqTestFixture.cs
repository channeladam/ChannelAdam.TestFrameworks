//-----------------------------------------------------------------------
// <copyright file="MoqTestFixture.cs">
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
    using ChannelAdam.Logging;

    using Moq;

    /// <summary>
    /// Abstract class to inherit for using Moq.
    /// </summary>
    public abstract class MoqTestFixture : TestEasy
    {
        private MockRepository mockRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoqTestFixture" /> class.
        /// </summary>
        /// <param name="logAssert">The log asserter.</param>
        protected MoqTestFixture(ILogAsserter logAssert)
            : this(MockBehavior.Loose, new SimpleConsoleLogger(), logAssert)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoqTestFixture" /> class.
        /// </summary>
        /// <param name="logger">The simple logger to use.</param>
        /// <param name="logAssert">The log asserter.</param>
        protected MoqTestFixture(ISimpleLogger logger, ILogAsserter logAssert)
            : this(MockBehavior.Loose, logger, logAssert)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoqTestFixture" /> class.
        /// </summary>
        /// <param name="mockBehaviour">The type of mock behavior.</param>
        /// <param name="logger">The simple logger to use.</param>
        /// <param name="logAssert">The log asserter.</param>
        protected MoqTestFixture(MockBehavior mockBehaviour, ISimpleLogger logger, ILogAsserter logAssert)
            : base(logger, logAssert)
        {
            this.mockRepository = new MockRepository(mockBehaviour);
        }

        #region Properties

        /// <summary>
        /// Gets my mock repository.
        /// </summary>
        /// <value>
        /// My mock repository.
        /// </value>
        public MockRepository MyMockRepository
        {
            get
            {
                return this.mockRepository;
            }
        }

        #endregion
    }
}
