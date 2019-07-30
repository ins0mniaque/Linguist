namespace Linguist.Resources
{
    /// <summary>
    /// Defines a strategy to encode format string arguments inside a resource name.
    /// </summary>
    public interface IResourceNamingStrategy
    {
        /// <summary>
        /// Parses a resource name into a base resource name and format string arguments for a specific plural rules.
        /// </summary>
        /// <param name="pluralRules">The specific plural rules to parse the format string arguments.</param>
        /// <param name="resourceName">The resource name to parse.</param>
        /// <param name="arguments">The parsed format string arguments.</param>
        /// <returns>The base resource name.</returns>
        string ParseArguments ( PluralRules pluralRules, string resourceName, out FormatString.Argument [ ] arguments );
    }
}