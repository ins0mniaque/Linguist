using System.Collections.Generic;
using System.Resources;

using Linguist.Resources.Naming;

namespace Linguist.Resources
{
    public class ResourceManagerPluralizer : ResourcePluralizer
    {
        private readonly ResourceManager         resourceManager;
        private readonly IResourceNamingStrategy namingStrategy;

        public ResourceManagerPluralizer ( ResourceManager resourceManager, IResourceNamingStrategy namingStrategy = null )
        {
            this.resourceManager = resourceManager;
            this.namingStrategy  = namingStrategy ?? ResourceNamingStrategy.Default;
        }

        protected override PluralResourceSet LoadResourceSet ( PluralRules pluralRules )
        {
            var resources       = resourceManager.GetResourceSet ( pluralRules.Culture, true, true );
            var pluralResources = new Dictionary < string, List < FormatString > > ( );

            var resource = resources.GetEnumerator ( );

            while ( resource.MoveNext ( ) )
            {
                if ( ! ( resource.Value is string ) )
                    continue;

                var name     = (string) resource.Key;
                var baseName = namingStrategy.ParseArguments ( pluralRules, name, out var arguments );

                if ( baseName != name )
                {
                    if ( ! pluralResources.TryGetValue ( baseName, out var pluralFormats ) )
                        pluralResources.Add ( baseName, pluralFormats = new List < FormatString > ( ) );

                    var pluralFormat = FormatString.Parse ( resource.Value?.ToString ( ) );
                    pluralFormat.Arguments = arguments;
                    pluralFormats.Add ( pluralFormat );
                }
            }

            var resourceSet = new PluralResourceSet ( pluralRules, pluralResources.Count );

            foreach ( var pluralResource in pluralResources )
            {
                var name  = pluralResource.Key;
                var value = resources.GetString ( name );
                if ( value == null )
                    continue;

                var defaultFormat = FormatString.Parse ( value );
                var pluralFormats = pluralResource.Value.ToArray ( );

                resourceSet.Add ( name, new PluralFormatString ( defaultFormat, pluralFormats ) );
            }

            return resourceSet;
        }
    }
}