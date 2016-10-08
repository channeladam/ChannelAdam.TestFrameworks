namespace SampleBizTalkMaps.Schemas {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://SampleBizTalkMaps.Schemas.XmlResponse",@"Response")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"Response"})]
    public sealed class XmlResponse : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://SampleBizTalkMaps.Schemas.XmlResponse"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://SampleBizTalkMaps.Schemas.XmlResponse"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""Response"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""Key"" type=""xs:string"" />
        <xs:element name=""StringValue"" type=""xs:string"" />
        <xs:element name=""DateTimeValue"" type=""xs:dateTime"" />
        <xs:element name=""IntegerValue"" type=""xs:integer"" />
        <xs:element name=""DecimalValue"" type=""xs:decimal"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public XmlResponse() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "Response";
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
