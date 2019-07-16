using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.CodeDom;

namespace Localizer.Generator
{
    public delegate CodeExpression ResourceManagerInitializer ( string resourcesBaseName, string className );
    public delegate CodeExpression ResourceSetGetter          ( CodeExpression resourceManager, CodeExpression culture );

    public class ResourceSet
    {
        public ResourceSet ( CodeTypeReference resourceManagerType, ResourceManagerInitializer resourceManagerInitializer, ResourceSetGetter resourceSetGetter, IDictionary < string, Resource > resources )
        {
            ResourceManagerType        = resourceManagerType;
            ResourceManagerInitializer = resourceManagerInitializer;
            ResourceSetGetter          = resourceSetGetter;
            Resources                  = new ReadOnlyDictionary < string, Resource > ( resources );
        }

        public CodeTypeReference          ResourceManagerType        { get; }
        public ResourceManagerInitializer ResourceManagerInitializer { get; }
        public ResourceSetGetter          ResourceSetGetter          { get; }

        public IReadOnlyDictionary < string, Resource > Resources { get; }
    }
}