namespace SampleBizTalkMaps.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://SampleBizTalkMaps.Schemas.FlatFileSchemaRequest",@"Request")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"Request"})]
    public sealed class FlatFileSchemaRequest : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://SampleBizTalkMaps.Schemas.FlatFileSchemaRequest"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://SampleBizTalkMaps.Schemas.FlatFileSchemaRequest"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:appinfo>
      <schemaEditorExtension:schemaInfo namespaceAlias=""b"" extensionClass=""Microsoft.BizTalk.FlatFileExtension.FlatFileExtension"" standardName=""Flat File"" xmlns:schemaEditorExtension=""http://schemas.microsoft.com/BizTalk/2003/SchemaEditorExtensions"" />
      <b:schemaInfo standard=""Flat File"" codepage=""65001"" default_pad_char="" "" pad_char_type=""char"" count_positions_by_byte=""false"" parser_optimization=""speed"" lookahead_depth=""3"" suppress_empty_nodes=""false"" generate_empty_nodes=""true"" allow_early_termination=""false"" early_terminate_optional_fields=""false"" allow_message_breakup_of_infix_root=""false"" compile_parse_tables=""false"" root_reference=""Request"" />
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""Request"">
    <xs:annotation>
      <xs:appinfo>
        <b:recordInfo structure=""delimited"" child_delimiter_type=""char"" child_delimiter="","" child_order=""infix"" sequence_number=""1"" preserve_delimiter_for_empty_data=""true"" suppress_trailing_delimiters=""false"" />
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:annotation>
          <xs:appinfo>
            <groupInfo sequence_number=""0"" xmlns=""http://schemas.microsoft.com/BizTalk/2003"" />
          </xs:appinfo>
        </xs:annotation>
        <xs:element name=""Key"" type=""xs:string"">
          <xs:annotation>
            <xs:appinfo>
              <b:fieldInfo justification=""left"" sequence_number=""1"" />
            </xs:appinfo>
          </xs:annotation>
        </xs:element>
        <xs:element name=""StringValue"" type=""xs:string"">
          <xs:annotation>
            <xs:appinfo>
              <b:fieldInfo justification=""left"" sequence_number=""2"" />
            </xs:appinfo>
          </xs:annotation>
        </xs:element>
        <xs:element name=""DateTimeValue"" type=""xs:string"">
          <xs:annotation>
            <xs:appinfo>
              <b:fieldInfo justification=""left"" sequence_number=""3"" />
            </xs:appinfo>
          </xs:annotation>
        </xs:element>
        <xs:element name=""IntegerValue"" type=""xs:string"">
          <xs:annotation>
            <xs:appinfo>
              <b:fieldInfo justification=""left"" sequence_number=""4"" />
            </xs:appinfo>
          </xs:annotation>
        </xs:element>
        <xs:element name=""DecimalValue"" type=""xs:string"">
          <xs:annotation>
            <xs:appinfo>
              <b:fieldInfo justification=""left"" sequence_number=""5"" />
            </xs:appinfo>
          </xs:annotation>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public FlatFileSchemaRequest() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "Request";
                return _RootElements;
            }
        }
        
        protected override object RawSchema {
            get {
                return _rawSchema;
            }
            set {
                _rawSchema = value;
            }
        }
    }
}
