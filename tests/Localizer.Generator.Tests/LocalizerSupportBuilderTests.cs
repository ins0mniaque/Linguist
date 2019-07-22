using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;

using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;

using Xunit;

using Localizer.CodeDom;

namespace Localizer.Generator.Tests
{
    using static MemberAttributes;

    public class LocalizerSupportBuilderTests
    {
        private readonly string PresentationCoreDll      = System.Reflection.Assembly.Load ( "PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"      ).Location;
        private readonly string PresentationFrameworkDll = System.Reflection.Assembly.Load ( "PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" ).Location;
        private readonly string WindowsBaseDll           = System.Reflection.Assembly.Load ( "WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"           ).Location;

        [ Theory ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public   | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public,            null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly,          null ) ]
        public void GeneratesCSharpCodeThatCompilesWithoutErrors ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        {
            var provider    = new Microsoft.CSharp.CSharpCodeProvider ( );
            var compiler    = new CSharpCodeProvider ( new XunitCompilerSettings ( Language.CSharp ) );
            var parameters  = GenerateCompilerParameters ( "System.dll",
                                                           "System.Drawing.dll",
                                                           "Localizer.dll" );

            var builder = GenerateBuilder ( provider,
                                            file,
                                            fileNamespace,
                                            resourceNamespace,
                                            accessModifiers,
                                            customToolType );

            var compilation = GenerateCodeThenCompile ( builder,
                                                        provider,
                                                        compiler,
                                                        parameters );

            Assert.Empty ( compilation.Errors );
        }

        [ Theory ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public   | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public,            null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly,          null ) ]
        public void GeneratesCSharpCodeForWPFThatCompilesWithoutErrors ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        {
            var provider    = new Microsoft.CSharp.CSharpCodeProvider ( );
            var compiler    = new CSharpCodeProvider ( new XunitCompilerSettings ( Language.CSharp ) );
            var parameters  = GenerateCompilerParameters ( "System.dll",
                                                           "System.Drawing.dll",
                                                           "System.Xaml.dll",
                                                           PresentationCoreDll,
                                                           PresentationFrameworkDll,
                                                           WindowsBaseDll,
                                                           "Localizer.dll",
                                                           "Localizer.WPF.dll" );

            var builder = GenerateBuilder ( provider,
                                            file,
                                            fileNamespace,
                                            resourceNamespace,
                                            accessModifiers,
                                            customToolType );

            builder.GenerateWPFSupport = true;

            var compilation = GenerateCodeThenCompile ( builder,
                                                        provider,
                                                        compiler,
                                                        parameters );

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
            var parameters  = GenerateCompilerParameters ( "System.dll",
                                                           "System.Drawing.dll",
                                                           "System.Web.dll",
                                                           "Localizer.dll" );

            var builder = GenerateBuilder ( provider,
                                            file,
                                            fileNamespace,
                                            resourceNamespace,
                                            accessModifiers,
                                            customToolType );

            var compilation = GenerateCodeThenCompile ( builder,
                                                        provider,
                                                        compiler,
                                                        parameters );

            Assert.Empty ( compilation.Errors );
        }

        [ Theory ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public   | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public,            null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly,          null ) ]
        public void GeneratesVBCodeForWPFThatCompilesWithoutErrors ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        {
            var provider    = new Microsoft.VisualBasic.VBCodeProvider ( );
            var compiler    = new VBCodeProvider ( new XunitCompilerSettings ( Language.VisualBasic ) );
            var parameters  = GenerateCompilerParameters ( "System.dll",
                                                           "System.Drawing.dll",
                                                           "System.Web.dll",
                                                           "System.Xaml.dll",
                                                           PresentationCoreDll,
                                                           PresentationFrameworkDll,
                                                           WindowsBaseDll,
                                                           "Localizer.dll",
                                                           "Localizer.WPF.dll" );

            var builder = GenerateBuilder ( provider,
                                            file,
                                            fileNamespace,
                                            resourceNamespace,
                                            accessModifiers,
                                            customToolType );

            builder.GenerateWPFSupport = true;

            var compilation = GenerateCodeThenCompile ( builder,
                                                        provider,
                                                        compiler,
                                                        parameters );

            Assert.Empty ( compilation.Errors );
        }

        private static LocalizerSupportBuilder GenerateBuilder ( CodeDomProvider provider, string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        {
            return LocalizerSupportBuilder.GenerateBuilder ( provider,
                                                             GetFullPath ( file ),
                                                             ReadFile    ( file ),
                                                             fileNamespace,
                                                             resourceNamespace,
                                                             accessModifiers,
                                                             customToolType );
        }

        private static CompilerResults GenerateCodeThenCompile ( LocalizerSupportBuilder builder, CodeDomProvider provider, CodeDomProvider compiler, CompilerParameters compilerParameters )
        {
            var code      = builder.Build ( );
            var source    = new StringBuilder ( );
            var generator = new ExtendedCodeGenerator ( provider );
            using ( var writer = new StringWriter ( source ) )
                generator.GenerateCodeFromCompileUnit ( code, writer, null );

            return compiler.CompileAssemblyFromSource ( compilerParameters, source.ToString ( ) );
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