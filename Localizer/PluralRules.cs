﻿using System;
using System.Globalization;

using Localizer.Plural;

namespace Localizer
{
    public sealed class PluralRules
    {
        public static PluralRules Invariant { get; } = new PluralRules ( CultureInfo.InvariantCulture,
                                                                         CardinalRuleSet.Default,
                                                                         OrdinalRuleSet.Default,
                                                                         PluralRangeFormSelector.Default );

        /// <summary>Retrieves a cached, read-only instance of a plural rule set fir the current UI culture.</summary>
        /// <returns>A read-only <see cref="T:Localizer.PluralRules" /> object.</returns>
        public static PluralRules GetPluralRules ( )
        {
            return GetPluralRules ( CultureInfo.CurrentUICulture );
        }

        /// <summary>Retrieves a cached, read-only instance of a plural rule set for the specified culture.</summary>
        /// <param name="culture">The specified <see cref="T:System.Globalization.CultureInfo" /> object.</param>
        /// <returns>A read-only <see cref="T:Localizer.PluralRules" /> object.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="culture" /> is null.</exception>
        public static PluralRules GetPluralRules ( CultureInfo culture )
        {
            culture = culture ?? throw new ArgumentNullException ( nameof ( culture ) );

            var unicodeLanguage = culture.Name;

            if ( unicodeLanguage.StartsWith ( "en" ) )
                return new PluralRules ( culture,
                                         new CardinalRuleSet ( new EnglishOneRule ( ) ),
                                         OrdinalRuleSet.Default,
                                         PluralRangeFormSelector.Default );

            if ( unicodeLanguage.StartsWith ( "fr" ) )
                return new PluralRules ( culture,
                                         new CardinalRuleSet ( new FrenchOneRule ( ) ),
                                         OrdinalRuleSet.Default,
                                         PluralRangeFormSelector.Default );

            return new PluralRules ( culture,
                                     CardinalRuleSet.Default,
                                     OrdinalRuleSet.Default,
                                     PluralRangeFormSelector.Default );
        }

        public static PluralRules CreateSpecificRules ( CultureInfo culture, PluralFormSelector cardinalFormSelector, PluralFormSelector ordinalFormSelector, PluralRangeFormSelector rangeFormSelector )
        {
            return new PluralRules ( culture, cardinalFormSelector, ordinalFormSelector, rangeFormSelector );
        }

        private PluralRules ( CultureInfo culture, PluralFormSelector cardinalForm, PluralFormSelector ordinalForm, PluralRangeFormSelector rangeForm )
        {
            Culture      = culture      ?? throw new ArgumentNullException ( nameof ( culture      ) );
            CardinalForm = cardinalForm ?? throw new ArgumentNullException ( nameof ( cardinalForm ) );
            OrdinalForm  = ordinalForm  ?? throw new ArgumentNullException ( nameof ( ordinalForm  ) );
            RangeForm    = rangeForm    ?? throw new ArgumentNullException ( nameof ( rangeForm    ) );
        }

        public CultureInfo             Culture      { get; }
        public PluralFormSelector      CardinalForm { get; }
        public PluralFormSelector      OrdinalForm  { get; }
        public PluralRangeFormSelector RangeForm    { get; }
    }
}