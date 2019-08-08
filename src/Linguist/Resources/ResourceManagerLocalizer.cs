using System;
using System.Globalization;
using System.Resources;

namespace Linguist.Resources
{
    public class ResourceManagerLocalizer : ILocalizer
    {
        private readonly ResourceManager resourceManager;
        private readonly IPluralizer     pluralizer;

        public ResourceManagerLocalizer ( ResourceManager resourceManager, IPluralizer pluralizer = null )
        {
            this.resourceManager = resourceManager;
            this.pluralizer      = pluralizer ?? new ResourceManagerPluralizer ( resourceManager );
        }

        public object GetObject ( CultureInfo culture, string name ) => resourceManager.GetObject ( name, culture );
        public string GetString ( CultureInfo culture, string name ) => resourceManager.GetString ( name, culture );

        public string Format ( CultureInfo culture, IFormatProvider provider, string name, params object [ ] args )
        {
            var format = pluralizer.GetFormat ( culture, name, args ) ??
                         GetString ( culture, name );

            return format != null ? string.Format ( provider, format, args ) : null;
        }
    }
}