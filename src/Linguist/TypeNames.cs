using System.IO;

namespace Linguist
{
    public static class TypeNames
    {
        public static readonly TypeName Void                  = new TypeName ( typeof ( void     ) );
        public static readonly TypeName Object                = new TypeName ( typeof ( object   ) );
        public static readonly TypeName String                = new TypeName ( typeof ( string   ) );
        public static readonly TypeName ByteArray             = new TypeName ( typeof ( byte [ ] ) );
        public static readonly TypeName Stream                = new TypeName ( typeof ( Stream                ) );
        public static readonly TypeName MemoryStream          = new TypeName ( typeof ( MemoryStream          ) );
        public static readonly TypeName UnmanagedMemoryStream = new TypeName ( typeof ( UnmanagedMemoryStream ) );
    }
}