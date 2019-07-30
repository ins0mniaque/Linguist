using System;
using System.Globalization;

using Linguist.Pluralization;

namespace Linguist
{
    public sealed class PluralRules
    {
        private static readonly Cache < CultureInfo, PluralRules > cache = new Cache < CultureInfo, PluralRules > ( );

        public static PluralRules Invariant { get; } = new PluralRules ( CultureInfo.InvariantCulture,
                                                                         CLDR.CardinalRuleSet.For ( CultureInfo.InvariantCulture ),
                                                                         CLDR.OrdinalRuleSet .For ( CultureInfo.InvariantCulture ),
                                                                         CLDR.RangeRuleSet   .For ( CultureInfo.InvariantCulture ) );

        /// <summary>Retrieves a cached, read-only instance of a plural rule set for the current UI culture.</summary>
        /// <returns>A read-only <see cref="T:Linguist.PluralRules" /> object.</returns>
        public static PluralRules GetPluralRules ( )
        {
            return GetPluralRules ( CultureInfo.CurrentUICulture );
        }

        /// <summary>Retrieves a cached, read-only instance of a plural rule set for the specified culture.</summary>
        /// <param name="culture">The specified <see cref="T:System.Globalization.CultureInfo" /> object.</param>
        /// <returns>A read-only <see cref="T:Linguist.PluralRules" /> object.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="culture" /> is null.</exception>
        public static PluralRules GetPluralRules ( CultureInfo culture )
        {
            if ( culture == null )
                throw new ArgumentNullException ( nameof ( culture ) );

            if ( culture == CultureInfo.InvariantCulture )
                return Invariant;

            if ( ! cache.TryGet ( culture, out var pluralRules ) )
                cache.Add ( culture, pluralRules = new PluralRules ( culture,
                                                                     CLDR.CardinalRuleSet.For ( culture ),
                                                                     CLDR.OrdinalRuleSet .For ( culture ),
                                                                     CLDR.RangeRuleSet   .For ( culture ) ) );

            return pluralRules;
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