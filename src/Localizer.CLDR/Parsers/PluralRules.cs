using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Localizer.CLDR
{
    public class PluralRules
    {
        private PluralRules ( ) { }

        public string         Name       { get; private set; }
        public string     [ ] Locales    { get; private set; }
        public PluralForm     PluralForm { get; private set; }
        public string     [ ] Operands   { get; private set; }
        public PluralRule [ ] Rules      { get; private set; }

        public static PluralRules [ ] Parse ( XDocument document )
        {
            var ruleSets = new List < PluralRules > ( );

            var plurals = document.Descendants ( "plurals" ).Single ( );
            var type    = plurals .Attribute   ( "type"    ).Value?.ToString ( );
            var prefix  = type == "cardinal" ? "CardinalRuleSet" :
                          type == "ordinal"  ? "OrdinalRuleSet"  :
                          throw new XmlException ( "Invalid plurals type attribute" );
            var names   = new Dictionary < string, int > ( );

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

                var name      = (string) null;
                var isDefault = rules.Count == 0 && ruleSet.PluralForm == PluralForm.Other;
                if ( ! isDefault )
                {
                    name = prefix + ruleSet.PluralForm.ToString ( ).Replace ( ", ", string.Empty );
                    if ( ! names.TryGetValue ( name, out var index ) )
                        index = -1;

                    names [ name ] = ++index;

                    name += (char) ( 'A' + index );
                }
                else
                    name = "Default" + prefix;

                ruleSet.Name     = name;
                ruleSet.Operands = operands.ToArray ( );
                ruleSet.Rules    = rules   .ToArray ( );

                ruleSets.Add ( ruleSet );
            }

            return ruleSets.ToArray ( );
        }
    }
}