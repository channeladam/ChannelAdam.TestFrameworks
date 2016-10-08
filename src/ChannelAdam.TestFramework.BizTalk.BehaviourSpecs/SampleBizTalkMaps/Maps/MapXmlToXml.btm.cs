namespace SampleBizTalkMaps.Maps {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"SampleBizTalkMaps.Schemas.XmlRequest", typeof(global::SampleBizTalkMaps.Schemas.XmlRequest))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"SampleBizTalkMaps.Schemas.XmlResponse", typeof(global::SampleBizTalkMaps.Schemas.XmlResponse))]
    public sealed class MapXmlToXml : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" xmlns:msxsl=""urn:schemas-microsoft-com:xslt"" xmlns:var=""http://schemas.microsoft.com/BizTalk/2003/var"" exclude-result-prefixes=""msxsl var s0 ScriptNS0 userCSharp"" version=""1.0"" xmlns:s0=""http://SampleBizTalkMaps.Schemas.XmlRequest"" xmlns:ns0=""http://SampleBizTalkMaps.Schemas.XmlResponse"" xmlns:ScriptNS0=""http://schemas.microsoft.com/BizTalk/2003/ScriptNS0"" xmlns:userCSharp=""http://schemas.microsoft.com/BizTalk/2003/userCSharp"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />
  <xsl:template match=""/"">
    <xsl:apply-templates select=""/s0:Request"" />
  </xsl:template>
  <xsl:template match=""/s0:Request"">
    <ns0:Response>
      <Key>
        <xsl:value-of select=""Key/text()"" />
      </Key>
      <xsl:variable name=""var:v1"" select=""ScriptNS0:NewGuid()"" />
      <xsl:variable name=""var:v2"" select=""userCSharp:StringConcat(string(StringValue/text()) , string($var:v1))"" />
      <StringValue>
        <xsl:value-of select=""$var:v2"" />
      </StringValue>
      <DateTimeValue>
        <xsl:value-of select=""DateTimeValue/text()"" />
      </DateTimeValue>
      <IntegerValue>
        <xsl:value-of select=""IntegerValue/text()"" />
      </IntegerValue>
      <DecimalValue>
        <xsl:value-of select=""DecimalValue/text()"" />
      </DecimalValue>
    </ns0:Response>
  </xsl:template>
  <msxsl:script language=""C#"" implements-prefix=""userCSharp""><![CDATA[
public string StringConcat(string param0, string param1)
{
   return param0 + param1;
}



]]></msxsl:script>
</xsl:stylesheet>";
        
        private const string _strArgList = @"<ExtensionObjects>
  <ExtensionObject Namespace=""http://schemas.microsoft.com/BizTalk/2003/ScriptNS0"" AssemblyName=""SampleBizTalkMapHelpers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"" ClassName=""SampleBizTalkMapHelpers.GuidHelper"" />
</ExtensionObjects>";
        
        private const string _strSrcSchemasList0 = @"SampleBizTalkMaps.Schemas.XmlRequest";
        
        private const global::SampleBizTalkMaps.Schemas.XmlRequest _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"SampleBizTalkMaps.Schemas.XmlResponse";
        
        private const global::SampleBizTalkMaps.Schemas.XmlResponse _trgSchemaTypeReference0 = null;
        
        public override string XmlContent {
            get {
                return _strMap;
            }
        }
        
        public override string XsltArgumentListContent {
            get {
                return _strArgList;
            }
        }
        
        public override string[] SourceSchemas {
            get {
                string[] _SrcSchemas = new string [1];
                _SrcSchemas[0] = @"SampleBizTalkMaps.Schemas.XmlRequest";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"SampleBizTalkMaps.Schemas.XmlResponse";
                return _TrgSchemas;
            }
        }
    }
}
