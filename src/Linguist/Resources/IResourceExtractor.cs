using System.Collections;
using System.Collections.Generic;
using System.Resources;

namespace Linguist.Resources
{
    public interface IResourceExtractor : IResourceReader
    {
        IEnumerable < IResource >       Extract ( );
        IEnumerable < DictionaryEntry > Read    ( );
    }
}