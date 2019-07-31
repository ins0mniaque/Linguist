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

            var builder = LinguistSupportBuilder.GenerateBuilder ( CodeDomProvider,
                                                                   inputFileName,
                                                                   inputFileContent,
                                                                   FileNamespace,
                                                                   GetResourcesNamespace ( ),
                                                                   AccessModifier,
                                                                   GetType ( ) );

            builder.GenerateWPFSupport = Project.HasReference ( "Linguist.WPF" );

            var code      = builder.Build ( );
            var source    = new StringBuilder ( );
            var generator = new ExtendedCodeGenerator ( CodeDomProvider );

            using ( var writer = new StringWriter ( source ) )
                generator.GenerateCodeFromCompileUnit ( code, writer, null );

            foreach ( var error in builder.GetErrors ( ) )
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

        protected string GetResourcesNamespace ( )
        {
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

                vsHierarchy.GetProperty ( itemId, -2049, out var propertyValue );
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