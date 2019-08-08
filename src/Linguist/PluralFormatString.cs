namespace Linguist
{
    public class PluralFormatString
    {
        private FormatString     defaultFormat;
        private FormatString [ ] pluralFormats;

        public PluralFormatString ( FormatString defaultFormat, params FormatString [ ] pluralFormats )
        {
            this.defaultFormat = defaultFormat;
            this.pluralFormats = pluralFormats;

            SetDefaultArguments ( defaultFormat, pluralFormats );
        }

        private static void SetDefaultArguments ( FormatString defaultFormat, FormatString [ ] pluralFormats )
        {
            var length = defaultFormat.Arguments.Length;

            for ( var index = 0; index < length; index++ )
            {
                var argument = defaultFormat.Arguments [ index ];

                foreach ( var pluralFormat in pluralFormats )
                {
                    if ( index >= pluralFormat.Arguments.Length )
                        continue;

                    var pluralArgument = pluralFormat.Arguments [ index ];

                    argument.NumberForm           |= pluralArgument.NumberForm;
                    argument.AvailablePluralForms |= pluralArgument.AvailablePluralForms;
                    argument.PluralRangeForm      |= pluralArgument.PluralRangeForm;
                }
            }
        }

        public FormatString ApplyPluralRules ( PluralRules pluralRules, params object [ ] args )
        {
            var pluralForms = pluralRules.SelectPluralForms ( defaultFormat, args );

            ApplyPluralRangeRules ( pluralRules, defaultFormat.Arguments, pluralForms );

            for ( var index = 0; index < pluralFormats.Length; index++ )
            {
                var candidateFormat = pluralFormats [ index ];
                if ( Matches ( candidateFormat, pluralForms ) )
                    return candidateFormat;
            }

            return defaultFormat;
        }

        private static void ApplyPluralRangeRules ( PluralRules pluralRules, FormatString.Argument [ ] arguments, PluralForm [ ] pluralForms )
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

            return availablePluralForms.HasBitMask ( pluralForm );
        }
    }
}