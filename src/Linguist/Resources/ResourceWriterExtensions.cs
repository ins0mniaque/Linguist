using System.IO;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;

namespace Linguist.Resources
{
    public static class ResourceWriterExtensions
    {
        public static void AddResource ( this ResourceWriter writer, IResource resource )
        {
            if ( ! writer.TryAddBinaryResource ( resource ) )
                ( (IResourceWriter) writer ).AddResource ( resource );
        }

        public static void AddResource ( this IResourceWriter writer, IResource resource )
        {
            var value = resource.Value;

            if      ( value is string   text ) writer.AddResource ( resource.Name, text  );
            else if ( value is byte [ ] data ) writer.AddResource ( resource.Name, data  );
            else                               writer.AddResource ( resource.Name, value );
        }

        private static bool TryAddBinaryResource ( this ResourceWriter writer, IResource resource )
        {
            var binary = resource.Type != TypeNames.String &&
                         resource.Type != TypeNames.ByteArray;
            if ( ! binary )
                return false;

            var value = resource.Value;
            if ( value == null )
                return false;

            var binaryFormatter = new BinaryFormatter ( ) { Binder            = TypeResolver.Binder,
                                                            SurrogateSelector = TypeResolver.SurrogateSelector };

            using ( var stream = new MemoryStream ( ) )
            {
                binaryFormatter.Serialize ( stream, resource.Value );

                writer.AddResourceData ( resource.Name,
                                         resource.Type.ToString ( ),
                                         stream.ToArray ( ) );
            }

            return true;
        }
    }
}