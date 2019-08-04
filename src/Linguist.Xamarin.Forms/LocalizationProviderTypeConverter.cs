using System.Reflection;
using System.Resources;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Linguist.Xamarin.Forms
{
    [ TypeConversion ( typeof ( ILocalizationProvider ) ) ]
    public class LocalizationProviderTypeConverter : TypeConverter
    {
        private static Cache < string, ILocalizationProvider > cache = new Cache < string, ILocalizationProvider > ( );

        public override object ConvertFromInvariantString ( string namedProvider )
        {
            if ( ! cache.TryGet ( namedProvider, out var provider ) )
                cache.Add ( namedProvider, provider = LoadNamedProvider ( namedProvider ) );

            return provider;
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