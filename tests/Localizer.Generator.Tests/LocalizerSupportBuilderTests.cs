using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;

using Xunit;

using Localizer.CodeDom;

namespace Localizer.Generator.Tests
{
    using static MemberAttributes;

    public class LocalizerSupportBuilderTests
    {
        [ Theory ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public   | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public,            null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly,          null ) ]
        public void GeneratesCSharpCodeThatCompilesWithoutErrors ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        {
            var provider    = new Microsoft.CSharp.CSharpCodeProvider ( );
            var compiler    = new CSharpCodeProvider ( new XunitCompilerSettings ( Language.CSharp ) );
            var parameters  = GenerateCompilerParameters ( "System.dll", "System.Drawing.dll" );
            var compilation = GenerateCodeThenCompile    ( provider,
                                                           compiler,
                                                           parameters,
                                                           file,
                                                           fileNamespace,
                                                           resourceNamespace,
                                                           accessModifiers,
                                                           customToolType );

            Assert.Empty ( compilation.Errors );
        }

        [ Theory ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public   | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public,            null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly,          null ) ]
        public void GeneratesVBCodeThatCompilesWithoutErrors ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        {
            var provider    = new Microsoft.VisualBasic.VBCodeProvider ( );
            var compiler    = new VBCodeProvider ( new XunitCompilerSettings ( Language.VisualBasic ) );
            var parameters  = GenerateCompilerParameters ( "System.dll", "System.Drawing.dll", "System.Web.dll" );
            var compilation = GenerateCodeThenCompile    ( provider,
                                                           compiler,
                                                           parameters,
                                                           file,
                                                           fileNamespace,
                                                           resourceNamespace,
                                                           accessModifiers,
                                                           customToolType );

            Assert.Empty ( compilation.Errors );
        }

        private static CompilerResults GenerateCodeThenCompile ( CodeDomProvider provider, CodeDomProvider compiler, CompilerParameters compilerParameters, string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        {
            var code = LocalizerSupportBuilder.GenerateSource ( provider,
                                                                GetFullPath ( file ),
                                                                ReadFile    ( file ),
                                                                fileNamespace,
                                                                resourceNamespace,
                                                                accessModifiers,
                                                                customToolType,
                                                                out var _ );

            return compiler.CompileAssemblyFromSource ( compilerParameters, code );
        }

        private static CompilerParameters GenerateCompilerParameters ( params string [ ] referencedAssemblies )
        {
            var parameters = new CompilerParameters ( ) { GenerateInMemory = true };

            parameters.ReferencedAssemblies.AddRange ( referencedAssemblies );

            return parameters;
        }

        private static string GetFullPath ( string filename ) => Path.GetFullPath ( FindFile ( filename ) );
        private static string ReadFile    ( string filename ) => File.ReadAllText ( FindFile ( filename ) );
        private static string FindFile    ( string filename ) => Path.Combine ( "..\\..\\", filename );
    }
}