using System.Collections;
using System.Globalization;
using System.Resources;

namespace Linguist.Resources.Dictionary
{
    public class DelegateResourceManager : ResourceManager
    {
        private readonly Cache < string, ResourceSet > cache = new Cache < string, ResourceSet > ( );

        public delegate IDictionary LoadResourceSet    ( string cultureName, bool createIfNotExists );
        public delegate IDictionary NeutralResourceSet ( bool createIfNotExists );

        private readonly LoadResourceSet    loadResourceSet;
        private readonly NeutralResourceSet neutralResourceSet;

        public DelegateResourceManager ( LoadResourceSet loadResourceSet, NeutralResourceSet neutralResourceSet = null )
        {
            this.loadResourceSet    = loadResourceSet;
            this.neutralResourceSet = neutralResourceSet;
        }

        protected override ResourceSet InternalGetResourceSet ( CultureInfo culture, bool createIfNotExists, bool tryParents )
        {
            if ( cache.TryGet ( culture.Name, out var resourceSet ) )
                return resourceSet;

            var fallback  = culture.Name;
            var resources = loadResourceSet ( fallback, createIfNotExists );

            if ( resources == null && tryParents )
            {
                fallback = culture.Parent.Name;
                if ( fallback != culture.Name )
                    resources = loadResourceSet ( fallback, createIfNotExists );
            }

            if ( resources == null && neutralResourceSet != null )
            {
                fallback  = CultureInfo.InvariantCulture.Name;
                resources = neutralResourceSet ( createIfNotExists );
            }

            if ( resources == null )
                return null;

            resourceSet = new ResourceSet ( new DictionaryResourceReader ( resources ) );

            cache.Add ( culture.Name, resourceSet );
            if ( fallback != culture.Name )
                cache.Add ( fallback, resourceSet );

            return resourceSet;
        }
    }
}