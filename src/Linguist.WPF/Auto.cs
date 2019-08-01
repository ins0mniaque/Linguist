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

        internal static void SetComponent ( IServiceProvider serviceProvider )
        {
            if ( Designer.IsInDesignMode )
            {
                SetDesignModeComponent ( serviceProvider );
                return;
            }

            #if ! NET35
            var rootProvider = (System.Xaml.IRootObjectProvider) serviceProvider.GetService ( typeof ( System.Xaml.IRootObjectProvider ) );
            var root         = rootProvider?.RootObject as DependencyObject;

            if ( root is IComponentConnector )
            {
                var hasComponentSet = root.ReadLocalValue ( Localize.ComponentProperty ) != DependencyProperty.UnsetValue;
                if ( ! hasComponentSet )
                    root.SetValue ( Localize.ComponentProperty, root.GetType ( ).Name );
            }
            #endif
        }

        private static void SetDesignModeComponent ( IServiceProvider serviceProvider )
        {
            var viewPath = Designer.GetCurrentViewPath ( );
            if ( string.IsNullOrEmpty ( viewPath ) )
                return;

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
            if ( ! hasComponentSet )
                root.SetValue ( Localize.ComponentProperty, Path.GetFileNameWithoutExtension ( viewPath ) );
        }
    }
}