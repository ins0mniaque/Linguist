using System;
using System.Globalization;

using Xamarin.Forms;

namespace Linguist.Xamarin.Forms
{
    using TypeDescriptor = System.ComponentModel.TypeDescriptor;

    public partial class Localize
    {
        [ TypeConverter ( typeof ( LocalizerTypeConverter ) ) ]
        public static ILocalizer GetLocalizer ( BindableObject element )
        {
            return (ILocalizer) element.GetValue ( LocalizerProperty );
        }

        public static void SetLocalizer ( BindableObject element, ILocalizer localizer )
        {
            element.SetValue ( LocalizerProperty, localizer );
        }

        public static readonly BindableProperty LocalizerProperty =
            BindableProperty.CreateAttached ( "Localizer",
                                              typeof ( ILocalizer ),
                                              typeof ( Localize ),
                                              null );

        public static string GetComponent ( BindableObject element )
        {
            return (string) element.GetValue ( ComponentProperty );
        }

        public static void SetComponent ( BindableObject element, string component )
        {
            element.SetValue ( ComponentProperty, component );
        }

        public static readonly BindableProperty ComponentProperty =
            BindableProperty.CreateAttached ( "Component",
                                              typeof ( string ),
                                              typeof ( Localize ),
                                              null );

        public static string GetName ( BindableObject element )
        {
            return (string) element.GetValue ( NameProperty );
        }

        public static void SetName ( BindableObject element, string name )
        {
            element.SetValue ( NameProperty, name );
        }

        public static readonly BindableProperty NameProperty =
            BindableProperty.CreateAttached ( "Name",
                                              typeof ( string ),
                                              typeof ( Localize ),
                                              null );

        internal static object ProvideResource ( ILocalizer localizer, CultureInfo culture, string key, object [ ] values, Type targetType )
        {
            var start = 0;

            if ( values.Length > start && localizer == null )
                localizer = values [ start++ ] as ILocalizer;

            if ( localizer == null )
                return null;

            if ( values.Length > start && values [ start ] is CultureInfo cultureOverride )
            {
                culture = cultureOverride;
                start++;
            }

            var name = key ?? values [ start++ ]?.ToString ( );

            if ( string.IsNullOrEmpty ( name ) )
                return null;

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