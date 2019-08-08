using System.Collections;
using System.Globalization;
using System.Resources;

namespace Linguist.Resources.Dictionary
{
    public class DelegateResourceManager : ResourceManager
    {
        public delegate IDictionary LoadResourceSet    ( string cultureName );
        public delegate IDictionary NeutralResourceSet ( );

        private readonly LoadResourceSet    loadResourceSet;
        private readonly NeutralResourceSet neutralResourceSet;

        public DelegateResourceManager ( LoadResourceSet loadResourceSet, NeutralResourceSet neutralResourceSet = null )
        {
            this.loadResourceSet    = loadResourceSet;
            this.neutralResourceSet = neutralResourceSet;
        }

        protected override ResourceSet InternalGetResourceSet ( CultureInfo culture, bool createIfNotExists, bool tryParents )
        {
            var resourceSet = loadResourceSet ( culture.Name );

            if ( resourceSet == null && tryParents )
            {
                var parent = culture.Parent;
                if ( parent.Name != culture.Name )
                    resourceSet = loadResourceSet ( parent.Name );
            }

            if ( resourceSet == null && createIfNotExists )
                resourceSet = neutralResourceSet ( );

            if ( resourceSet != null )
                return new ResourceSet ( new DictionaryResourceReader ( resourceSet ) );

            return null;
        }
    }
}