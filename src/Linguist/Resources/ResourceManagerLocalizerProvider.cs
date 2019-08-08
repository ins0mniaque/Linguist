using System;
using System.IO;
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
                                             GetResourceSetType ( path ) );

            return ResourceManager.CreateFileBasedResourceManager ( Path.GetFileName      ( path ),
                                                                    Path.GetDirectoryName ( path ),
                                                                    GetResourceSetType    ( path ) );
        }

        private static Type GetResourceSetType ( string path )
        {
            var extension = Localizer.Path.GetExtension ( path )?.ToLowerInvariant ( );

            switch ( extension )
            {
                case "resx" :
                case "resw" :
                default     : return null;
            }
        }
    }
}