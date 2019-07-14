using System;

namespace Localizer
{
    public static class Pluralizer
    {
        public static PluralForm [ ] SelectPluralForms ( this PluralRules pluralRules, string format, PluralForm [ ] argumentsAvailablePluralForms, object [ ] arguments )
        {
            return pluralRules.SelectPluralForms ( null, FormatString.Parse ( format ), argumentsAvailablePluralForms, null, arguments );
        }

        public static PluralForm [ ] SelectPluralForms ( this PluralRules pluralRules, string format, PluralForm [ ] argumentsAvailablePluralForms, NumberForm [ ] argumentsNumberForm, object [ ] arguments )
        {
            return pluralRules.SelectPluralForms ( null, FormatString.Parse ( format ), argumentsAvailablePluralForms, argumentsNumberForm, arguments );
        }

        public static PluralForm [ ] SelectPluralForms ( this PluralRules pluralRules, FormatString formatString, PluralForm [ ] argumentsAvailablePluralForms, object [ ] arguments )
        {
            return pluralRules.SelectPluralForms ( null, formatString, argumentsAvailablePluralForms, null, arguments );
        }

        public static PluralForm [ ] SelectPluralForms ( this PluralRules pluralRules, FormatString formatString, PluralForm [ ] argumentsAvailablePluralForms, NumberForm [ ] argumentsNumberForm, object [ ] arguments )
        {
            return pluralRules.SelectPluralForms ( null, formatString, argumentsAvailablePluralForms, argumentsNumberForm, arguments );
        }

        public static PluralForm [ ] SelectPluralForms ( this PluralRules pluralRules, IFormatProvider provider, string format, PluralForm [ ] argumentsAvailablePluralForms, object [ ] arguments )
        {
            return pluralRules.SelectPluralForms ( provider, FormatString.Parse ( format ), argumentsAvailablePluralForms, null, arguments );
        }

        public static PluralForm [ ] SelectPluralForms ( this PluralRules pluralRules, IFormatProvider provider, string format, PluralForm [ ] argumentsAvailablePluralForms, NumberForm [ ] argumentsNumberForm, object [ ] arguments )
        {
            return pluralRules.SelectPluralForms ( provider, FormatString.Parse ( format ), argumentsAvailablePluralForms, argumentsNumberForm, arguments );
        }

        public static PluralForm [ ] SelectPluralForms ( this PluralRules pluralRules, IFormatProvider provider, FormatString formatString, PluralForm [ ] argumentsAvailablePluralForms, object [ ] arguments )
        {
            return pluralRules.SelectPluralForms ( provider, formatString, argumentsAvailablePluralForms, null, arguments );
        }

        public static PluralForm [ ] SelectPluralForms ( this PluralRules pluralRules, IFormatProvider provider, FormatString formatString, PluralForm [ ] argumentsAvailablePluralForms, NumberForm [ ] argumentsNumberForm, object [ ] arguments )
        {
            var selection = new PluralForm [ arguments.Length ];

            foreach ( var argumentHole in formatString.ArgumentHoles )
            {
                var index  = argumentHole.Index;
                var number = FormattedNumber.Parse ( provider ?? pluralRules.Culture, argumentHole, arguments [ index ] );
                if ( number != null )
                {
                    var numberForm = pluralRules.CardinalForm;
                    if ( argumentsNumberForm? [ index ] == NumberForm.Ordinal )
                        numberForm = pluralRules.OrdinalForm;

                    var available = argumentsAvailablePluralForms? [ index ] ?? numberForm.PluralForms;

                    selection [ index ] = numberForm.SelectPluralForm ( number.Value, available );
                }
            }

            return selection;
        }
    }
}