//-----------------------------------------------------------------------
// <copyright file="BizTalkXmlFlatFileAdapter.cs">
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
    using System.Xml.Linq;

    using Microsoft.BizTalk.TOM;
    using Microsoft.XLANGs.BaseTypes;
    using System.Linq;
    using System;
    using System.IO;

    public static class BizTalkXmlFlatFileAdapter
    {
        public static XNode ConvertInputFlatFileContentsToXml(TransformBase map, string inputFlatFileContents)
        {
            string sourceSchemaClassName = map.SourceSchemas[0];
            CMapperSchemaTree sourceSchemaTree = BizTalkMapSchemaUtility.CreateSchemaTreeAndLoadSchema(map, sourceSchemaClassName);
            string convertedXml = BizTalkXmlMapTestValidator.ValidateFlatFileContents(inputFlatFileContents, sourceSchemaTree, sourceSchemaClassName);

            return XElement.Parse(convertedXml);
        }

        public static string ConvertOutputXmlToFlatFileContents(TransformBase map, XNode outputXml, bool validateFlatFile)
        {
            string flatFileContentsResult = null;
            ITOMErrorInfo[] creationErrors = null;

            string targetSchemaClassName = map.TargetSchemas[0];
            CMapperSchemaTree targetSchemaTree = BizTalkMapSchemaUtility.CreateSchemaTreeAndLoadSchema(map, targetSchemaClassName);

            var tempXmlFilename = Path.GetTempFileName();
            var tempFlatFileFilename = Path.GetTempFileName();

            try
            {
                File.WriteAllText(tempXmlFilename, outputXml.ToString());

                if (!targetSchemaTree.CreateNativeInstanceFromXMLInstance(tempXmlFilename, tempFlatFileFilename, out creationErrors))
                {
                    var messages = creationErrors.Select(e => $"Line:{e.LineNumber} Position:{e.LinePosition} {(e.IsWarning ? "Warning: " : "Error: ")} {e.ErrorInfo}");
                    var message = string.Join(". " + Environment.NewLine, messages);
                    throw new ApplicationException($"An error occurred while converting from XML to a flat file format: {Environment.NewLine}{message}");
                }

                if (validateFlatFile)
                {
                    BizTalkXmlMapTestValidator.ValidateFlatFile(tempFlatFileFilename, targetSchemaTree, targetSchemaClassName);
                }

                flatFileContentsResult = File.ReadAllText(tempFlatFileFilename);
            }
            finally
            {
                if (File.Exists(tempFlatFileFilename))
                {
                    File.Delete(tempFlatFileFilename);
                }

                if (File.Exists(tempXmlFilename))
                {
                    File.Delete(tempXmlFilename);
                }
            }

            return flatFileContentsResult;
        }
    }
}
