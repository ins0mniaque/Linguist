namespace Localizer
{
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
        Other = 0
    }
}