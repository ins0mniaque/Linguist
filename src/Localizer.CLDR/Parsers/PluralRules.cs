using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Localizer.CLDR
{
    public class PluralRules
    {
        private PluralRules ( ) { }

        public string     [ ] Locales    { get; private set; }
        public PluralForm     PluralForm { get; private set; }
        public string     [ ] Operands   { get; private set; }
        public PluralRule [ ] Rules      { get; private set; }

        public static PluralRules [ ] Parse ( XDocument plurals )
        {
            var ruleSets = new List < PluralRules > ( );

            foreach ( var pluralRules in plurals.Descendants ( "pluralRules" ) )
            {
                var ruleSet = new PluralRules ( );

                ruleSet.Locales = pluralRules.Attribute ( "locales" ).Value
                                             .Replace ( "_", "-" )
                                             .Split   ( ' ' );

                var operands = new HashSet < string >  ( );
                var rules    = new List < PluralRule > ( );

                foreach ( var pluralRule in pluralRules.Descendants ( "pluralRule" ) )
                {
                    var rule = PluralRule.Parse ( pluralRule.Attribute ( "count" ).Value,
                                                  pluralRule.Value.ToString ( ),
                                                  out var ruleOperands );
                    if ( rule.Rule == null )
                        continue;

                    foreach ( var operand in ruleOperands )
                        operands.Add ( operand );

                    rules.Add ( rule );
                }

                foreach ( var rule in rules )
                    ruleSet.PluralForm |= rule.PluralForm;

                ruleSet.Operands = operands.ToArray ( );
                ruleSet.Rules    = rules   .ToArray ( );

                ruleSets.Add ( ruleSet );
            }

            return ruleSets.ToArray ( );
        }
    }
}