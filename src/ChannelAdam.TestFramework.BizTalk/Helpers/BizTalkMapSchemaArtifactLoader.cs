//-----------------------------------------------------------------------
// <copyright file="BizTalkMapSchemaArtifactLoader.cs">
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

namespace ChannelAdam.TestFramework.BizTalk.Helpers
{
    using System;

    using Microsoft.BizTalk.TOM;
    using Microsoft.XLANGs.BaseTypes;

    public class BizTalkMapSchemaArtifactLoader : ILoadArtifact
    {
        #region Private Fields

        private readonly Type mapperType;

        #endregion Private Fields

        #region Public Constructors

        public BizTalkMapSchemaArtifactLoader(TransformBase map)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));

            this.mapperType = map.GetType();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Load a schema from the assembly containing the map.
        /// </summary>
        /// <param name="strLoadPath">This is the schema type name to load.</param>
        /// <returns></returns>
        public string GetSchemaFromLoadPath(string strLoadPath)
        {
            if (!string.IsNullOrWhiteSpace(strLoadPath))
            {
                Type schemaType = BizTalkMapSchemaUtility.FindSchemaViaAssembly(strLoadPath, this.mapperType.Assembly);
                if (schemaType != null)
                {
                    return BizTalkMapSchemaUtility.LoadSchemaBase(schemaType).XmlContent;
                }
            }

            return null;
        }

        #endregion Public Methods
    }
}
