//-----------------------------------------------------------------------
// <copyright file="BizTalkXmlMapTestValidator.cs">
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
    using System.Xml.Linq;
    using System.Xml.Schema;

    using ChannelAdam.Xml;

    using Logging;
    using Microsoft.XLANGs.BaseTypes;
    using Microsoft.BizTalk.TestTools;

    public static class BizTalkXmlMapTestValidator
    {
        #region Public Methods

        public static void ValidateInputXml(TransformBase map, XNode inputXml, ISimpleLogger logger)
        {
            var attrs = GetSchemaReferenceAttributes(map);
            logger.Log("Validating the input XML");
            ValidateXml(inputXml.ToString(), map.SourceSchemas, attrs);
        }

        public static void ValidateActualOutputXml(TransformBase map, XNode actualOutputXml, ISimpleLogger logger)
        {
            var attrs = GetSchemaReferenceAttributes(map);
            logger.Log("Validating the actual output XML");
            ValidateXml(actualOutputXml.ToString(), map.TargetSchemas, attrs);
        }

        #endregion

        #region Private Methods

        private static IEnumerable<SchemaReferenceAttribute> GetSchemaReferenceAttributes(TransformBase map)
        {
            return System.Attribute.GetCustomAttributes(
                map.GetType(),
                typeof(Microsoft.XLANGs.BaseTypes.SchemaReferenceAttribute)).Cast<Microsoft.XLANGs.BaseTypes.SchemaReferenceAttribute>();
        }

        private static void ValidateXml(string xmlToValidate, IEnumerable<string> schemas, IEnumerable<SchemaReferenceAttribute> attrs)
        {
            var validationErrors = string.Empty;

            var schemaSet = new XmlSchemaSet();

            foreach (var schema in schemas)
            {
                schemaSet.Add(LoadSchema(schema, attrs));
            }

            var xmlValidator = new XmlValidator();
            xmlValidator.ValidateXml(xmlToValidate, schemaSet);
        }

        private static XmlSchema LoadSchema(string schemaName, IEnumerable<SchemaReferenceAttribute> attrs)
        {
            Type schemasType = attrs.First(a => a.Reference == schemaName).Type;
            var schemaBase = Activator.CreateInstance(schemasType) as Microsoft.XLANGs.BaseTypes.SchemaBase;

            if (schemaBase == null)
            {
                throw new BizTalkTestAssertFailException("Could not cast to a SchemaBase");
            }

            return schemaBase.Schema;
        }

        #endregion
    }
}