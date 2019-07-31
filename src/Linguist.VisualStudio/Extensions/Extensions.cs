using System.Collections.Generic;

using EnvDTE;

using Microsoft.VisualStudio.Shell;

namespace Linguist.VisualStudio
{
    internal static class Extensions
    {
        // TODO: Add support for project.Object is VsWebSite.VSWebSite.
        public static IEnumerable < string > GetReferences ( this Project project )
        {
            ThreadHelper.ThrowIfNotOnUIThread ( );

            if ( project.Object is VSLangProj.VSProject vsProject )
                foreach ( VSLangProj.Reference reference in vsProject.References )
                    yield return reference.Name;
        }

        public static bool HasReference ( this Project project, string name )
        {
            foreach ( var reference in project.GetReferences ( ) )
                if ( reference == name )
                    return true;

            return false;
        }
    }
}