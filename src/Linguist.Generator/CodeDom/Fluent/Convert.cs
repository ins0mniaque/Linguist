using System.CodeDom;
using System.Reflection;

namespace Linguist.CodeDom.Fluent
{
    /// <summary>
    /// Reflection <=> CodeDom conversions
    /// </summary>
    public static class Convert
    {
        public static ParameterAttributes ToParameterAttributes ( this FieldDirection direction )
        {
            var paramAttributes = ParameterAttributes.None;

            // Only few param attributes are supported
            switch ( direction )
            {
                case FieldDirection.In:
                    paramAttributes = ParameterAttributes.In;
                    break;
                case FieldDirection.Out:
                    paramAttributes = ParameterAttributes.Out;
                    break;
                default:
                    paramAttributes = default ( ParameterAttributes );
                    break;
            }

            return paramAttributes;
        }

        public static MethodAttributes ToMethodAttributes ( this MemberAttributes memberAttributes )
        {
            var methodAttributes = MethodAttributes.ReuseSlot;

            // convert access attributes
            if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.Assembly )
                methodAttributes |= MethodAttributes.Assembly;
            else if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.Family )
                methodAttributes |= MethodAttributes.Family;
            else if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.FamilyAndAssembly )
                methodAttributes |= MethodAttributes.FamANDAssem;
            else if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.FamilyOrAssembly )
                methodAttributes |= MethodAttributes.FamORAssem;
            else if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.Private )
                methodAttributes |= MethodAttributes.Private;
            else if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.Public )
                methodAttributes |= MethodAttributes.Public;

            // covert scope attributes
            if ( ( memberAttributes & MemberAttributes.ScopeMask ) == MemberAttributes.Abstract )
                methodAttributes |= MethodAttributes.Abstract;
            else if ( ( memberAttributes & MemberAttributes.ScopeMask ) == MemberAttributes.Final )
                methodAttributes |= MethodAttributes.Final;
            else if ( ( memberAttributes & MemberAttributes.ScopeMask ) == MemberAttributes.Static )
                methodAttributes |= MethodAttributes.Static;
            else if ( ( memberAttributes & MemberAttributes.ScopeMask ) == MemberAttributes.Override )
                methodAttributes |= MethodAttributes.ReuseSlot;

            // convert vtable slot
            if ( ( memberAttributes & MemberAttributes.VTableMask ) == MemberAttributes.New )
                methodAttributes |= MethodAttributes.NewSlot;
            if ( ( memberAttributes & MemberAttributes.VTableMask ) == MemberAttributes.Overloaded )
                methodAttributes |= MethodAttributes.HideBySig;

            return methodAttributes;
        }

        public static FieldAttributes ToFieldAttributes ( this MemberAttributes memberAttributes )
        {
            var fieldAttributes = default(FieldAttributes);

            if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.Assembly )
                fieldAttributes |= FieldAttributes.Assembly;
            else if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.Family )
                fieldAttributes |= FieldAttributes.Family;
            else if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.FamilyAndAssembly )
                fieldAttributes |= FieldAttributes.FamANDAssem;
            else if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.FamilyOrAssembly )
                fieldAttributes |= FieldAttributes.FamORAssem;
            else if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.Private )
                fieldAttributes |= FieldAttributes.Private;
            else if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.Public )
                fieldAttributes |= FieldAttributes.Public;

            if ( ( memberAttributes & MemberAttributes.ScopeMask ) == MemberAttributes.Const )
                fieldAttributes |= ( FieldAttributes.Static | FieldAttributes.Literal );
            else if ( ( memberAttributes & MemberAttributes.ScopeMask ) == MemberAttributes.Static )
                fieldAttributes |= FieldAttributes.Static;

            return fieldAttributes;
        }

        public static TypeAttributes ToTypeAttributes ( this MemberAttributes memberAttributes, bool nested = false )
        {
            var typeAttributes = default(TypeAttributes);

            // convert access attributes
            if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.Assembly )
                typeAttributes |= ( nested ? TypeAttributes.NestedAssembly : TypeAttributes.NotPublic );
            else if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.Family )
                typeAttributes |= ( nested ? TypeAttributes.NestedFamily : TypeAttributes.NotPublic );
            else if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.FamilyAndAssembly )
                typeAttributes |= ( nested ? TypeAttributes.NestedFamANDAssem : TypeAttributes.NotPublic );
            else if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.FamilyOrAssembly )
                typeAttributes |= ( nested ? TypeAttributes.NestedFamORAssem : TypeAttributes.NotPublic );
            else if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.Private )
                typeAttributes |= ( nested ? TypeAttributes.NestedPrivate : TypeAttributes.NotPublic );
            else if ( ( memberAttributes & MemberAttributes.AccessMask ) == MemberAttributes.Public )
                typeAttributes |= ( nested ? TypeAttributes.NestedPublic : TypeAttributes.Public );

            // covert scope attributes
            if ( ( memberAttributes & MemberAttributes.ScopeMask ) == MemberAttributes.Abstract )
                typeAttributes |= TypeAttributes.Abstract;
            else if ( ( memberAttributes & MemberAttributes.ScopeMask ) == MemberAttributes.Final )
                typeAttributes |= TypeAttributes.Sealed;

            return typeAttributes;
        }
    }
}