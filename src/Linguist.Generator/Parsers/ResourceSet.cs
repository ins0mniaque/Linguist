using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.CodeDom;

namespace Linguist.Generator
{
    public delegate CodeExpression ResourceManagerInitializer ( string resourcesBaseName, string className );
    public delegate CodeExpression LocalizerInitializer       ( CodeExpression resourceManager, CodeExpression resourceNamingStrategy );

    public class ResourceSet
    {
        public ResourceSet ( CodeTypeReference resourceManagerType, ResourceManagerInitializer resourceManagerInitializer, CodeTypeReference localizerType, LocalizerInitializer localizerInitializer, IDictionary < string, Resource > resources )
        {
            ResourceManagerType        = resourceManagerType;
            ResourceManagerInitializer = resourceManagerInitializer;
            LocalizerType              = localizerType;
            LocalizerInitializer       = localizerInitializer;
            Resources                  = new ReadOnlyDictionary < string, Resource > ( resources );
        }

        public CodeTypeReference          ResourceManagerType        { get; }
        public ResourceManagerInitializer ResourceManagerInitializer { get; }
        public CodeTypeReference          LocalizerType              { get; }
        public LocalizerInitializer       LocalizerInitializer       { get; }

        public ReadOnlyDictionary < string, Resource > Resources { get; }
    }

    #if NET35
    public class ReadOnlyDictionary < TKey, TValue > : Dictionary < TKey, TValue >
    {
        public ReadOnlyDictionary ( IDictionary < TKey, TValue > dictionary ) : base ( dictionary ) { }
    }
    #endif
}