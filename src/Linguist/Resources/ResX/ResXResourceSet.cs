using System;
using System.Collections;
using System.IO;

namespace Linguist.Resources.ResX
{
    /// <summary>Represents all resources in an XML resource (.resx) file.</summary>
    public class ResXResourceSet : Common.ResourceSet
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="T:Linguist.Resources.ResX.ResXResourceSet" /> class
        /// using the default <see cref="T:Linguist.Resources.ResX.ResXResourceExtractor" /> that opens
        /// and reads resources from the specified file.
        /// </summary>
        /// <param name="fileName">The name of the file to read resources from.</param>
        public ResXResourceSet ( string fileName )
        {
            Reader = new ResXResourceExtractor ( fileName );
            Table  = new Hashtable ( );

            ReadResources ( );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Linguist.Resources.ResX.ResXResourceSet" /> class
        /// using the default <see cref="T:Linguist.Resources.ResX.ResXResourceExtractor" /> to read resources
        /// from the specified stream.
        /// </summary>
        /// <param name="stream">The <see cref="T:System.IO.Stream" /> of resources to be read. The stream should refer to an existing resource file.</param>
        public ResXResourceSet ( Stream stream )
        {
            Reader = new ResXResourceExtractor ( stream );
            Table  = new Hashtable ( );

            ReadResources ( );
        }

        /// <summary>Returns the preferred resource reader class for this kind of <see cref="T:Linguist.Resources.ResX.ResXResourceSet" />.</summary>
        /// <returns>The <see cref="T:System.Type" /> of the preferred resource reader for this kind of <see cref="T:Linguist.Resources.ResX.ResXResourceSet" />.</returns>
        public override Type GetDefaultReader ( ) => typeof ( ResXResourceExtractor );

        /// <summary>Returns the preferred resource writer class for this kind of <see cref="T:Linguist.Resources.ResX.ResXResourceSet" />.</summary>
        /// <returns>The <see cref="T:System.Type" /> of the preferred resource writer for this kind of <see cref="T:Linguist.Resources.ResX.ResXResourceSet" />.</returns>
        public override Type GetDefaultWriter ( ) => throw new NotImplementedException ( "ResX resource writer is not implemented" );
    }
}