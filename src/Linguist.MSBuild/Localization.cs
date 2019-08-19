using System;
using System.CodeDom.Compiler;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Linguist.MSBuild
{
    public class Localization : Task
    {
        [ Required ] public ITaskItem [ ] Localizations     { get; set; }
        [ Required ] public ITaskItem     Language          { get; set; }
                     public ITaskItem [ ] References        { get; set; }
        [ Output   ] public ITaskItem [ ] Compile           { get; set; }
        [ Output   ] public ITaskItem [ ] EmbeddedResources { get; set; }

        public override bool Execute ( )
        {
            if ( Language == null || string.IsNullOrEmpty ( Language.ItemSpec ) )
            {
                Log.LogError ( "Missing Language PropertyItem definition in project to specify which CodeDomProvider to use.  e.g. <Language>C#</Language>" );
                return false;
            }

            // System.Diagnostics.Debugger.Launch ( );

            Log.LogMessage ( MessageImportance.High, "Hello World!" );
 
            var provider = CreateCodeDomProvider ( Language.ItemSpec );
            if ( provider == null )
                return false;

            return true;
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