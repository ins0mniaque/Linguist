using System.Reflection;
using System.Resources;

namespace Linguist.Resources
{
    public class ResourceManagerLocalizerProvider : ILocalizerProvider
    {
        public ILocalizer Load ( string path )
        {
            var type = Localizer.Path.GetType ( path );
            if ( type == Localizer.Path.Type.Protocol )
                return null;

            return new ResourceManagerLocalizer ( LoadResourceManager ( type, path ) );
        }

        private static ResourceManager LoadResourceManager ( Localizer.Path.Type type, string path )
        {
            if ( type == Localizer.Path.Type.ManifestResource )
                return new ResourceManager ( Localizer.Path.GetManifestResourceName ( path ),
                                             Assembly.Load ( Localizer.Path.GetManifestResourceAssembly ( path ) ),
                                             AutoDetect.ResourceSetType ( path ) );

            return new FileBasedResourceManager ( AutoDetect.PathFormat ( path, out var neutralCultureName ),
                                                  neutralCultureName,
                                                  AutoDetect.ResourceSetType ( path ) );
        }
    }
}