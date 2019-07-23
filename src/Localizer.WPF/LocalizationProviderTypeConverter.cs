using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Localizer.WPF
{
    public class LocalizationProviderTypeConverter : TypeConverter
    {
        private static Cache < string, ILocalizationProvider > cache = new Cache < string, ILocalizationProvider > ( );

        public override bool CanConvertFrom ( ITypeDescriptorContext context, Type sourceType )
        {
            return sourceType == typeof ( string );
        }

        public override object ConvertFrom ( ITypeDescriptorContext context, CultureInfo culture, object value )
        {
            if ( value is string namedProvider )
            {
                if ( ! cache.TryGet ( namedProvider, out var provider ) )
                    cache.Add ( namedProvider, provider = LoadNamedProvider ( namedProvider ) );

                return provider;
            }

            return base.ConvertFrom ( context, culture, value );
        }

        private static ILocalizationProvider LoadNamedProvider ( string name )
        {
            var assembly = Assembly.Load ( name.Split ( '/', '\\' ) [ 0 ] );

            name = name.Replace ( "/",  "." )
                       .Replace ( "\\", "." );

            return new ResourceManagerLocalizationProvider ( new ResourceManager ( name, assembly ) );
        }
    }
}