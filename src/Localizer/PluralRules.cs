using System;
using System.Globalization;

using Localizer.Pluralization;

namespace Localizer
{
    public sealed class PluralRules
    {
        public static PluralRules Invariant { get; } = new PluralRules ( CultureInfo.InvariantCulture,
                                                                         CardinalRuleSet.Default,
                                                                         OrdinalRuleSet.Default,
                                                                         PluralFormRangeSelector.Default );

        /// <summary>Retrieves a cached, read-only instance of a plural rule set for the current UI culture.</summary>
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
                                         PluralFormRangeSelector.Default );

            if ( unicodeLanguage.StartsWith ( "fr" ) )
                return new PluralRules ( culture,
                                         new CardinalRuleSet ( new FrenchOneRule ( ) ),
                                         OrdinalRuleSet.Default,
                                         PluralFormRangeSelector.Default );

            return new PluralRules ( culture,
                                     CardinalRuleSet.Default,
                                     OrdinalRuleSet.Default,
                                     PluralFormRangeSelector.Default );
        }

        public static PluralRules CreateSpecificRules ( CultureInfo culture, PluralFormSelector cardinalFormSelector, PluralFormSelector ordinalFormSelector, PluralFormRangeSelector rangeFormSelector )
        {
            return new PluralRules ( culture, cardinalFormSelector, ordinalFormSelector, rangeFormSelector );
        }

        private PluralRules ( CultureInfo culture, PluralFormSelector cardinalForm, PluralFormSelector ordinalForm, PluralFormRangeSelector pluralFormRange )
        {
            Culture         = culture         ?? throw new ArgumentNullException ( nameof ( culture         ) );
            CardinalForm    = cardinalForm    ?? throw new ArgumentNullException ( nameof ( cardinalForm    ) );
            OrdinalForm     = ordinalForm     ?? throw new ArgumentNullException ( nameof ( ordinalForm     ) );
            PluralFormRange = pluralFormRange ?? throw new ArgumentNullException ( nameof ( pluralFormRange ) );
        }

        public CultureInfo             Culture         { get; }
        public PluralFormSelector      CardinalForm    { get; }
        public PluralFormSelector      OrdinalForm     { get; }
        public PluralFormRangeSelector PluralFormRange { get; }
    }
}