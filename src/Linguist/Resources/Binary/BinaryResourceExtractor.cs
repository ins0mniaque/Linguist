using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Linguist.Resources.Binary
{
    /// <summary>
    /// Provides an implementation of IResourceReader, reading
    /// .resources file from the system default binary format.
    /// This class can be treated as an enumerator once.
    /// </summary>
    public sealed class BinaryResourceExtractor : IResourceExtractor
    {
        // A reasonable default buffer size for reading from files, especially
        // when we will likely be seeking frequently.  Could be smaller, but does
        // it make sense to use anything less than one page?
        private const int DefaultFileStreamBufferSize = 4096;

        private BinaryReader    store; // Backing store we're reading from
        private BinaryFormatter binaryFormatter;

        private long nameSectionOffset;  // Offset to name section of file
        private long dataSectionOffset;  // Offset to Data section of file

        // Note this class is tightly coupled with UnmanagedMemoryStream.
        // At runtime when getting an embedded resource from an assembly,
        // we're given an UnmanagedMemoryStream referring to the mmap'ed portion
        // of the assembly.  The pointers here are pointers into that block of
        // memory controlled by the OS's loader.
        private int  [ ] nameHashes;         // Hash values for all names
        private int  [ ] namePositions;      // Relative locations of names
        private Type [ ] typeTable;          // Lazy array of Types for resource values
        private int  [ ] typeNamePositions;  // To delay initialize type table
        private int      numResources;       // Num of resources files, in case arrays aren't allocated

        #if UNSAFE
        // We'll include a separate code path that uses UnmanagedMemoryStream to
        // avoid allocating String objects and the like.
        private UnmanagedMemoryStream ums;
        private unsafe int* nameHashesPtr;    // Hash values for all names
        private unsafe int* namePositionsPtr; // Relative locations of names
        #endif

        // Version number of .resources file, for compatibility
        private int version;

        public BinaryResourceExtractor ( string fileName )
        {
            store = new BinaryReader ( new FileStream ( fileName, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultFileStreamBufferSize, FileOptions.RandomAccess ), Encoding.UTF8 );

            try
            {
                ReadResources ( );
            }
            catch
            {
                store.Close ( ); // If we threw an exception, close the file.
                throw;
            }
        }

        public BinaryResourceExtractor ( Stream stream )
        {
            if ( stream == null   ) throw new ArgumentNullException ( nameof ( stream ) );
            if ( ! stream.CanRead ) throw new ArgumentException ( SR.Argument_StreamNotReadable );

            store = new BinaryReader ( stream, Encoding.UTF8 );

            #if UNSAFE
            // We have a faster code path for reading resource files from an assembly.
            ums = stream as UnmanagedMemoryStream;
            #endif

            ReadResources ( );
        }

        public IEnumerable < IResource > Extract ( )
        {
            var enumerator = (Enumerator) GetEnumerator ( );
            var source     = ResourceExtractor.GetStreamSource ( store.BaseStream );

            while ( enumerator.MoveNext ( ) )
            {
                var entry    = enumerator.Entry;
                var position = enumerator.Position;

                yield return new Resource ( ) { Name   = (string) entry.Key,
                                                Value  = entry.Value,
                                                Source = source,
                                                Column = position };
            }
        }

        public IEnumerable < DictionaryEntry > Read ( )
        {
            var enumerator = GetEnumerator ( );
            while ( enumerator.MoveNext ( ) )
                yield return enumerator.Entry;
        }

        // Reads in the header information for a .resources file.  Verifies some
        // of the assumptions about this resource set, and builds the class table
        // for the default resource file format.
        private void ReadResources ( )
        {
            Debug.Assert ( store != null, "ResourceReader is closed" );

            try
            {
                // NOTE: Mega try-catch performs exceptionally bad on x64;
                //       factored out body into ReadResourcesCore and wrap here.
                ReadResourcesCore ( );
            }
            catch ( EndOfStreamException eof )
            {
                throw new BadImageFormatException ( SR.BadImageFormat_ResourcesHeaderCorrupted, eof );
            }
            catch ( IndexOutOfRangeException e )
            {
                throw new BadImageFormatException ( SR.BadImageFormat_ResourcesHeaderCorrupted, e );
            }
        }

        private void ReadResourcesCore ( )
        {
            // Check for magic number
            var magicNum = store.ReadInt32 ( );
            if ( magicNum != ResourceManager.MagicNumber )
                throw new ArgumentException ( SR.Resources_StreamNotValid );

            // Assuming this is ResourceManager header V1 or greater, hopefully
            // after the version number there is a number of bytes to skip
            // to bypass the rest of the ResMgr header. For V2 or greater, we
            // use this to skip to the end of the header.
            var resMgrHeaderVersion = store.ReadInt32 ( );
            var numBytesToSkip      = store.ReadInt32 ( );
            if ( numBytesToSkip < 0 || resMgrHeaderVersion < 0 )
                throw new BadImageFormatException ( SR.BadImageFormat_ResourcesHeaderCorrupted );

            if ( resMgrHeaderVersion > 1 )
                store.BaseStream.Seek ( numBytesToSkip, SeekOrigin.Current );
            else
            {
                // We don't care about numBytesToSkip; read the rest of the header

                // Read in type name for a suitable ResourceReader
                // NOTE: ResourceWriter & InternalResGen use different Strings.
                var readerType = store.ReadString ( );

                // Skip over type name for a suitable ResourceSet
                SkipString ( );
            }

            // Do file version check
            version = store.ReadInt32 ( );
            if ( version != 2 && version != 1 )
                throw new ArgumentException ( string.Format ( SR.Arg_ResourceFileUnsupportedVersion, 2, version ) );

            numResources = store.ReadInt32 ( );
            if ( numResources < 0 )
                throw new BadImageFormatException ( SR.BadImageFormat_ResourcesHeaderCorrupted );

            // Read type positions into type positions array.
            // But delay initialize the type table.
            var numTypes = store.ReadInt32 ( );
            if ( numTypes < 0 )
                throw new BadImageFormatException ( SR.BadImageFormat_ResourcesHeaderCorrupted );

            typeTable         = new Type [ numTypes ];
            typeNamePositions = new int  [ numTypes ];
            for ( var i = 0; i < numTypes; i++ )
            {
                typeNamePositions [ i ] = (int) store.BaseStream.Position;

                // Skip over the Strings in the file.  Don't create types.
                SkipString ( );
            }

            // Prepare to read in the array of name hashes
            // Note that the name hashes array is aligned to 8 bytes so
            // we can use pointers into it on 64 bit machines. (4 bytes
            // may be sufficient, but let's plan for the future)
            // Skip over alignment stuff.  All public .resources files
            // should be aligned   No need to verify the byte values.
            var pos = store.BaseStream.Position;
            var alignBytes = ( (int) pos ) & 7;
            if ( alignBytes != 0 )
                for ( var i = 0; i < 8 - alignBytes; i++ )
                    store.ReadByte ( );

            // Read in the array of name hashes
            #if UNSAFE
            if ( ums != null )
            {
                var seekPos = unchecked ( 4 * numResources );
                if ( seekPos < 0 )
                    throw new BadImageFormatException ( SR.BadImageFormat_ResourcesHeaderCorrupted );

                unsafe
                {
                    nameHashesPtr = (int*) ums.PositionPointer;
                    // Skip over the array of nameHashes.
                    ums.Seek ( seekPos, SeekOrigin.Current );
                    // get the position pointer once more to check that the whole table is within the stream
                    _ = ums.PositionPointer;
                }
            }
            else
            #endif
            {
                nameHashes = new int [ numResources ];
                for ( var i = 0; i < numResources; i++ )
                    nameHashes [ i ] = store.ReadInt32 ( );
            }

            // Read in the array of relative positions for all the names.
            #if UNSAFE
            if ( ums != null )
            {
                var seekPos = unchecked ( 4 * numResources );
                if ( seekPos < 0 )
                    throw new BadImageFormatException ( SR.BadImageFormat_ResourcesHeaderCorrupted );

                unsafe
                {
                    namePositionsPtr = (int*) ums.PositionPointer;
                    // Skip over the array of namePositions.
                    ums.Seek ( seekPos, SeekOrigin.Current );
                    // get the position pointer once more to check that the whole table is within the stream
                    _ = ums.PositionPointer;
                }
            }
            else
            #endif
            {
                namePositions = new int [ numResources ];
                for ( var i = 0; i < numResources; i++ )
                {
                    var namePosition = store.ReadInt32();
                    if ( namePosition < 0 )
                        throw new BadImageFormatException ( SR.BadImageFormat_ResourcesHeaderCorrupted );

                    namePositions [ i ] = namePosition;
                }
            }

            // Read location of data section.
            dataSectionOffset = store.ReadInt32 ( );
            if ( dataSectionOffset < 0 )
                throw new BadImageFormatException ( SR.BadImageFormat_ResourcesHeaderCorrupted );

            // Store current location as start of name section
            nameSectionOffset = store.BaseStream.Position;

            // _nameSectionOffset should be <= _dataSectionOffset; if not, it's corrupt
            if ( dataSectionOffset < nameSectionOffset )
                throw new BadImageFormatException ( SR.BadImageFormat_ResourcesHeaderCorrupted );
        }

        IEnumerator      IEnumerable.GetEnumerator ( ) => GetEnumerator ( );
        public IDictionaryEnumerator GetEnumerator ( )
        {
            if ( store == null )
                throw new InvalidOperationException ( SR.ResourceReaderIsClosed );

            return new Enumerator ( this );
        }

        public void Close   ( ) => Dispose ( true );
        public void Dispose ( ) => Dispose ( true );

        private
        #if UNSAFE
        unsafe
        #endif
        void Dispose ( bool disposing )
        {
            if ( store != null )
            {
                if ( disposing )
                    store?.Close ( );

                store         = null;
                namePositions = null;
                nameHashes    = null;

                #if UNSAFE
                ums              = null;
                namePositionsPtr = null;
                nameHashesPtr    = null;
                #endif
            }
        }

        private
        #if UNSAFE
        unsafe
        #endif
        int GetNamePosition ( int index )
        {
            Debug.Assert ( index >= 0 && index < numResources, "Bad index into name position array.  index: " + index );

            int offset;

            #if UNSAFE
            if ( ums != null )
            {
                Debug.Assert ( namePositions == null && namePositionsPtr != null, "Internal state mangled." );
                offset = ReadUnalignedI4 ( &namePositionsPtr [ index ] );
            }
            else
            {
                Debug.Assert ( namePositions != null && namePositionsPtr == null, "Internal state mangled." );
                offset = namePositions [ index ];
            }
            #else
            offset = namePositions [ index ];
            #endif

            if ( offset < 0 || offset > dataSectionOffset - nameSectionOffset )
                throw new FormatException ( string.Format ( SR.BadImageFormat_ResourcesNameInvalidOffset, offset ) );

            return offset;
        }

        // This is used in the enumerator.  The enumerator iterates from 0 to n
        // of our resources and this returns the resource name for a particular
        // index.  The parameter is NOT a virtual offset.
        private
        #if UNSAFE
        unsafe
        #endif
        string AllocateStringForNameIndex ( int index, out int dataOffset )
        {
            Debug.Assert ( store != null, "ResourceReader is closed" );

            byte [ ] bytes;
            int      byteLen;
            var      nameVA = GetNamePosition ( index );

            lock ( this )
            {
                store.BaseStream.Seek ( nameVA + nameSectionOffset, SeekOrigin.Begin );

                // Can't use _store.ReadString, since it's using UTF-8!
                byteLen = Read7BitEncodedInt ( store );
                if ( byteLen < 0 )
                    throw new BadImageFormatException ( SR.BadImageFormat_NegativeStringLength );

                #if UNSAFE
                if ( ums != null )
                {
                    if ( ums.Position > ums.Length - byteLen )
                        throw new BadImageFormatException ( string.Format ( SR.BadImageFormat_ResourcesIndexTooLong, index ) );

                    var s       = (string) null;
                    var charPtr = (char*)  ums.PositionPointer;

                    s = new string ( charPtr, 0, byteLen / 2 );

                    ums.Position += byteLen;
                    dataOffset = store.ReadInt32 ( );
                    if ( dataOffset < 0 || dataOffset >= store.BaseStream.Length - dataSectionOffset )
                        throw new FormatException ( string.Format ( SR.BadImageFormat_ResourcesDataInvalidOffset, dataOffset ) );

                    return s;
                }
                #endif

                bytes = new byte [ byteLen ];

                // We must read byteLen bytes, or we have a corrupted file.
                // Use a blocking read in case the stream doesn't give us back
                // everything immediately.
                var count = byteLen;
                while ( count > 0 )
                {
                    var read = store.Read ( bytes, byteLen - count, count );
                    if ( read == 0 )
                        throw new EndOfStreamException ( string.Format ( SR.BadImageFormat_ResourceNameCorrupted_NameIndex, index ) );

                    count -= read;
                }

                dataOffset = store.ReadInt32 ( );
                if ( dataOffset < 0 || dataOffset >= store.BaseStream.Length - dataSectionOffset )
                    throw new FormatException ( string.Format ( SR.BadImageFormat_ResourcesDataInvalidOffset, dataOffset ) );
            }

            return Encoding.Unicode.GetString ( bytes, 0, byteLen );
        }

        // This is used in the enumerator.  The enumerator iterates from 0 to n
        // of our resources and this returns the resource value for a particular
        // index.  The parameter is NOT a virtual offset.
        private object GetValueForNameIndex ( int index )
        {
            Debug.Assert ( store != null, "ResourceReader is closed" );

            var nameVA = GetNamePosition ( index );

            lock ( this )
            {
                store.BaseStream.Seek ( nameVA + nameSectionOffset, SeekOrigin.Begin );

                SkipString ( );

                var dataPos = store.ReadInt32 ( );
                if ( dataPos < 0 || dataPos >= store.BaseStream.Length - dataSectionOffset )
                    throw new FormatException ( string.Format ( SR.BadImageFormat_ResourcesDataInvalidOffset, dataPos ) );

                return LoadObject ( dataPos );
            }
        }

        private object LoadObject ( int pos )
        {
            if ( version == 1 )
                return LoadObjectV1 ( pos );

            return LoadObjectV2 ( pos );
        }

        // This takes a virtual offset into the data section and reads an Object
        // from that location.
        // Anyone who calls LoadObject should make sure they take a lock so
        // no one can cause us to do a seek in here.
        private object LoadObjectV1 ( int pos )
        {
            Debug.Assert ( store != null, "ResourceReader is closed" );
            Debug.Assert ( version == 1,  ".resources file was not a V1 .resources file" );

            try
            {
                // NOTE: Mega try-catch performs exceptionally bad on x64;
                //       factored out body into LoadObjectV1Core and wrap here.
                return LoadObjectV1Core ( pos );
            }
            catch ( EndOfStreamException eof )
            {
                throw new BadImageFormatException ( SR.BadImageFormat_TypeMismatch, eof );
            }
            catch ( ArgumentOutOfRangeException e )
            {
                throw new BadImageFormatException ( SR.BadImageFormat_TypeMismatch, e );
            }
        }

        private object LoadObjectV1Core ( int pos )
        {
            store.BaseStream.Seek ( dataSectionOffset + pos, SeekOrigin.Begin );

            var typeIndex = Read7BitEncodedInt ( store );
            if ( typeIndex == -1 )
                return null;

            var type = FindType ( typeIndex );

            if      ( type == typeof ( string   ) ) return store.ReadString ( );
            else if ( type == typeof ( int      ) ) return store.ReadInt32  ( );
            else if ( type == typeof ( byte     ) ) return store.ReadByte   ( );
            else if ( type == typeof ( sbyte    ) ) return store.ReadSByte  ( );
            else if ( type == typeof ( short    ) ) return store.ReadInt16  ( );
            else if ( type == typeof ( long     ) ) return store.ReadInt64  ( );
            else if ( type == typeof ( ushort   ) ) return store.ReadUInt16 ( );
            else if ( type == typeof ( uint     ) ) return store.ReadUInt32 ( );
            else if ( type == typeof ( ulong    ) ) return store.ReadUInt64 ( );
            else if ( type == typeof ( float    ) ) return store.ReadSingle ( );
            else if ( type == typeof ( double   ) ) return store.ReadDouble ( );
            else if ( type == typeof ( DateTime ) ) return new DateTime ( store.ReadInt64 ( ) );
            else if ( type == typeof ( TimeSpan ) ) return new TimeSpan ( store.ReadInt64 ( ) );
            else if ( type == typeof ( decimal  ) )
            {
                var bits = new int [ 4 ];
                for ( var i = 0; i < bits.Length; i++ )
                    bits [ i ] = store.ReadInt32 ( );

                return new decimal ( bits );
            }
            else
                return DeserializeObject ( typeIndex );
        }

        private object LoadObjectV2 ( int pos )
        {
            Debug.Assert ( store != null, "ResourceReader is closed" );
            Debug.Assert ( version >= 2,  ".resources file was not a V2 (or higher) .resources file" );

            try
            {
                // NOTE: Mega try-catch performs exceptionally bad on x64;
                //       factored out body into LoadObjectV2Core and wrap here.
                return LoadObjectV2Core ( pos );
            }
            catch ( EndOfStreamException eof )
            {
                throw new BadImageFormatException ( SR.BadImageFormat_TypeMismatch, eof );
            }
            catch ( ArgumentOutOfRangeException e )
            {
                throw new BadImageFormatException ( SR.BadImageFormat_TypeMismatch, e );
            }
        }

        private object LoadObjectV2Core ( int pos )
        {
            store.BaseStream.Seek ( dataSectionOffset + pos, SeekOrigin.Begin );

            var typeCode = (ResourceTypeCode) Read7BitEncodedInt ( store );

            switch ( typeCode )
            {
                case ResourceTypeCode.Null     : return null;
                case ResourceTypeCode.String   : return store.ReadString  ( );
                case ResourceTypeCode.Boolean  : return store.ReadBoolean ( );
                case ResourceTypeCode.Char     : return (char) store.ReadUInt16 ( );
                case ResourceTypeCode.Byte     : return store.ReadByte    ( );
                case ResourceTypeCode.SByte    : return store.ReadSByte   ( );
                case ResourceTypeCode.Int16    : return store.ReadInt16   ( );
                case ResourceTypeCode.UInt16   : return store.ReadUInt16  ( );
                case ResourceTypeCode.Int32    : return store.ReadInt32   ( );
                case ResourceTypeCode.UInt32   : return store.ReadUInt32  ( );
                case ResourceTypeCode.Int64    : return store.ReadInt64   ( );
                case ResourceTypeCode.UInt64   : return store.ReadUInt64  ( );
                case ResourceTypeCode.Single   : return store.ReadSingle  ( );
                case ResourceTypeCode.Double   : return store.ReadDouble  ( );
                case ResourceTypeCode.Decimal  : return store.ReadDecimal ( );
                case ResourceTypeCode.DateTime : return DateTime.FromBinary ( store.ReadInt64 ( ) );
                case ResourceTypeCode.TimeSpan : return new TimeSpan ( store.ReadInt64 ( ) );

                // Special types
                case ResourceTypeCode.ByteArray :
                {
                    var length = store.ReadInt32 ( );
                    if ( length < 0 )
                        throw new BadImageFormatException ( string.Format ( SR.BadImageFormat_ResourceDataLengthInvalid, length ) );

                    #if UNSAFE
                    if ( ums != null )
                    {
                        if ( length > ums.Length - ums.Position )
                            throw new BadImageFormatException ( string.Format ( SR.BadImageFormat_ResourceDataLengthInvalid, length ) );

                        var bytes = new byte [ length ];
                        var read  = ums.Read ( bytes, 0, length );
                        Debug.Assert ( read == length, "ResourceReader needs to use a blocking read here.  (Call _store.ReadBytes ( length ) ?)" );
                        return bytes;
                    }
                    #endif

                    if ( length > store.BaseStream.Length )
                        throw new BadImageFormatException ( string.Format ( SR.BadImageFormat_ResourceDataLengthInvalid, length ) );

                    return store.ReadBytes ( length );
                }

                case ResourceTypeCode.Stream :
                {
                    var length = store.ReadInt32 ( );
                    if ( length < 0 )
                        throw new BadImageFormatException ( string.Format ( SR.BadImageFormat_ResourceDataLengthInvalid, length ) );

                    #if UNSAFE
                    if ( ums != null )
                    {
                        // make sure we don't create an UnmanagedMemoryStream that is longer than the resource stream.
                        if ( length > ums.Length - ums.Position )
                            throw new BadImageFormatException ( string.Format ( SR.BadImageFormat_ResourceDataLengthInvalid, length ) );

                        // For the case that we've memory mapped in the .resources
                        // file, just return a Stream pointing to that block of memory.
                        unsafe
                        {
                            return new UnmanagedMemoryStream ( ums.PositionPointer, length, length, FileAccess.Read );
                        }
                    }
                    #endif

                    var bytes = store.ReadBytes ( length );

                    #if UNSAFE
                    unsafe
                    {
                        // Lifetime of memory == lifetime of this stream.
                        return new PinnedBufferMemoryStream ( bytes );
                    }
                    #else
                    return new MemoryStream ( bytes );
                    #endif
                }

                default :
                    if ( typeCode < ResourceTypeCode.StartOfUserTypes )
                        throw new BadImageFormatException ( SR.BadImageFormat_TypeMismatch );

                    break;
            }

            // Normal serialized objects
            var typeIndex = typeCode - ResourceTypeCode.StartOfUserTypes;

            return DeserializeObject ( typeIndex );
        }

        private object DeserializeObject ( int typeIndex )
        {
            if ( binaryFormatter == null )
                binaryFormatter = new BinaryFormatter ( ) { Binder            = TypeResolver.Binder,
                                                            SurrogateSelector = TypeResolver.SurrogateSelector };

            var type  = FindType ( typeIndex );
            var value = binaryFormatter.Deserialize ( store.BaseStream );

            // Guard against corrupted resources
            if ( value.GetType ( ) != type )
                throw new BadImageFormatException ( string.Format ( SR.BadImageFormat_ResType_SerBlobMismatch, type.FullName, value.GetType ( ).FullName ) );

            return value;
        }

        // This allows us to delay-initialize the Type [ ].  This might be a
        // good startup time savings, since we might have to load assemblies
        // and initialize Reflection.
        private Type FindType ( int typeIndex )
        {
            if ( typeIndex < 0 || typeIndex >= typeTable.Length )
                throw new BadImageFormatException ( SR.BadImageFormat_InvalidType );

            if ( typeTable [ typeIndex ] == null )
            {
                var oldPos = store.BaseStream.Position;

                try
                {
                    store.BaseStream.Position = typeNamePositions [ typeIndex ];
                    var typeName = store.ReadString ( );
                    typeTable [ typeIndex ] = TypeResolver.ResolveType ( typeName );
                }
                finally
                {
                    store.BaseStream.Position = oldPos;
                }
            }

            Debug.Assert ( typeTable [ typeIndex ] != null, "Should have found a type" );

            return typeTable [ typeIndex ];
        }

        private void SkipString ( )
        {
            var stringLength = Read7BitEncodedInt ( store );
            if ( stringLength < 0 )
                throw new BadImageFormatException ( SR.BadImageFormat_NegativeStringLength );

            store.BaseStream.Seek ( stringLength, SeekOrigin.Current );
        }

        #if UNSAFE
        private static unsafe int ReadUnalignedI4 ( int* p )
        {
            var buffer = (byte*) p;
            // Unaligned, little endian format
            return buffer [ 0 ] | ( buffer [ 1 ] << 8 ) | ( buffer [ 2 ] << 16 ) | ( buffer [ 3 ] << 24 );
        }
        #endif

        private static int Read7BitEncodedInt ( BinaryReader reader )
        {
            // Read out an Int32 7 bits at a time.  The high bit
            // of the byte when on means to continue reading more bytes.
            var count = 0;
            var shift = 0;

            byte b;
            do
            {
                // Check for a corrupted stream.  Read a max of 5 bytes.
                // In a future version, add a DataFormatException.
                if ( shift == 5 * 7 )  // 5 bytes max per Int32, shift += 7
                    throw new FormatException ( SR.Format_Bad7BitInt32 );

                // ReadByte handles end of stream cases for us.
                b = reader.ReadByte ( );
                count |= ( b & 0x7F ) << shift;
                shift += 7;
            }
            while ( ( b & 0x80 ) != 0 );

            return count;
        }

        private sealed class Enumerator : IDictionaryEnumerator
        {
            private const int EnumEnded      = int.MinValue;
            private const int EnumNotStarted = -1;

            private readonly BinaryResourceExtractor reader;

            private bool currentIsValid;
            private int  currentName;
            private int  position; // Cached for case-insensitive table

            public Enumerator ( BinaryResourceExtractor reader )
            {
                this.reader = reader;

                currentName = EnumNotStarted;
                position    = -2;
            }

            public bool MoveNext ( )
            {
                if ( currentName == reader.numResources - 1 || currentName == EnumEnded )
                {
                    currentIsValid = false;
                    currentName    = EnumEnded;
                    return false;
                }

                currentIsValid = true;
                currentName++;
                return true;
            }

            public object Key
            {
                get
                {
                    if ( currentName == EnumEnded ) throw new InvalidOperationException ( SR.InvalidOperation_EnumEnded      );
                    if ( ! currentIsValid         ) throw new InvalidOperationException ( SR.InvalidOperation_EnumNotStarted );
                    if ( reader.store == null     ) throw new InvalidOperationException ( SR.ResourceReaderIsClosed          );

                    return reader.AllocateStringForNameIndex ( currentName, out position );
                }
            }

            public object Current => Entry;

            // NOTE: This requires that you call the Key or Entry property FIRST before calling it!
            internal int Position => position;

            public DictionaryEntry Entry
            {
                get
                {
                    if ( currentName == EnumEnded ) throw new InvalidOperationException ( SR.InvalidOperation_EnumEnded      );
                    if ( ! currentIsValid )         throw new InvalidOperationException ( SR.InvalidOperation_EnumNotStarted );
                    if ( reader.store == null )     throw new InvalidOperationException ( SR.ResourceReaderIsClosed          );

                    string key;
                    object value;

                    lock ( reader )
                    {
                        key = reader.AllocateStringForNameIndex ( currentName, out position );

                        if ( position == -1 )
                            value = reader.GetValueForNameIndex ( currentName );
                        else
                            value = reader.LoadObject ( position );
                    }

                    return new DictionaryEntry ( key, value );
                }
            }

            public object Value
            {
                get
                {
                    if ( currentName == EnumEnded ) throw new InvalidOperationException ( SR.InvalidOperation_EnumEnded );
                    if ( ! currentIsValid )         throw new InvalidOperationException ( SR.InvalidOperation_EnumNotStarted );
                    if ( reader.store == null )     throw new InvalidOperationException ( SR.ResourceReaderIsClosed );

                    return reader.GetValueForNameIndex ( currentName );
                }
            }

            public void Reset ( )
            {
                if ( reader.store == null ) throw new InvalidOperationException ( SR.ResourceReaderIsClosed );

                currentIsValid = false;
                currentName    = EnumNotStarted;
            }
        }

        private sealed class Comparer : IComparer, IEqualityComparer, IComparer < string >, IEqualityComparer < string >
        {
            public static readonly Comparer Default = new Comparer ( );

            // This hash function MUST be publically documented with the resource
            // file format, AND we may NEVER change this hash function's return
            // value (without changing the file format).
            private static int HashFunction ( string key )
            {
                // Never change this hash function.  We must standardize it so that
                // others can read & write our .resources files.  Additionally, we
                // have a copy of it in InternalResGen as well.
                var hash = 5381U;
                for ( int i = 0; i < key.Length; i++ )
                    hash = ( ( hash << 5 ) + hash ) ^ key [ i ];
                return (int) hash;
            }

            public int GetHashCode ( object key ) => HashFunction ( (string) key );
            public int GetHashCode ( string key ) => HashFunction ( key );

            public int Compare ( object a, object b ) => a == b ? 0 : string.CompareOrdinal ( (string) a, (string) b );
            public int Compare ( string a, string b ) => string.CompareOrdinal ( a, b );

            public bool     Equals ( string a, string b ) => string.Equals ( a, b );
            public new bool Equals ( object a, object b ) => a == b ? true : string.Equals ( (string) a, (string) b );
        }

        private static class SR
        {
            public const string Arg_ResourceFileUnsupportedVersion             = "The ResourceReader class does not know how to read this version of .resources files. Expected version: {0}  This file: {1}";
            public const string Argument_StreamNotReadable                     = "Stream was not readable.";
            public const string BadImageFormat_ResType_SerBlobMismatch         = "The type serialized in the .resources file was not the same type that the .resources file said it contained. Expected '{0}' but read '{1}'.";
            public const string BadImageFormat_InvalidType                     = "Corrupt .resources file.  The specified type doesn't exist.";
            public const string BadImageFormat_NegativeStringLength            = "Corrupt .resources file. String length must be non-negative.";
            public const string BadImageFormat_ResourcesNameInvalidOffset      = "Corrupt .resources file. Invalid offset '{0}' into name section.";
            public const string BadImageFormat_ResourcesHeaderCorrupted        = "Corrupt .resources file. Unable to read resources from this file because of invalid header information. Try regenerating the .resources file.";
            public const string BadImageFormat_ResourceDataLengthInvalid       = "Corrupt .resources file.  The specified data length '{0}' is not a valid position in the stream.";
            public const string BadImageFormat_TypeMismatch                    = "Corrupt .resources file.  The specified type doesn't match the available data in the stream.";
            public const string BadImageFormat_ResourceNameCorrupted_NameIndex = "Corrupt .resources file. The resource name for name index {0} extends past the end of the stream.";
            public const string BadImageFormat_ResourcesIndexTooLong           = "Corrupt .resources file. String for name index '{0}' extends past the end of the file.";
            public const string BadImageFormat_ResourcesDataInvalidOffset      = "Corrupt .resources file. Invalid offset '{0}' into data section.";
            public const string Format_Bad7BitInt32                            = "Too many bytes in what should have been a 7 bit encoded Int32.";
            public const string InvalidOperation_EnumEnded                     = "Enumeration already finished.";
            public const string InvalidOperation_EnumNotStarted                = "Enumeration has not started. Call MoveNext.";
            public const string ResourceReaderIsClosed                         = "ResourceReader is closed.";
            public const string Resources_StreamNotValid                       = "Stream is not a valid resource file.";
        }
    }
}