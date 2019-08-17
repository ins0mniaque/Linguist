using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;

#if NET461
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
#endif

using Xunit;

using Linguist.CodeDom;

namespace Linguist.Generator.Tests
{
    using static MemberAttributes;

    public class ResourceTypeBuilderTests
    {
        #if NET461
        private readonly string PresentationCoreDll      = System.Reflection.Assembly.Load ( "PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"      ).Location;
        private readonly string PresentationFrameworkDll = System.Reflection.Assembly.Load ( "PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" ).Location;
        private readonly string WindowsBaseDll           = System.Reflection.Assembly.Load ( "WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"           ).Location;
        #endif

        [ Theory ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public   | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public,            null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly,          null ) ]
        #if NET461
        public void GeneratesCSharpCodeThatCompilesWithoutErrors ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #else
        public void GeneratesCSharpCode                          ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #endif
        {
            var provider = new Microsoft.CSharp.CSharpCodeProvider ( );
            var builder  = GenerateBuilder ( provider,
                                             file,
                                             fileNamespace,
                                             resourceNamespace,
                                             accessModifiers,
                                             customToolType );

            builder.Settings.ConfigureResourceManager ( resourceNamespace + '.' + builder.Settings.ClassName );

            #if NET461
            var compiler    = new CSharpCodeProvider ( new XunitCompilerSettings ( Language.CSharp ) );
            var parameters  = GenerateCompilerParameters ( "System.dll",
                                                           "System.Drawing.dll",
                                                           "Linguist.dll" );
            var compilation = GenerateCodeThenCompile ( builder,
                                                        provider,
                                                        compiler,
                                                        parameters );

            Assert.Empty ( compilation.Errors );
            #else
            Assert.NotEmpty ( GenerateCode ( builder, provider ) );
            #endif
        }

