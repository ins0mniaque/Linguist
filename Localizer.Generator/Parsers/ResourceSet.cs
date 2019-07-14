using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.CodeDom;

namespace Localizer.Generator
{
    public delegate CodeExpression ResourceManagerInitializer ( string resourcesBaseName, string className );

    public class ResourceSet
    {
        public ResourceSet ( CodeTypeReference resourceManagerType, ResourceManagerInitializer resourceManagerInitializer, IDictionary < string, Resource > resources )
        {
            ResourceManagerType         = resourceManagerType;
            ResourceManagerInitializer  = resourceManagerInitializer;
            Resources                   = new ReadOnlyDictionary < string, Resource > ( resources );
        }

        public CodeTypeReference          ResourceManagerType        { get; }
        public ResourceManagerInitializer ResourceManagerInitializer { get; }

        public IReadOnlyDictionary < string, Resource > Resources { get; }
    }
}