//-----------------------------------------------------------------------
// <copyright file="MoqTestFixture.cs">
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

    using ChannelAdam.Logging;
    using ChannelAdam.TestFramework;

    using Moq;

    /// <summary>
    /// Abstract class to inherit for using Moq.
    /// </summary>
    public abstract class MoqTestFixture : ChannelAdam.TestFramework.MoqTestFixture
    {
        private static ILogAsserter logAssert = new LogAssert();

        /// <summary>
        /// Initializes a new instance of the <see cref="MoqTestFixture" /> class.
        /// </summary>
        protected MoqTestFixture() : base(logAssert)
        {
        }
    }
}
