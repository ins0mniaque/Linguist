using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Linguist.CodeDom;
using Linguist.Generator;
using Linguist.Resources;

namespace Linguist.MSBuild
{
    public class Localization : Task
    {
        [ Required ] public ITaskItem [ ] Localizations          { get; set; }
        [ Required ] public ITaskItem     Language               { get; set; }
        [ Required ] public ITaskItem     RootNamespace          { get; set; }
        [ Required ] public ITaskItem     IntermediateOutputPath { get; set; }
                     public ITaskItem [ ] ReferencedAssemblies   { get; set; }
        [ Output   ] public ITaskItem [ ] Compile                { get; set; }
        [ Output   ] public ITaskItem [ ] EmbeddedResources      { get; set; }

        public override bool Execute ( )
        {
            if ( Language == null || string.IsNullOrEmpty ( Language.ItemSpec ) )
            {
                Log.LogError ( "Missing Language PropertyItem definition in project to specify which CodeDomProvider to use.  e.g. <Language>C#</Language>" );
                return false;
            }

            // System.Diagnostics.Debugger.Launch ( );

            var provider = CreateCodeDomProvider ( Language.ItemSpec );
            if ( provider == null )
                return false;

            var hasLinguist             = ReferencedAssemblies.Any ( reference => Path.GetFileName ( reference.ItemSpec ) == "Linguist.dll" );
            var hasLinguistWPF          = ReferencedAssemblies.Any ( reference => Path.GetFileName ( reference.ItemSpec ) == "Linguist.WPF.dll"  );
            var hasLinguistXamarinForms = ReferencedAssemblies.Any ( reference => Path.GetFileName ( reference.ItemSpec ) == "Linguist.Xamarin.Forms.dll" );

            var compile           = new List < ITaskItem > ( );
            var embeddedResources = new List < ITaskItem > ( );

            foreach ( var localization in Localizations )
            {
                if ( localization == null || string.IsNullOrEmpty ( localization.ItemSpec ) )
                    continue;

                var resourceSet = Generator.ResourceExtractor.ExtractResources ( localization.ItemSpec );
                var code        = GenerateCode ( provider, RootNamespace.ItemSpec, localization.ItemSpec, resourceSet, hasLinguist, hasLinguistWPF, hasLinguistXamarinForms, out var baseName, out var manifestPath );
                var resources   = Path.Combine ( IntermediateOutputPath.ItemSpec, manifestPath + "." + baseName + ".resources" );

                GenerateEmbeddedResource ( resources, resourceSet );

                var codeItem      = new TaskItem ( code      );
                var resourcesItem = new TaskItem ( resources );

                resourcesItem.SetMetadata ( "ManifestResourceName", resources );

                compile          .Add ( codeItem      );
                embeddedResources.Add ( resourcesItem );
            }

            Compile           = compile          .ToArray ( );
            EmbeddedResources = embeddedResources.ToArray ( );

            return true;
        }

        protected string GenerateCode ( CodeDomProvider codeDomProvider, string rootNamespace, string inputFileName, IResource [ ] resourceSet, bool hasLinguist, bool hasLinguistWPF, bool hasLinguistXamarinForms, out string baseName, out string manifestPath )
        {
            var fileBased = false;

            hasLinguist = hasLinguist || hasLinguistWPF || hasLinguistXamarinForms;

            var settings = new ResourceTypeSettings ( );
                baseName = Path.GetFileNameWithoutExtension ( inputFileName );
            var basePath = Path.GetDirectoryName ( inputFileName ).Replace ( '\\', '.' ).Replace ( '\\', '.' );

            manifestPath = rootNamespace;
            if ( ! string.IsNullOrEmpty ( basePath ) )
                manifestPath += "." + basePath;

            var relativePath   = Path.Combine ( rootNamespace, Path.Combine ( manifestPath.Substring ( rootNamespace.Length ).Split ( '.' ) ) );
            var accessModifier = System.CodeDom.MemberAttributes.Assembly | System.CodeDom.MemberAttributes.Static;

            settings.ClassName       = baseName;
            settings.Namespace       = manifestPath;
            settings.AccessModifiers = accessModifier;
            settings.CustomToolType  = GetType ( );
            settings.Extension       = hasLinguistWPF          ? ResourceTypeExtension.WPF          :
                                       hasLinguistXamarinForms ? ResourceTypeExtension.XamarinForms :
                                                                 ResourceTypeExtension.None;

            if      ( fileBased   ) settings.ConfigureFileBasedResourceManager ( baseName, Path.Combine ( relativePath, Path.GetFileName ( inputFileName ) ) );
            else if ( hasLinguist ) settings.ConfigureResourceManager          ( manifestPath + '.' + baseName );
            else                    settings.ConfigureWithoutLocalizer         ( manifestPath + '.' + baseName );

            var builder   = new ResourceTypeBuilder ( codeDomProvider, resourceSet, settings );
            var code      = builder.Build ( );
            var errors    = builder.GetErrors ( );
            var source    = new StringBuilder ( );
            var generator = new ExtendedCodeGenerator ( codeDomProvider );

            using ( var writer = new StringWriter ( source ) )
                generator.GenerateCodeFromCompileUnit ( code, writer, null );

            if ( errors != null )
            {
                foreach ( var error in errors )
                {
                    if ( error.IsWarning ) Log.LogWarning ( error.ToString ( ) );
                    else                   Log.LogError   ( error.ToString ( ) );
                }
            }

            var output = Path.ChangeExtension ( inputFileName, codeDomProvider.FileExtension );

            using ( var stream = File.Open ( output, FileMode.Create, FileAccess.Write ) )
            using ( var writer = new StreamWriter ( stream, Encoding.UTF8 ) )
                generator.GenerateCodeFromCompileUnit ( code, writer, null );

            return output;
        }

        protected void GenerateEmbeddedResource ( string path, IResource [ ] resourceSet )
        {
            using ( var stream = File.Open ( path, FileMode.Create, FileAccess.Write ) )
            using ( var writer = new System.Resources.ResourceWriter ( stream ) )
                foreach ( var res in resourceSet )
                    AddResource ( writer, res );
        }

        private static void AddResource ( System.Resources.ResourceWriter writer, IResource resource )
        {
            if ( resource is ILoadableResource loadable && loadable.Data is byte [ ] loadedData )
            {
                writer.AddResourceData ( resource.Name, resource.Type, loadedData );
                return;
            }

            var value = resource.Value;

            if      ( value is string   text ) writer.AddResource ( resource.Name, text  );
            else if ( value is byte [ ] data ) writer.AddResource ( resource.Name, data  );
            else                               writer.AddResource ( resource.Name, value );
        }

        private CodeDomProvider CreateCodeDomProvider ( string language )
        {
            foreach ( var compiler in CodeDomProvider.GetAllCompilerInfo ( ) )
                foreach ( var compilerLanguage in compiler.GetLanguages ( ) )
                    if ( string.Equals ( language, compilerLanguage, StringComparison.OrdinalIgnoreCase ) )
                        return CodeDomProvider.CreateProvider ( compilerLanguage );

            Log.LogError ( $"Unable to find a CodeDomProvider for language { language }." );

            return null;
        }
    }
}