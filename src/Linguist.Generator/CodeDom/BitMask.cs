using System.CodeDom;
using System.Reflection;

namespace Linguist.CodeDom
{
    public static class BitMask
    {
        public static bool HasBitMask ( this MemberAttributes attributes, MemberAttributes bitMask )
        {
            return ( attributes & bitMask ) == bitMask;
        }

        public static bool HasBitMask ( this TypeAttributes attributes, TypeAttributes bitMask )
        {
            return ( attributes & bitMask ) == bitMask;
        }
    }
}