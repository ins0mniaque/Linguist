using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Localizer.Resources
{
    public class PluralResourceManager
    {
        public delegate IEnumerable GetResources ( CultureInfo culture );

        private readonly GetResources                        getResources;
        private readonly IResourceNamingStrategy             namingStrategy;
        private readonly Cache < string, PluralResourceSet > resourceSets;

        public PluralResourceManager ( GetResources getResourceSet, IResourceNamingStrategy namingStrategy = null )
        {
            this.getResources   = getResourceSet;
            this.namingStrategy = namingStrategy ?? ResourceNamingStrategy.Default;
            this.resourceSets   = new Cache < string, PluralResourceSet > ( );
        }

        public PluralResourceSet GetResourceSet ( CultureInfo culture )
        {
            if ( culture == null )
                culture = CultureInfo.CurrentUICulture;

            if ( resourceSets.TryGet ( culture.Name, out var resourceSet ) )
                return resourceSet;

            resourceSets.Add ( culture.Name, resourceSet = LoadResourceSet ( PluralRules.GetPluralRules ( culture ) ) );

            return resourceSet;
        }

        public PluralResourceSet GetResourceSet ( PluralRules pluralRules )
        {
            if ( resourceSets.TryGet ( pluralRules.Culture.Name, out var resourceSet ) )
                return resourceSet;

            resourceSets.Add ( pluralRules.Culture.Name, resourceSet = LoadResourceSet ( pluralRules ) );

            return resourceSet;
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