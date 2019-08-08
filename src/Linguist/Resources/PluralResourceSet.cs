namespace Linguist.Resources
{
    public class PluralResourceSet
    {
        private readonly PluralRules                          pluralRules;
        private readonly Cache < string, PluralFormatString > resources;

        public PluralResourceSet ( PluralRules rules, int capacity )
        {
            pluralRules = rules;
            resources   = new Cache < string, PluralFormatString > ( capacity );
        }

        public void Add ( string name, PluralFormatString pluralFormatString )
        {
            resources.Add ( name, pluralFormatString );
        }

        public void Remove ( string name )
        {
            resources.Remove ( name );
        }

        public void Clear ( )
        {
            resources.Clear ( );
        }

        public FormatString GetFormat ( string name, params object [ ] args )
        {
            if ( ! resources.TryGet ( name, out var pluralFormatString ) )
                return null;

            return pluralFormatString.ApplyPluralRules ( pluralRules, args );
        }
    }
}