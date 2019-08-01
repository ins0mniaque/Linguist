using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace Linguist.WPF
{
    internal static class Designer
    {
        private static bool? isInDesignMode;
        public  static bool  IsInDesignMode => isInDesignMode ?? ( isInDesignMode = DetectDesignMode ( ) ).Value;

        private static bool DetectDesignMode ( )
        {
            return (bool) DesignerProperties.IsInDesignModeProperty
                                            .GetMetadata ( typeof ( DependencyObject ) )
                                            .DefaultValue;
        }

        public static string GetCurrentViewPath ( )
        {
            var designer = Assembly.Load        ( "Microsoft.VisualStudio.DesignTools.Designer" )
                                  ?.GetType     ( "Microsoft.VisualStudio.DesignTools.Designer.DesignerService" )
                                   .GetProperty ( "CurrentDesigner", BindingFlags.NonPublic | BindingFlags.Static )
                                  ?.GetValue    ( null, null );

            var context = designer?.GetType ( )
                                   .GetField ( "designerContext", BindingFlags.NonPublic | BindingFlags.Instance )
                                  ?.GetValue ( designer );

            var documentView = context?.GetType ( )
                                       .GetProperty ( "DocumentViewContext" )
                                      ?.GetValue ( context, null );

            var views = documentView?.GetType ( )
                                     .GetProperty ( "Views" )
                                    ?.GetValue ( documentView, null ) as IEnumerable;

            if ( views == null )
                return null;

            foreach ( var view in views )
            {
                var viewType   = view.GetType ( );
                var isUpdating = viewType.GetProperty ( "IsUpdating" )?.GetValue ( view, null );

                if ( isUpdating is bool updating && updating )
                {
                    // NOTE: view.ProjectContext.TargetAssemblyName contains the assembly name
                    return viewType.GetProperty ( "DocumentPath" )?.GetValue ( view, null )?.ToString ( );
                }
            }

            return null;
        }
    }
}