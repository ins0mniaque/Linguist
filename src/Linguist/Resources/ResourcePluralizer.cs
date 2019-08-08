using System.Globalization;

namespace Linguist.Resources
{
    public abstract class ResourcePluralizer : IPluralizer
    {
        private readonly Cache < string, PluralResourceSet > cache = new Cache < string, PluralResourceSet > ( );

        public PluralResourceSet GetResourceSet ( CultureInfo culture )
        {
            if ( culture == null )
                culture = CultureInfo.CurrentUICulture;

            if ( cache.TryGet ( culture.Name, out var resourceSet ) )
                return resourceSet;

            cache.Add ( culture.Name, resourceSet = LoadResourceSet ( PluralRules.GetPluralRules ( culture ) ) );

            return resourceSet;
        }

        public string GetFormat ( CultureInfo culture, string name, params object [ ] args )
        {
            return GetResourceSet ( culture )?.GetFormat ( name, args )?.Format;
        }

        protected abstract PluralResourceSet LoadResourceSet ( PluralRules pluralRules );
    }
}