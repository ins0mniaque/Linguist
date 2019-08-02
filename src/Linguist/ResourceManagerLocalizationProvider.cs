using System;
using System.Collections;
using System.Globalization;
using System.Resources;

using Linguist.Resources;

namespace Linguist
{
    public class ResourceManagerLocalizationProvider : ILocalizationProvider
    {
        private readonly ResourceManager       resourceManager;
        private readonly PluralResourceManager pluralResourceManager;

        public ResourceManagerLocalizationProvider ( ResourceManager manager, IResourceNamingStrategy namingStrategy = null )
        {
            resourceManager       = manager;
            pluralResourceManager = new PluralResourceManager ( GetResourceSet, namingStrategy );
        }

        private IEnumerable GetResourceSet ( CultureInfo culture ) => resourceManager.GetResourceSet ( culture, true, true );

        public object GetObject ( CultureInfo culture, string name ) => resourceManager.GetObject ( name, culture );
        public string GetString ( CultureInfo culture, string name ) => resourceManager.GetString ( name, culture );

        public string Format ( CultureInfo culture, IFormatProvider provider, string name, params object [ ] args )
        {
            var pluralResourceSet = pluralResourceManager.GetResourceSet ( culture );
            var pluralResource    = pluralResourceSet.SelectPluralResource ( name, args );
            var format            = pluralResource?.Format ?? GetString ( culture, name );

            return format != null ? string.Format ( provider, format, args ) : null;
        }
    }
}