using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Resources;

namespace Linguist.Resources.Binary
{
    /// <summary>Represents all resources in a binary resource (.resources) file.</summary>
    public class BinaryResourceSet : Common.ResourceSet
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="T:Linguist.Resources.Binary.BinaryResourceSet" /> class
        /// using the default <see cref="T:Linguist.Resources.Binary.BinaryResourceExtractor" /> that opens
        /// and reads resources from the specified file.
        /// </summary>
        /// <param name="reader">The reader to read resources from.</param>
        public BinaryResourceSet ( IResourceReader reader )
        {
            // NOTE: ResourceManager does not respect DefaultReader type and
            //       creates a default ResourceReader. This forces the reader
            //       back to a BinaryResourceExtractor.
            if ( reader is ResourceReader resourceReader )
                reader = ConvertToDefaultReader ( resourceReader );

            Reader = reader;
            Table  = new Hashtable ( );

            ReadResources ( );
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="T:Linguist.Resources.Binary.BinaryResourceSet" /> class
        /// using the default <see cref="T:Linguist.Resources.Binary.BinaryResourceExtractor" /> that opens
        /// and reads resources from the specified file.
        /// </summary>
        /// <param name="fileName">The name of the file to read resources from.</param>
        public BinaryResourceSet ( string fileName )
        {
            Reader = new BinaryResourceExtractor ( fileName );
            Table  = new Hashtable ( );

            ReadResources ( );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Linguist.Resources.Binary.BinaryResourceSet" /> class
        /// using the default <see cref="T:Linguist.Resources.Binary.BinaryResourceExtractor" /> to read resources
        /// from the specified stream.
        /// </summary>
        /// <param name="stream">The <see cref="T:System.IO.Stream" /> of resources to be read. The stream should refer to an existing resource file.</param>
        public BinaryResourceSet ( Stream stream )
        {
            Reader = new BinaryResourceExtractor ( stream );
            Table  = new Hashtable ( );

            ReadResources ( );
        }

        /// <summary>Returns the preferred resource reader class for this kind of <see cref="T:Linguist.Resources.Binary.BinaryResourceSet" />.</summary>
        /// <returns>The <see cref="T:System.Type" /> of the preferred resource reader for this kind of <see cref="T:Linguist.Resources.Binary.BinaryResourceSet" />.</returns>
        public override Type GetDefaultReader ( ) => typeof ( BinaryResourceExtractor );

        /// <summary>Returns the preferred resource writer class for this kind of <see cref="T:Linguist.Resources.Binary.BinaryResourceSet" />.</summary>
        /// <returns>The <see cref="T:System.Type" /> of the preferred resource writer for this kind of <see cref="T:Linguist.Resources.Binary.BinaryResourceSet" />.</returns>
        public override Type GetDefaultWriter ( ) => throw new NotImplementedException ( "Binary resource writer is not implemented" );

        private static FieldInfo StoreField;

        private static IResourceReader ConvertToDefaultReader ( ResourceReader resourceReader )
        {
            if ( StoreField == null )
                StoreField = typeof ( ResourceReader ).GetField ( "_store", BindingFlags.NonPublic | BindingFlags.Instance );

            var store = StoreField?.GetValue ( resourceReader ) as BinaryReader;
            if ( store == null )
                return resourceReader;

            store.BaseStream.Seek ( 0, SeekOrigin.Begin );

            return new BinaryResourceExtractor ( store.BaseStream );
        }
    }
}