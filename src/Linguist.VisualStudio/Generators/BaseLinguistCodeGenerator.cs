using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using EnvDTE;

using Microsoft.VisualStudio.Designer.Interfaces;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextTemplating.VSHost;

using Linguist.CodeDom;
using Linguist.Generator;

namespace Linguist.VisualStudio
{
    public abstract class BaseLinguistCodeGenerator : BaseCodeGeneratorWithSite
    {
        protected abstract MemberAttributes AccessModifier { get; }

        public override string GetDefaultExtension ( )
        {
            var extension = CodeDomProvider.FileExtension;
            if ( extension?.Length > 0 && extension [ 0 ] != '.' )
                extension = "." + extension;

            return extension;
        }

        protected override byte [ ] GenerateCode ( string inputFileName, string inputFileContent )
        {
            ThreadHelper.ThrowIfNotOnUIThread ( );

            var fileBased               = ProjectItem.Properties.Item ( "ItemType" ).Value?.ToString ( ) != "EmbeddedResource";
            var hasLinguist             = Project.HasReference ( "Linguist" );
            var hasLinguistWPF          = Project.HasReference ( "Linguist.WPF" );
            var hasLinguistXamarinForms = Project.HasReference ( "Linguist.Xamarin.Forms" );

            hasLinguist = hasLinguist || hasLinguistWPF || hasLinguistXamarinForms;

            var resourceSet  = ResourceExtractor.ExtractResources ( inputFileName, inputFileContent );
            var settings     = new LinguistSupportBuilderSettings ( );
            var baseName     = Path.GetFileNameWithoutExtension ( inputFileName );
            var manifestPath = GetDefaultNamespace ( );
            var relativePath = Path.Combine ( Project.Name, Path.Combine ( manifestPath.Substring ( Project.Name.Length ).Split ( '.' ) ) );

            settings.ClassName                   = baseName;
            settings.Namespace                   = FileNamespace ?? manifestPath;
            settings.AccessModifiers             = AccessModifier;
            settings.CustomToolType              = GetType ( );
            settings.GenerateWPFSupport          = hasLinguistWPF;
            settings.GenerateXamarinFormsSupport = hasLinguistXamarinForms;

            if      ( fileBased   ) settings.ConfigureFileBasedResourceManager ( baseName, Path.Combine ( relativePath, Path.GetFileName ( inputFileName ) ) );
            else if ( hasLinguist ) settings.ConfigureResourceManager          ( manifestPath + '.' + baseName );
            else                    settings.ConfigureWithoutLocalizer         ( manifestPath + '.' + baseName );

            var builder   = new LinguistSupportBuilder ( CodeDomProvider, resourceSet, settings );
            var code      = builder.Build ( );
            var errors    = builder.GetErrors ( );
            var source    = new StringBuilder ( );
            var generator = new ExtendedCodeGenerator ( CodeDomProvider );

            using ( var writer = new StringWriter ( source ) )
                generator.GenerateCodeFromCompileUnit ( code, writer, null );

            if ( errors != null )
                foreach ( var error in errors )
                    GeneratorErrorCallback ( error.IsWarning, default, error.ErrorText, error.Line, error.Column );

            using ( var stream = new MemoryStream ( ) )
            {
                using ( var writer = new StreamWriter ( stream, Encoding.UTF8 ) )
                    generator.GenerateCodeFromCompileUnit ( code, writer, null );

                return stream.ToArray ( );
            }
        }

        private   CodeDomProvider codeDomProvider;
        protected CodeDomProvider CodeDomProvider
        {
            get
            {
                if ( codeDomProvider == null )
                {
                    var vsmdCodeDomProvider = (IVSMDCodeDomProvider) GetService ( typeof ( IVSMDCodeDomProvider ).GUID ) ??
                                              throw new ServiceUnavailableException ( typeof ( IVSMDCodeDomProvider ) );

                    codeDomProvider = vsmdCodeDomProvider.CodeDomProvider as CodeDomProvider ??
                                      throw new ServiceUnavailableException ( typeof ( CodeDomProvider ) );
                }

                return codeDomProvider;
            }
        }

        protected Project Project
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread ( );

                return ProjectItem.ContainingProject;
            }
        }

        protected ProjectItem ProjectItem
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread ( );

                return (ProjectItem) GetService ( typeof ( ProjectItem ) );
            }
        }

        protected string GetDefaultNamespace ( )
        {
            const int VSHPROPID_DefaultNamespace = -2049;

            ThreadHelper.ThrowIfNotOnUIThread ( );

            try
            {
                var vsBrowseObjectGuid = typeof ( IVsBrowseObject ).GUID;
                GetSite ( ref vsBrowseObjectGuid, out var siteInterfacePointer );
                if ( siteInterfacePointer == IntPtr.Zero )
                    return null;

                var vsBrowseObject = Marshal.GetObjectForIUnknown ( siteInterfacePointer ) as IVsBrowseObject;
                Marshal.Release ( siteInterfacePointer );
                if ( vsBrowseObject == null )
                    return null;

                vsBrowseObject.GetProjectItem ( out var vsHierarchy, out var itemId );
                if ( vsHierarchy == null )
                    return null;

                vsHierarchy.GetProperty ( itemId, VSHPROPID_DefaultNamespace, out var propertyValue );
                var propertyText = propertyValue as string;
                if ( propertyText == null )
                    return null;

                return propertyText;
            }
            catch ( NullReferenceException ) { throw; }
            catch ( StackOverflowException ) { throw; }
            catch ( OutOfMemoryException   ) { throw; }
            catch ( ThreadAbortException   ) { throw; }
            catch ( SEHException           ) { throw; }
            catch ( Exception              ) { return null; }
        }
    }
}