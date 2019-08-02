using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Linguist.WPF
{
    internal static class Auto
    {
        public static string GenerateKey ( DependencyObject target, DependencyProperty property )
        {
            var component = Localize.GetComponent ( target );
            if ( component == null )
                return null;

            var isComponent = target.ReadLocalValue ( Localize.ComponentProperty ) != DependencyProperty.UnsetValue;
            if ( isComponent )
                return GenerateKey ( component, property.Name );

            var name = Localize.GetName ( target );
            if ( string.IsNullOrEmpty ( name ) ) name = ( target as FrameworkElement )?.Name;
            if ( string.IsNullOrEmpty ( name ) ) name = target.GetType ( ).Name;

            return GenerateKey ( component, name, property.Name );
        }

        private static string GenerateKey ( params string [ ] parts )
        {
            return string.Join ( ".", Array.FindAll ( parts, part => ! string.IsNullOrEmpty ( part ) ) );
        }

        public static void SetComponent ( IServiceProvider serviceProvider )
        {
            if ( Designer.IsInDesignMode )
            {
                SetDesignModeComponent ( serviceProvider );
                return;
            }

            var root = RootObjectProvider.GetRootObject ( serviceProvider );

            if ( root is IComponentConnector )
            {
                var hasComponentSet = root.ReadLocalValue ( Localize.ComponentProperty ) != DependencyProperty.UnsetValue;
                if ( ! hasComponentSet )
                    root.SetValue ( Localize.ComponentProperty, root.GetType ( ).Name );
            }
        }

        private static void SetDesignModeComponent ( IServiceProvider serviceProvider )
        {
            var pvt  = (IProvideValueTarget) serviceProvider.GetService ( typeof ( IProvideValueTarget ) );
            var root = pvt.TargetObject as DependencyObject;

            while ( true )
            {
                var parent = VisualTreeHelper .GetParent ( root ) ??
                             LogicalTreeHelper.GetParent ( root );
                if ( parent == null )
                    break;

                root = parent;
            }

            var hasComponentSet = root.ReadLocalValue ( Localize.ComponentProperty ) != DependencyProperty.UnsetValue;
            if ( hasComponentSet )
                return;

            var component = ( root as IComponentConnector )?.GetType ( ).Name;

            if ( component == null )
            {
                var viewPath = Designer.GetCurrentViewPath ( );
                if ( string.IsNullOrEmpty ( viewPath ) )
                    return;

                component = Path.GetFileNameWithoutExtension ( viewPath );
            }

            root.SetValue ( Localize.ComponentProperty, component );
        }
    }
}