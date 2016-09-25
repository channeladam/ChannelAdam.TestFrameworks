//-----------------------------------------------------------------------
// <copyright file="BizTalkMapSchemaLoader.cs">
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
    using System.Reflection;

    using Microsoft.BizTalk.TOM;
    using Microsoft.XLANGs.BaseTypes;

    public class BizTalkMapSchemaLoader : ILoadArtifact
    {
        #region Private Fields

        private Type mapperType;

        #endregion Private Fields

        #region Public Constructors

        public BizTalkMapSchemaLoader(TransformBase map)
        {
            this.mapperType = map.GetType();
        }

        #endregion Public Constructors

        #region Public Methods

        public string GetSchemaFromLoadPath(string strLoadPath)
        {
            if (!string.IsNullOrWhiteSpace(strLoadPath))
            {
                Type schemaType = this.mapperType.Assembly.GetType(strLoadPath);
                if (schemaType != null)
                {
                    return GetXmlContent(schemaType);
                }

                foreach (AssemblyName name in this.mapperType.Assembly.GetReferencedAssemblies())
                {
                    Assembly assembly = AppDomain.CurrentDomain.Load(name);
                    if (assembly != null)
                    {
                        schemaType = assembly.GetType(strLoadPath);
                        if (schemaType != null)
                        {
                            return GetXmlContent(schemaType);
                        }
                    }
                }
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private static string GetXmlContent(Type schemaType)
        {
            SchemaBase schema = Activator.CreateInstance(schemaType) as SchemaBase;
            return schema.XmlContent;
        }

        #endregion Private Methods

    }
}
