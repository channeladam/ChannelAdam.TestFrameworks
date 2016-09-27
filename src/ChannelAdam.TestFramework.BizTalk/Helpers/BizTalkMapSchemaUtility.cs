//-----------------------------------------------------------------------
// <copyright file="BizTalkMapSchemaUtility.cs">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Schema;
    using System.Reflection;

    using Microsoft.BizTalk.TOM;
    using Microsoft.XLANGs.BaseTypes;
    using System.Xml.Xsl;
    using System.Xml;
    using System.IO;

    public static class BizTalkMapSchemaUtility
    {
        #region Public Methods

        public static XslCompiledTransform LoadStylesheetFromMap(TransformBase map)
        {
            return LoadStylesheetFromMap(map, null);
        }

        public static XslCompiledTransform LoadStylesheetFromMap(TransformBase map, XmlResolver stylesheetResolver)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));

            var transform = new XslCompiledTransform();
            using (var stylesheet = new XmlTextReader(new StringReader(map.XmlContent)))
            {
                transform.Load(stylesheet, new XsltSettings(true, true), stylesheetResolver);
            }

            return transform;
        }

        public static CMapperSchemaTree CreateSchemaTree(TransformBase map)
        {
            var schemaLoader = new BizTalkMapSchemaArtifactLoader(map);
            var schemaResolveHandler = new SchemaResolver();
            schemaResolveHandler.CustomResolution(schemaLoader);

            var schemaTree = new CMapperSchemaTree();
            schemaTree.SetSchemaResolveHandler(schemaResolveHandler);

            return schemaTree;
        }

        public static CMapperSchemaTree CreateSchemaTreeAndLoadSchema(TransformBase map, string schemaClassName)
        {
            string errorMessage = null;

            CMapperSchemaTree schemaTree = BizTalkMapSchemaUtility.CreateSchemaTree(map);

            if (!schemaTree.LoadFromDotNetPath(schemaClassName, null, out errorMessage))
            {
                throw new TypeLoadException($"An error occurred while loading the schema type '{schemaClassName}': {errorMessage}");
            }

            return schemaTree;
        }

        public static Type FindSchemaInReferencedAssemblies(string schemaTypeNameToLoad, Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            Type schemaType = null;

            foreach (AssemblyName referencedAssemblyName in assembly.GetReferencedAssemblies())
            {
                Assembly referencedAssembly = AppDomain.CurrentDomain.Load(referencedAssemblyName);
                if (referencedAssembly != null)
                {
                    schemaType = referencedAssembly.GetType(schemaTypeNameToLoad);
                    if (schemaType != null)
                    {
                        break;
                    }
                }
            }

            return schemaType;
        }

        public static Type FindSchemaViaAssembly(string schemaTypeNameToLoad, Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            Type schemaType = assembly.GetType(schemaTypeNameToLoad);
            if (schemaType == null)
            {
                schemaType = FindSchemaInReferencedAssemblies(schemaTypeNameToLoad, assembly);
            }

            return schemaType;
        }

        public static IEnumerable<SchemaReferenceAttribute> GetSchemaReferenceAttributes(TransformBase map)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));

            return map.GetType().GetCustomAttributes(typeof(Microsoft.XLANGs.BaseTypes.SchemaReferenceAttribute), false)
                .Cast<Microsoft.XLANGs.BaseTypes.SchemaReferenceAttribute>();
        }

        public static XmlSchema LoadSchema(string schemaName, IEnumerable<SchemaReferenceAttribute> schemaReferenceAttributes)
        {
            Type schemaType = schemaReferenceAttributes.First(a => a.Reference == schemaName).Type;
            var schemaBase = LoadSchemaBase(schemaType);

            return schemaBase.Schema;
        }

        public static XmlSchema LoadSchema(Type schemaType)
        {
            var schemaBase = LoadSchemaBase(schemaType);

            return schemaBase.Schema;
        }

        public static SchemaBase LoadSchemaBase(Type schemaType)
        {
            if (schemaType == null) throw new ArgumentNullException(nameof(schemaType));

            var schemaBase = Activator.CreateInstance(schemaType) as SchemaBase;

            if (schemaBase == null)
            {
                throw new ArgumentException($"Could not cast schema from Type '{schemaType.Name}' to Type SchemaBase");
            }

            return schemaBase;
        }

        #endregion Public Methods
    }
}
