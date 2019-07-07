using System.CodeDom;

namespace Localizer.Generator
{
    public static class LocalizerSupport
    {
        public static CodeCompileUnit GenerateCode ( string inputFileName, string inputFileContent, MemberAttributes accessModifier )
        {
            var code = new CodeCompileUnit ( );

            code.ReferencedAssemblies.Add ( "System.dll" );

            code.Namespaces.Add ( new CodeNamespace { Imports = { new CodeNamespaceImport ( "System" ) } } );

            return code;
        }
    }
}