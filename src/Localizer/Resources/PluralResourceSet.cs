using System;
using System.Collections.Generic;
using System.Linq;

namespace Localizer.Resources
{
    public class PluralResourceSet
    {
        private readonly PluralRules                              pluralRules;
        private readonly IDictionary < string, FormatString [ ] > pluralResources;

        public PluralResourceSet ( PluralRules rules, int capacity )
        {
            pluralRules     = rules;
            pluralResources = new Dictionary < string, FormatString [ ] > ( capacity );
        }

        public void AddPluralResource ( string name, FormatString formatString, IEnumerable < FormatString > pluralFormatStrings )
        {
            pluralResources.Add ( name, pluralFormatStrings.Prepend ( formatString ).ToArray ( ) );
        }

        public FormatString SelectPluralResource ( string name, params object [ ] args )
        {
            var pluralResource      = pluralResources [ name ];
            var defaultFormatString = pluralResource  [ 0 ];

            var pluralForms = pluralRules.SelectPluralForms ( defaultFormatString, args );

            ApplyPluralRangeRules ( defaultFormatString.Arguments, pluralForms );

            for ( var index = 1; index < pluralResource.Length; index++ )
            {
                var candidateFormatString = pluralResource [ index ];
                if ( Matches ( candidateFormatString, pluralForms ) )
                    return candidateFormatString;
            }

            return defaultFormatString;
        }

        public string Format ( string name, params object [ ] args )
        {
            var formatString = SelectPluralResource ( name, args );

            return string.Format ( pluralRules.Culture, formatString.Format, args );
        }

        public string Format ( IFormatProvider provider, string name, params object [ ] args )
        {
            var formatString = SelectPluralResource ( name, args );

            return string.Format ( provider ?? pluralRules.Culture, formatString.Format, args );
        }

        private void ApplyPluralRangeRules ( FormatString.Argument [ ] arguments, PluralForm [ ] pluralForms )
        {
            for ( var index = 0; index < arguments.Length; index++ )
            {
                var argument = arguments [ index ];
                if ( argument.PluralRangeForm == null )
                    continue;

                var start     = pluralForms [ index ];
                var end       = pluralForms [ index + 1 ];
                var rangeForm = pluralRules.PluralFormRange.SelectPluralForm ( start, end );

                pluralForms [ index++ ] = PluralForm.Other;
                pluralForms [ index   ] = rangeForm;
            }
        }

        private static bool Matches ( FormatString formatString, PluralForm [ ] pluralForms )
        {
            var arguments = formatString.Arguments;

            for ( var index = 0; index < pluralForms.Length; index++ )
            {
                var availablePluralForms = PluralForm.Other;
                if ( index < arguments.Length )
                    availablePluralForms = arguments [ index ].AvailablePluralForms;

                if ( ! Matches ( availablePluralForms, pluralForms [ index ] ) )
                    return false;
            }

            return true;
        }

        private static bool Matches ( PluralForm availablePluralForms, PluralForm pluralForm )
        {
            if ( pluralForm == PluralForm.Other )
                return availablePluralForms == PluralForm.Other;

            return availablePluralForms.HasFlag ( pluralForm );
        }
    }
}