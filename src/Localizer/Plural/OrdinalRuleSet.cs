﻿using System;

namespace Localizer.Plural
{
    public class OrdinalRuleSet : PluralRuleSet
    {
        public static OrdinalRuleSet Default { get; } = new OrdinalRuleSet ( Array.Empty < PluralRule > ( ) );

        public OrdinalRuleSet ( params PluralRule [ ] ruleSet ) : base ( ruleSet ) { }
    }
}