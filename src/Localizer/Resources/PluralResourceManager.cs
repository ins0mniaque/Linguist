using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Localizer.Resources
{
    public class PluralResourceManager
    {
        public delegate IEnumerable GetResources ( CultureInfo culture );

        private readonly GetResources                             getResources;
        private readonly IResourceNamingStrategy                  namingStrategy;
        private readonly Dictionary < string, PluralResourceSet > resourceSets;

        public PluralResourceManager ( GetResources getResourceSet, IResourceNamingStrategy namingStrategy = null )
        {
            this.getResources   = getResourceSet;
            this.namingStrategy = namingStrategy ?? ResourceNamingStrategy.Default;
            this.resourceSets   = new Dictionary < string, PluralResourceSet > ( );
        }

        private KeyValuePair < string, PluralResourceSet > lastUsedResourceSet;

        public PluralResourceSet GetResourceSet ( PluralRules pluralRules )
        {
            var language = pluralRules.Culture.Name;
            var lastUsed = lastUsedResourceSet;

            if ( lastUsed.Key == language )
                return lastUsed.Value;

            var resourceSet = GetCachedResourceSet ( pluralRules );
            if ( resourceSet != null )
                lastUsedResourceSet = new KeyValuePair < string, PluralResourceSet > ( language, resourceSet );

            return resourceSet;
        }

        private PluralResourceSet GetCachedResourceSet ( PluralRules pluralRules )
        {
            var language = pluralRules.Culture.Name;

            lock ( resourceSets )
                if ( resourceSets.TryGetValue ( language, out var resourceSet ) )
                    return resourceSet;

            var loadedResourceSet = LoadResourceSet ( pluralRules );

            lock ( resourceSets )
            {
                if ( resourceSets.TryGetValue ( language, out var resourceSet ) )
                    return resourceSet;

                resourceSets.Add ( language, loadedResourceSet );
            }

            return loadedResourceSet;
        }

        private PluralResourceSet LoadResourceSet ( PluralRules pluralRules )
        {
            var resources       = getResources ( pluralRules.Culture );
            var pluralResources = new Dictionary < string, List < FormatString > > ( );

            foreach ( var resource in resources )
            {
                var name = ReadResourceName ( resource );

                var baseName = namingStrategy.ParseArguments ( pluralRules, name, out var arguments );
                if ( baseName != name )
                {
                    if ( ! pluralResources.TryGetValue ( baseName, out var formatStrings ) )
                        pluralResources.Add ( baseName, formatStrings = new List < FormatString > ( ) );

                    var value = FormatString.Parse ( ReadResourceValue ( resource ) );
                    value.Arguments = arguments;
                    formatStrings.Add ( value );
                }
            }

            var resourceSet = new PluralResourceSet ( pluralRules, pluralResources.Count );

            foreach ( var resource in resources )
            {
                var name = ReadResourceName ( resource );
                if ( pluralResources.TryGetValue ( name, out var formatStrings ) )
                {
                    var formatString = FormatString.Parse ( ReadResourceValue ( resource ) );
                    SetDefaultArguments ( formatString, formatStrings );
                    resourceSet.AddPluralResource ( name, formatString, formatStrings );
                }
            }

            return resourceSet;
        }

        private static void SetDefaultArguments ( FormatString formatString, List < FormatString > pluralFormatStrings )
        {
            var length = formatString.Arguments.Length;

            for ( var index = 0; index < length; index++ )
            {
                var argument = formatString.Arguments [ index ];

                foreach ( var pluralFormatString in pluralFormatStrings )
                {
                    if ( index >= pluralFormatString.Arguments.Length )
                        continue;

                    var pluralArgument = pluralFormatString.Arguments [ index ];

                    argument.NumberForm           |= pluralArgument.NumberForm;
                    argument.AvailablePluralForms |= pluralArgument.AvailablePluralForms;
                    argument.PluralRangeForm      |= pluralArgument.PluralRangeForm;
                }
            }
        }

        private static string ReadResourceName ( object resource )
        {
            if ( resource is DictionaryEntry entry && entry.Key is string key )
                return key;

            if ( resource is KeyValuePair < string, object > pair )
                return pair.Key;

            throw new ArgumentException ( "Unknown resource entry type", nameof ( resource ) );
        }

        private static string ReadResourceValue ( object resource )
        {
            if ( resource is DictionaryEntry entry && entry.Value is string entryValue )
                return entryValue;

            if ( resource is KeyValuePair < string, object > pair && pair.Value is string pairValue )
                return pairValue;

            throw new ArgumentException ( "Unknown resource entry type", nameof ( resource ) );
        }
    }
}