        [ Theory ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public   | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public,            null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly,          null ) ]
        #if NET461
        public void GeneratesCSharpCodeForWPFThatCompilesWithoutErrors ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #else
        public void GeneratesCSharpCodeForWPF                          ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #endif
        {
            var provider = new Microsoft.CSharp.CSharpCodeProvider ( );
            var builder  = GenerateBuilder ( provider,
                                             file,
                                             fileNamespace,
                                             resourceNamespace,
                                             accessModifiers,
                                             customToolType );

            builder.Settings.ConfigureResourceManager ( resourceNamespace + '.' + builder.Settings.ClassName );
            builder.Settings.GenerateWPFSupport = true;

            #if NET461
            var compiler    = new CSharpCodeProvider ( new XunitCompilerSettings ( Language.CSharp ) );
            var parameters  = GenerateCompilerParameters ( "System.dll",
                                                           "System.Drawing.dll",
                                                           "System.Xaml.dll",
                                                           PresentationCoreDll,
                                                           PresentationFrameworkDll,
                                                           WindowsBaseDll,
                                                           "Linguist.dll",
                                                           "Linguist.WPF.dll" );
            var compilation = GenerateCodeThenCompile ( builder,
                                                        provider,
                                                        compiler,
                                                        parameters );

            Assert.Empty ( compilation.Errors );
            #else
            Assert.NotEmpty ( GenerateCode ( builder, provider ) );
            #endif
        }

        [ Theory ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public   | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public,            null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly,          null ) ]
        #if NET461
        public void GeneratesCSharpCodeForXamarinFormsThatCompilesWithoutErrors ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #else
        public void GeneratesCSharpCodeForXamarinForms                          ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #endif
        {
            var provider = new Microsoft.CSharp.CSharpCodeProvider ( );
            var builder  = GenerateBuilder ( provider,
                                             file,
                                             fileNamespace,
                                             resourceNamespace,
                                             accessModifiers,
                                             customToolType );

            builder.Settings.ConfigureResourceManager ( resourceNamespace + '.' + builder.Settings.ClassName );
            builder.Settings.GenerateXamarinFormsSupport = true;

            #if NET461
            var compiler    = new CSharpCodeProvider ( new XunitCompilerSettings ( Language.CSharp ) );
            var parameters  = GenerateCompilerParameters ( "netstandard.dll",
                                                           "System.dll",
                                                           "System.Drawing.dll",
                                                           "Xamarin.Forms.Core.dll",
                                                           "Linguist.dll",
                                                           "Linguist.Xamarin.Forms.dll" );
            var compilation = GenerateCodeThenCompile ( builder,
                                                        provider,
                                                        compiler,
                                                        parameters );

            Assert.Empty ( compilation.Errors );
            #else
            Assert.NotEmpty ( GenerateCode ( builder, provider ) );
            #endif
        }

        [ Theory ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public   | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public,            null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly,          null ) ]
        #if NET461
        public void GeneratesCSharpCodeWithoutLocalizerThatCompilesWithoutErrors ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #else
        public void GeneratesCSharpCodeWithoutLocalizer                          ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #endif
        {
            var provider = new Microsoft.CSharp.CSharpCodeProvider ( );
            var builder  = GenerateBuilder ( provider,
                                             file,
                                             fileNamespace,
                                             resourceNamespace,
                                             accessModifiers,
                                             customToolType );

            builder.Settings.ConfigureWithoutLocalizer ( resourceNamespace + '.' + builder.Settings.ClassName );

            #if NET461
            var compiler    = new CSharpCodeProvider ( new XunitCompilerSettings ( Language.CSharp ) );
            var parameters  = GenerateCompilerParameters ( "System.dll",
                                                           "System.Drawing.dll" );
            var compilation = GenerateCodeThenCompile ( builder,
                                                        provider,
                                                        compiler,
                                                        parameters );

            Assert.Empty ( compilation.Errors );
            #else
            Assert.NotEmpty ( GenerateCode ( builder, provider ) );
            #endif
        }

        [ Theory ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public   | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public,            null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly,          null ) ]
        #if NET461
        public void GeneratesVBCodeThatCompilesWithoutErrors ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #else
        public void GeneratesVBCode                          ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #endif
        {
            var provider = new Microsoft.VisualBasic.VBCodeProvider ( );
            var builder  = GenerateBuilder ( provider,
                                             file,
                                             fileNamespace,
                                             resourceNamespace,
                                             accessModifiers,
                                             customToolType );

            builder.Settings.ConfigureResourceManager ( resourceNamespace + '.' + builder.Settings.ClassName );

            #if NET461
            var compiler    = new VBCodeProvider ( new XunitCompilerSettings ( Language.VisualBasic ) );
            var parameters  = GenerateCompilerParameters ( "System.dll",
                                                           "System.Drawing.dll",
                                                           "System.Web.dll",
                                                           "Linguist.dll" );
            var compilation = GenerateCodeThenCompile ( builder,
                                                        provider,
                                                        compiler,
                                                        parameters );

            Assert.Empty ( compilation.Errors );
            #else
            Assert.NotEmpty ( GenerateCode ( builder, provider ) );
            #endif
        }

        [ Theory ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public   | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public,            null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly,          null ) ]
        #if NET461
        public void GeneratesVBCodeForWPFThatCompilesWithoutErrors ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #else
        public void GeneratesVBCodeForWPF                          ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #endif
        {
            var provider = new Microsoft.VisualBasic.VBCodeProvider ( );
            var builder  = GenerateBuilder ( provider,
                                             file,
                                             fileNamespace,
                                             resourceNamespace,
                                             accessModifiers,
                                             customToolType );

            builder.Settings.ConfigureResourceManager ( resourceNamespace + '.' + builder.Settings.ClassName );
            builder.Settings.GenerateWPFSupport = true;

            #if NET461
            var compiler    = new VBCodeProvider ( new XunitCompilerSettings ( Language.VisualBasic ) );
            var parameters  = GenerateCompilerParameters ( "System.dll",
                                                           "System.Drawing.dll",
                                                           "System.Web.dll",
                                                           "System.Xaml.dll",
                                                           PresentationCoreDll,
                                                           PresentationFrameworkDll,
                                                           WindowsBaseDll,
                                                           "Linguist.dll",
                                                           "Linguist.WPF.dll" );
            var compilation = GenerateCodeThenCompile ( builder,
                                                        provider,
                                                        compiler,
                                                        parameters );

            Assert.Empty ( compilation.Errors );
            #else
            Assert.NotEmpty ( GenerateCode ( builder, provider ) );
            #endif
        }

        [ Theory ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public   | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public,            null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly,          null ) ]
        #if NET461
        public void GeneratesVBCodeForXamarinFormsThatCompilesWithoutErrors ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #else
        public void GeneratesVBCodeForXamarinForms                          ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #endif
        {
            var provider = new Microsoft.VisualBasic.VBCodeProvider ( );
            var builder  = GenerateBuilder ( provider,
                                             file,
                                             fileNamespace,
                                             resourceNamespace,
                                             accessModifiers,
                                             customToolType );

            builder.Settings.ConfigureResourceManager ( resourceNamespace + '.' + Path.GetFileNameWithoutExtension ( file ) );
            builder.Settings.GenerateXamarinFormsSupport = true;

            #if NET461
            var compiler    = new VBCodeProvider ( new XunitCompilerSettings ( Language.VisualBasic ) );
            var parameters  = GenerateCompilerParameters ( "netstandard.dll",
                                                           "System.dll",
                                                           "System.Drawing.dll",
                                                           "System.Web.dll",
                                                           "Xamarin.Forms.Core.dll",
                                                           "Linguist.dll",
                                                           "Linguist.Xamarin.Forms.dll" );
            var compilation = GenerateCodeThenCompile ( builder,
                                                        provider,
                                                        compiler,
                                                        parameters );

            Assert.Empty ( compilation.Errors );
            #else
            Assert.NotEmpty ( GenerateCode ( builder, provider ) );
            #endif
        }

        [ Theory ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public   | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly | Static, null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Public,            null ) ]
        [ InlineData ( "Data/Resources.resx", "Namespace", "ResourceNamespace", Assembly,          null ) ]
        #if NET461
        public void GeneratesVBCodeWithoutLocalizerThatCompilesWithoutErrors ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #else
        public void GeneratesVBCodeWithoutLocalizer                          ( string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        #endif
        {
            var provider = new Microsoft.VisualBasic.VBCodeProvider ( );
            var builder  = GenerateBuilder ( provider,
                                             file,
                                             fileNamespace,
                                             resourceNamespace,
                                             accessModifiers,
                                             customToolType );

            builder.Settings.ConfigureWithoutLocalizer ( resourceNamespace + '.' + builder.Settings.ClassName );

            #if NET461
            var compiler    = new VBCodeProvider ( new XunitCompilerSettings ( Language.VisualBasic ) );
            var parameters  = GenerateCompilerParameters ( "System.dll",
                                                           "System.Drawing.dll",
                                                           "System.Web.dll" );
            var compilation = GenerateCodeThenCompile ( builder,
                                                        provider,
                                                        compiler,
                                                        parameters );

            Assert.Empty ( compilation.Errors );
            #else
            Assert.NotEmpty ( GenerateCode ( builder, provider ) );
            #endif
        }

        private static ResourceTypeBuilder GenerateBuilder ( CodeDomProvider provider, string file, string fileNamespace, string resourceNamespace, MemberAttributes accessModifiers, Type customToolType )
        {
            var resourceSet  = ResourceExtractor.ExtractResources ( GetFullPath ( file ), ReadFile ( file ) );
            var settings     = new ResourceTypeSettings ( );
            var baseName     = Path.GetFileNameWithoutExtension ( file );

            settings.ClassName       = baseName;
            settings.Namespace       = fileNamespace ?? resourceNamespace;
            settings.AccessModifiers = accessModifiers;
            settings.CustomToolType  = customToolType;

            return new ResourceTypeBuilder ( provider, resourceSet, settings );
        }

        private static string GenerateCode ( ResourceTypeBuilder builder, CodeDomProvider provider )
        {
            var code      = builder.Build ( );
            var source    = new StringBuilder ( );
            var generator = new ExtendedCodeGenerator ( provider );
            using ( var writer = new StringWriter ( source ) )
                generator.GenerateCodeFromCompileUnit ( code, writer, null );

            return source.ToString ( );
        }

        #if NET461
        private static CompilerResults GenerateCodeThenCompile ( ResourceTypeBuilder builder, CodeDomProvider provider, CodeDomProvider compiler, CompilerParameters compilerParameters )
        {
            return compiler.CompileAssemblyFromSource ( compilerParameters, GenerateCode ( builder, provider ) );
        }

        private static CompilerParameters GenerateCompilerParameters ( params string [ ] referencedAssemblies )
        {
            var parameters = new CompilerParameters ( ) { GenerateInMemory = true };

            parameters.ReferencedAssemblies.AddRange ( referencedAssemblies );

            return parameters;
        }
        #endif

        private static string GetFullPath ( string filename ) => Path.GetFullPath ( FindFile ( filename ) );
        private static string ReadFile    ( string filename ) => File.ReadAllText ( FindFile ( filename ) );
        private static string FindFile    ( string filename ) => Path.Combine ( "..\\..\\..\\", filename );
    }
}