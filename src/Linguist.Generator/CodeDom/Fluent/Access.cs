using System.CodeDom;

namespace Linguist.CodeDom.Fluent
{
    /// <summary>
    /// Fluent access setter to deal with CodeDom's verbosity
    /// </summary>
    public static class Access
    {
        public static T Public < T > ( this T member ) where T : CodeTypeMember
        {
            member.Modifiers ( MemberAttributes.Public );

            return member;
        }

        public static T Internal < T > ( this T member ) where T : CodeTypeMember
        {
            member.Modifiers ( MemberAttributes.Assembly );

            return member;
        }

        public static T Protected < T > ( this T member ) where T : CodeTypeMember
        {
            member.Modifiers ( MemberAttributes.Family );

            return member;
        }

        public static T Private < T > ( this T member ) where T : CodeTypeMember
        {
            member.Modifiers ( MemberAttributes.Private );

            return member;
        }

        public static CodeMemberField Const ( this CodeMemberField member )
        {
            member.Attributes |= MemberAttributes.Const;

            return member;
        }

        public static T Static < T > ( this T member ) where T : CodeTypeMember
        {
            member.Attributes |= MemberAttributes.Static;

            return member;
        }

        public static T Override < T > ( this T member ) where T : CodeTypeMember
        {
            member.Attributes |= MemberAttributes.Override;

            return member;
        }

        public static T Modifiers < T > ( this T member, MemberAttributes memberAttributes ) where T : CodeTypeMember
        {
            var access = memberAttributes & MemberAttributes.AccessMask;
            if ( access != 0 )
                member.Attributes = ( member.Attributes & ~MemberAttributes.AccessMask ) | access;

            var scope = memberAttributes & MemberAttributes.ScopeMask;
            if ( scope != 0 )
                member.Attributes = ( member.Attributes & ~MemberAttributes.ScopeMask ) | scope;

            var vTable = memberAttributes & MemberAttributes.VTableMask;
            if ( vTable != 0 )
                member.Attributes = ( member.Attributes & ~MemberAttributes.VTableMask ) | vTable;

            return member;
        }

        public static CodeTypeDeclaration Partial ( this CodeTypeDeclaration type )
        {
            type.IsPartial = true;

            return type;
        }

        public static CodeTypeDeclaration IsPartial ( this CodeTypeDeclaration type, bool isPartial )
        {
            type.IsPartial = isPartial;

            return type;
        }
    }
}