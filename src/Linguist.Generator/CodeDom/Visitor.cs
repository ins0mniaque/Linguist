using System.CodeDom;
using System.Collections.Generic;

namespace Linguist.CodeDom
{
    public static class Visitor
    {
        public static bool Contains ( this CodeTypeMemberCollection members, string name )
        {
            foreach ( CodeTypeMember member in members )
                if ( member?.Name == name )
                    return true;

            return false;
        }

        public static IEnumerable < CodeTypeDeclaration > WithNestedTypes ( this CodeTypeDeclaration type )
        {
            var stack = new Stack < CodeTypeDeclaration > ( );

            stack.Push ( type );

            while ( stack.Count > 0 )
            {
                var next = stack.Pop ( );

                yield return next;

                foreach ( var child in next.Members )
                    if( child is CodeTypeDeclaration nestedType )
                        stack.Push ( nestedType );
            }
        }
    }
}