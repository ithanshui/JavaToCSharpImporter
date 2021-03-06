﻿

namespace JavaCodeLoader_Unittest.TextHelpData
{
    public static  class JavaMapperObject
    {
        public static string JavaMapper = @";Excplicit Type Mapping (From what Java Type to Waht C# Type)
[Namespace]
java.io.Reader=System.IO
java.io.IOException=System.IO.Exception
java.nio.charset.StandardCharsets=System

;Fuzzy Search Type Mapping (Used to Map this Project to 'LuceNET')
[TypeStartsWith]
org.apache.lucene=LuceNET

;Currently Unused
[Type]
IndexOutOfBoundsException=System.IO.Exception.IndexOutOfBoundsException
String=string
boolean=bool

;Methode Mapping (to be Implemented)
[Methode]
String.substring=Substring
object.getName=GetType().Name

";
    }
}
