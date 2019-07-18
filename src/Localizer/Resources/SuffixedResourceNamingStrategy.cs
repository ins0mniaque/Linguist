using System;
using System.Collections.Generic;

namespace Localizer.Resources
{
    public class SuffixedResourceNamingStrategy : ResourceNamingStrategy
    {
        public const string DefaultSeparator = ".";

        public SuffixedResourceNamingStrategy ( ) : this ( DefaultSeparator ) { }
        public SuffixedResourceNamingStrategy ( string separator,
                                                string rangeKeyword          = null,
                                                string defaultKeyword        = null,
                                                string ordinalDefaultKeyword = null,
                                                StringComparison comparisonType = StringComparison.OrdinalIgnoreCase )
            : base ( separator + "{0}",
                     separator + Keywords.Ordinal + "{0}",
                     separator + ( rangeKeyword ?? Keywords.Range ),
                     defaultKeyword,
                     ordinalDefaultKeyword,
                     comparisonType ) { }

        public override string ParseArguments ( PluralRules pluralRules, string resourceName, out FormatString.Argument [ ] arguments )
        {
            var stack = new Stack < FormatString.Argument > ( );

            while ( true )
            {
                if ( TryExtractPluralForm ( ref resourceName, out var pluralForm, out var numberForm ) )
                {
                    stack.Push ( new FormatString.Argument ( null, numberForm, pluralForm, null ) );
                    continue;
                }

                if ( stack.Count > 0 && TryExtractPluralRangeForm ( ref resourceName ) )
                {
                    var last = stack.Peek ( );

                    last.PluralRangeForm      = last.AvailablePluralForms;
                    last.AvailablePluralForms = pluralRules.CardinalForm.PluralForms;

                    stack.Push ( new FormatString.Argument ( null,
                                                             last.NumberForm,
                                                             last.AvailablePluralForms,
                                                             last.PluralRangeForm ) );

                    continue;
                }

                break;
            }

            arguments = stack.ToArray ( );

            return resourceName;
        }

        protected override bool Match ( string name, string form, StringComparison comparisonType )
        {
            return name.EndsWith ( form, comparisonType );
        }

        protected override string Trim ( string name, string match )
        {
            return name.Remove ( name.Length - match.Length, match.Length );
        }
    }
}