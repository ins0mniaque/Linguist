using System;

namespace Localizer
{
    public static class Pluralizer
    {
        public static PluralForm [ ] SelectPluralForms ( this PluralRules pluralRules, FormatString formatString, object [ ] arguments )
        {
            return pluralRules.SelectPluralForms ( null, formatString, arguments );
        }

        public static PluralForm [ ] SelectPluralForms ( this PluralRules pluralRules, IFormatProvider provider, FormatString formatString, object [ ] arguments )
        {
            var selection = new PluralForm [ arguments.Length ];

            for ( var index = 0; index < arguments.Length; index++ )
            {
                var argument = formatString.Arguments [ index ];
                if ( argument.NumberArgumentHole == null )
                    continue;

                var number = FormattedNumber.Parse ( provider ?? pluralRules.Culture, argument.NumberArgumentHole, arguments [ index ] );
                if ( number == null )
                    continue;

                var numberForm = pluralRules.CardinalForm;
                if ( argument.NumberForm == NumberForm.Ordinal )
                    numberForm = pluralRules.OrdinalForm;

                selection [ index ] = numberForm.SelectPluralForm ( number.Value, argument.AvailablePluralForms );
            }

            return selection;
        }
    }
}