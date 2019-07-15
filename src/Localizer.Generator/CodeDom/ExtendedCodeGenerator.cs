using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

using Localizer.CodeDom.Extensions;

namespace Localizer.CodeDom
{
    /// <summary>
    /// Extends the CodeDomProvider code generator with support for C# static classes
    /// </summary>
    public class ExtendedCodeGenerator : ICodeGenerator
    {
        private readonly CodeDomProvider provider;

        public ExtendedCodeGenerator ( CodeDomProvider codeDomProvider )
        {
            provider = codeDomProvider;
            Language = codeDomProvider.GetLanguage ( );
        }

        public Language Language { get; }

        public string CreateEscapedIdentifier ( string value )
        {
            return provider.CreateEscapedIdentifier ( value );
        }

        public string CreateValidIdentifier ( string value )
        {
            return provider.CreateValidIdentifier ( value );
        }

        public void GenerateCodeFromCompileUnit ( CodeCompileUnit e, TextWriter w, CodeGeneratorOptions o )
        {
            if ( Language == Language.CSharp )
                provider.AddCSharpStaticClassSupport ( e, ref w );

            provider.GenerateCodeFromCompileUnit ( e, w, o );
        }

        public void GenerateCodeFromExpression ( CodeExpression e, TextWriter w, CodeGeneratorOptions o )
        {
            provider.GenerateCodeFromExpression ( e, w, o );
        }

        public void GenerateCodeFromNamespace ( CodeNamespace e, TextWriter w, CodeGeneratorOptions o )
        {
            provider.GenerateCodeFromNamespace ( e, w, o );
        }

        public void GenerateCodeFromStatement ( CodeStatement e, TextWriter w, CodeGeneratorOptions o )
        {
            provider.GenerateCodeFromStatement ( e, w, o );
        }

        public void GenerateCodeFromType ( CodeTypeDeclaration e, TextWriter w, CodeGeneratorOptions o )
        {
            if ( Language == Language.CSharp )
                provider.AddCSharpStaticClassSupport ( e, ref w );

            provider.GenerateCodeFromType ( e, w, o );
        }

        public string GetTypeOutput ( CodeTypeReference type )
        {
            return provider.GetTypeOutput ( type );
        }

        public bool IsValidIdentifier ( string value )
        {
            return provider.IsValidIdentifier ( value );
        }

        public bool Supports ( GeneratorSupport supports )
        {
            return provider.Supports ( supports );
        }

        public void ValidateIdentifier ( string value )
        {
            provider.ValidateIdentifier ( value );
        }
    }
}