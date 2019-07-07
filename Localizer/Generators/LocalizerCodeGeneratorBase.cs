using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.Designer.Interfaces;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextTemplating.VSHost;

using Localizer.Generator;

namespace Localizer
{
    public abstract class LocalizerCodeGeneratorBase : BaseCodeGeneratorWithSite
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
            var code = LocalizerSupport.GenerateCode ( inputFileName, inputFileContent, AccessModifier );

            using ( var stream = new MemoryStream ( ) )
            {
                using ( var writer = new StreamWriter ( stream, Encoding.UTF8 ) )
                    CodeDomProvider.GenerateCodeFromCompileUnit ( code, writer, null );

                return stream.ToArray ( );
            }
        }

        private static readonly Guid CodeDomServiceGuid = new Guid ( "73E59688-C7C4-4A85-AF64-A538754784C5" );

        private CodeDomProvider codeDomProvider;
        private CodeDomProvider CodeDomProvider
        {
            get
            {
                if ( codeDomProvider == null )
                {
                    var vsmdCodeDomProvider = (IVSMDCodeDomProvider) GetService ( CodeDomServiceGuid ) ??
                                              throw new ServiceUnavailableException ( typeof ( IVSMDCodeDomProvider ) );

                    codeDomProvider = vsmdCodeDomProvider.CodeDomProvider as CodeDomProvider ??
                                      throw new ServiceUnavailableException ( typeof ( CodeDomProvider ) );
                }

                return codeDomProvider;
            }
        }
    }
}