﻿using System.Collections;
using System.Globalization;
using System.Resources;

namespace Linguist.Resources.Dictionary
{
    public class DictionaryResourceManager : ResourceManager
    {
        private readonly Cache < string, IDictionary > cache = new Cache < string, IDictionary > ( );

        public DictionaryResourceManager ( string neutralCultureName = null )
        {
            NeutralCultureName = neutralCultureName;
        }

        public string NeutralCultureName { get; set; }

        public void Add ( string cultureName, IDictionary dictionary )
        {
            cache.Add ( cultureName, dictionary );
        }

        public void Remove ( string cultureName )
        {
            cache.Remove ( cultureName );
        }

        public void Clear ( )
        {
            cache.Clear ( );
        }

        public IDictionary this [ string cultureName ]
        {
            get
            {
                if ( cache.TryGet ( cultureName, out var resourceSet ) )
                    return resourceSet;

                return null;
            }
        }

        protected override ResourceSet InternalGetResourceSet ( CultureInfo culture, bool createIfNotExists, bool tryParents )
        {
            var resourceSet =  this [ culture.Name ];

            if ( resourceSet == null && tryParents )
            {
                var parent = culture.Parent;
                if ( parent.Name != culture.Name )
                    resourceSet = this [ parent.Name ];
            }

            if ( resourceSet == null && NeutralCultureName != null )
                resourceSet = this [ NeutralCultureName ];

            if ( resourceSet != null )
                return new ResourceSet ( new DictionaryResourceReader ( resourceSet ) );

            return null;
        }
    }
}