using System;
using System.Collections;
using System.Resources;

namespace Linguist.Resources.Dictionary
{
    public class DictionaryResourceReader : IResourceReader
    {
        private readonly IDictionary resources;

        public DictionaryResourceReader ( IDictionary resources )
        {
            this.resources = resources;
        }

        void IResourceReader.Close   ( ) { }
        void IDisposable    .Dispose ( ) { }

        public IDictionaryEnumerator GetEnumerator ( ) => resources.GetEnumerator ( );
        IEnumerator      IEnumerable.GetEnumerator ( ) => resources.GetEnumerator ( );
    }
}