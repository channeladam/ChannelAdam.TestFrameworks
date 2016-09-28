//-----------------------------------------------------------------------
// <copyright file="MappingFromFlatFileToXmlTester.cs">
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

namespace ChannelAdam.TestFramework.Mapping
{
    using System;
    using System.Reflection;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using Abstractions;
    using ChannelAdam.Logging;
    using ChannelAdam.Reflection;
    using ChannelAdam.Xml;

    public class MappingFromFlatFileToXmlTester : MappingToXmlTesterBase, IHasInputFlatFileContents
    {
        #region Constructor / Destructor

        protected MappingFromFlatFileToXmlTester(ILogAsserter logAsserter) : base(logAsserter)
        {
        }

        protected MappingFromFlatFileToXmlTester(ISimpleLogger logger, ILogAsserter logAsserter) : base(logger, logAsserter)
        {
        }

        #endregion

        #region Properties

        public string InputFlatFileContents { get; private set; }

        #endregion

        #region Public Methods

        #region Arrange Input Flat File Contents

        /// <summary>
        /// Arrange the input flat file contents from the given string.
        /// </summary>
        /// <param name="flatFileContents">The string to use as the input.</param>
        public void ArrangeInputFlatFileContents(string flatFileContents)
        {
            Logger.Log();
            Logger.Log($"The input flat file contents for the map is: {Environment.NewLine}{flatFileContents}");
            this.InputFlatFileContents = flatFileContents;
        }

        /// <summary>
        /// Arrange the input flat file contents from an embedded resource in the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly that contains the resource.</param>
        /// <param name="resourceName">The name of the resource.</param>
        public void ArrangeInputFlatFileContents(Assembly assembly, string resourceName)
        {
            this.ArrangeInputFlatFileContents(EmbeddedResource.GetAsString(assembly, resourceName));
        }

        #endregion

        #endregion
    }
}
