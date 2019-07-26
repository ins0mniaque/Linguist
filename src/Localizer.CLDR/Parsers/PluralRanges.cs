using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Localizer.CLDR
{
    public class PluralRanges
    {
        private PluralRanges ( ) { }

        public string          Name    { get; private set; }
        public string      [ ] Locales { get; private set; }
        public PluralRange [ ] Ranges  { get; private set; }

        public static PluralRanges [ ] Parse ( XDocument document )
        {
            var rangesSets  = new List < PluralRanges > ( );
            var prefix      = "RangeRuleSet";
            var names       = new Dictionary < string, int > ( );
            var expressions = new Dictionary < string, PluralRanges > ( );

            foreach ( var pluralRanges in document.Descendants ( "pluralRanges" ) )
            {
                var rangesSet = new PluralRanges ( );

                rangesSet.Locales = pluralRanges.Attribute ( "locales" ).Value
                                                .Replace ( "_", "-" )
                                                .Split   ( ' ' );

                var ranges = new List < PluralRange > ( );

                foreach ( var pluralRange in pluralRanges.Descendants ( "pluralRange" ) )
                {
                    var start  = PluralFormParser.Parse ( pluralRange.Attribute ( "start"  ).Value.ToString ( ) );
                    var end    = PluralFormParser.Parse ( pluralRange.Attribute ( "end"    ).Value.ToString ( ) );
                    var result = PluralFormParser.Parse ( pluralRange.Attribute ( "result" ).Value.ToString ( ) );

                    var isDefaultRule = end == result;
                    if ( ! isDefaultRule )
                        ranges.Add ( new PluralRange ( start, end, result ) );
                }

                var expression = Expression ( ranges );

                if ( expressions.TryGetValue ( expression, out var matchingRangesSet ) )
                {
                    matchingRangesSet.Locales = matchingRangesSet.Locales
                                                                 .Concat  ( rangesSet.Locales )
                                                                 .OrderBy ( _ => _ )
                                                                 .ToArray ( );
                    continue;
                }

                expressions.Add ( expression, rangesSet );

                var pluralForm = PluralForm.Other;
                foreach ( var range in ranges )
                    pluralForm |= range.Start | range.End | range.Result;

                var name = (string) null;

                var isDefaultRuleSet = ranges.Count == 0 && pluralForm == PluralForm.Other;
                if ( ! isDefaultRuleSet )
                {
                    name = prefix + pluralForm.ToString ( ).Replace ( ", ", string.Empty );
                    if ( ! names.TryGetValue ( name, out var index ) )
                        index = -1;

                    names [ name ] = ++index;

                    name += (char) ( 'A' + index );
                }
                else
                    name = "Default" + prefix;

                rangesSet.Name   = name;
                rangesSet.Ranges = ranges.ToArray ( );

                rangesSets.Add ( rangesSet );
            }

            return rangesSets.ToArray ( );
        }

        private static string Expression ( IEnumerable < PluralRange > pluralRanges )
        {
            var expression = new List < string > ( );
            foreach ( var pluralRange in pluralRanges )
                expression.Add ( $"{ pluralRange.Start }+{ pluralRange.End }={ pluralRange.Result }" );

            expression.Sort ( );

            return string.Join ( "\n", expression );
        }
    }
}