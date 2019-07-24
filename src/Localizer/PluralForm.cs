using System;

namespace Localizer
{
    /// <remarks>
    /// Explicit "0" and "1" rules:
    ///
    ///   • The explicit "0" and "1" cases are not defined by language-specific rules, and are available in any language
    ///     for the CLDR data items that accept them.
    ///   • The explicit "0" and "1" cases apply to the exact numeric values 0 and 1 respectively. These cases are
    ///     typically used for plurals of items that do not have fractional value, like books or files.
    ///   • The explicit "0" and "1" cases have precedence over the "zero" and "one" cases. For example, if for a
    ///     particular element CLDR data includes values for both the "1" and "one" cases, then the "1" value is used for
    ///     numeric values of exactly 1, while the "one" value is used for any other formatted numeric values matching
    ///     the "one" plural rule for the language.
    /// </remarks>
    [ Flags ]
    public enum PluralForm
    {
        /// <summary>
        /// Common name for the 'zero' plural form.
        /// </summary>
        Zero = 1 << 0,

        /// <summary>
        /// Common name for the 'singular' plural form.
        /// </summary>
        One = 1 << 1,

        /// <summary>
        /// Common name for the 'dual' plural form.
        /// </summary>
        Two = 1 << 2,

        /// <summary>
        /// Common name for the 'paucal' or other special plural form.
        /// </summary>
        Few = 1 << 3,

        /// <summary>
        /// Common name for the arabic (11 to 99) plural form.
        /// </summary>
        Many = 1 << 4,

        /// <summary>
        /// Common name for the default plural form.
        /// </summary>
        Other = 0,

        /// <summary>
        /// Common name for the explicit 'zero' plural form.
        /// </summary>
        ExplicitZero = 1 << 5,

        /// <summary>
        /// Common name for the explicit 'one' plural form.
        /// </summary>
        ExplicitOne = 1 << 6
    }
}