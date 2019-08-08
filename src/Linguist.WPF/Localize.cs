using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Linguist.WPF
{
    public static class Localize
    {
        [ TypeConverter ( typeof ( LocalizerTypeConverter ) ) ]
        public static ILocalizer GetLocalizer ( DependencyObject element )
        {
            return (ILocalizer) element.GetValue ( LocalizerProperty );
        }

        public static void SetLocalizer ( DependencyObject element, ILocalizer localizer )
        {
            element.SetValue ( LocalizerProperty, localizer );
        }

        public static readonly DependencyProperty LocalizerProperty =
            DependencyProperty.RegisterAttached ( "Localizer",
                                                  typeof ( ILocalizer ),
                                                  typeof ( Localize ),
                                                  new FrameworkPropertyMetadata ( null, FrameworkPropertyMetadataOptions.Inherits ) );

        public static string GetComponent ( DependencyObject element )
        {
            return (string) element.GetValue ( ComponentProperty );
        }

        public static void SetComponent ( DependencyObject element, string component )
        {
            element.SetValue ( ComponentProperty, component );
        }

        public static readonly DependencyProperty ComponentProperty =
            DependencyProperty.RegisterAttached ( "Component",
                                                  typeof ( string ),
                                                  typeof ( Localize ),
                                                  new FrameworkPropertyMetadata ( null, FrameworkPropertyMetadataOptions.Inherits ) );

        public static string GetName ( DependencyObject element )
        {
            return (string) element.GetValue ( NameProperty );
        }

        public static void SetName ( DependencyObject element, string name )
        {
            element.SetValue ( NameProperty, name );
        }

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.RegisterAttached ( "Name",
                                                  typeof ( string ),
                                                  typeof ( Localize ),
                                                  new FrameworkPropertyMetadata ( null ) );

        internal static object ProvideResource ( ILocalizer localizer, CultureInfo culture, object key, object [ ] values, Type targetType )
        {
            var start = 0;

            if ( localizer == null )
                localizer = values [ start++ ] as ILocalizer;

            if ( localizer == null )
                return Binding.DoNothing;

            // NOTE: Skipping language property, value is reflected through provided culture.
            //       Language is bound only to trigger updates on language changes.
            if ( values [ start ] is XmlLanguage )
                start++;

            var name = key == null                    ? values [ start++ ]?.ToString ( ) :
                       key is IProvideValueTarget pvt ? AutoGenerateKey ( pvt ) :
                                                        key?.ToString ( );

            if ( string.IsNullOrEmpty ( name ) )
                return Binding.DoNothing;

            if ( values.Length > start )
            {
                var arguments = new object [ values.Length - start ];
                Array.Copy ( values, start, arguments, 0, arguments.Length );
                return localizer.Format ( culture, culture, name, arguments ) ?? Fallback.String ( name );
            }

            if ( targetType.IsAssignableFrom ( typeof ( string ) ) )
                return localizer.GetString ( culture, name ) ?? Fallback.String ( name );

            var resource = localizer.GetObject ( culture, name );
            if ( resource == null )
                return Fallback.Object ( name, targetType );

            return ConvertTo ( resource, targetType );
        }

        private static string AutoGenerateKey ( IProvideValueTarget pvt )
        {
            var target    = pvt.TargetObject   as DependencyObject;
            var property  = pvt.TargetProperty as DependencyProperty;

            return Auto.GenerateKey ( target, property );
        }

        private static object ConvertTo ( object resource, Type targetType )
        {
            if ( resource == null )
                return null;

            var resourceType = resource.GetType ( );
            if ( targetType.IsAssignableFrom ( resourceType ) )
                return resource;

            var targetConverter = TypeDescriptor.GetConverter ( targetType );
            if ( targetConverter.CanConvertFrom ( resourceType ) )
                return targetConverter.ConvertFrom ( resource );

            var resourceConverter = TypeDescriptor.GetConverter ( resourceType );
            if ( resourceConverter.CanConvertTo ( targetType ) )
                return resourceConverter.ConvertTo ( resource, targetType );

            return resource;
        }
    }
}