#if UNSAFE
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Linguist.Resources.Binary
{
    internal sealed unsafe class PinnedBufferMemoryStream : UnmanagedMemoryStream
    {
        private readonly byte [ ] buffer;
        private readonly GCHandle handle;

        internal PinnedBufferMemoryStream ( byte [ ] array )
        {
            buffer = array ?? throw new ArgumentNullException ( nameof ( array ) );
            handle = GCHandle.Alloc ( buffer, GCHandleType.Pinned );

            var length = buffer.Length;
            fixed ( byte* ptr = buffer )
                Initialize ( ptr, length, length, FileAccess.Read );
        }

        ~PinnedBufferMemoryStream ( )
        {
            Dispose ( false );
        }

        protected override void Dispose ( bool disposing )
        {
            if ( handle.IsAllocated )
                handle.Free ( );

            base.Dispose ( disposing );
        }
    }
}
#endif