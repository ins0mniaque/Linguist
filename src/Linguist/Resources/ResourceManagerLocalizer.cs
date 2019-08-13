using System;
using System.Globalization;
using System.Resources;

namespace Linguist.Resources
{
    public class ResourceManagerLocalizer : ILocalizer
    {
        private readonly IPluralizer pluralizer;

        public ResourceManagerLocalizer ( ResourceManager resourceManager, IPluralizer pluralizer = null )
        {
            ResourceManager = resourceManager;
            this.pluralizer = pluralizer ?? new ResourceManagerPluralizer ( resourceManager );
        }

        public ResourceManager ResourceManager { get; }

        public object GetObject ( CultureInfo culture, string name ) => ResourceManager.GetObject ( name, culture );
        public string GetString ( CultureInfo culture, string name ) => ResourceManager.GetString ( name, culture );

        public string Format ( CultureInfo culture, IFormatProvider provider, string name, params object [ ] args )
        {
            var format = pluralizer     .GetFormat ( culture, name, args ) ??
                         ResourceManager.GetString ( name, culture );

            return format != null ? string.Format ( provider, format, args ) : null;
        }
    }
}