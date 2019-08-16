using System;
using System.CodeDom;

namespace Linguist.CodeDom.Fluent
{
    /// <summary>
    /// Fluent declaration builder to deal with CodeDom's verbosity
    /// </summary>
    public static class Declare
    {
        public static CodeAttributeDeclaration Attribute ( CodeTypeReference type )
        {
            return new CodeAttributeDeclaration ( type );
        }

        public static CodeAttributeDeclaration Attribute ( CodeTypeReference type, params object [ ] arguments )
        {
            var attribute = new CodeAttributeDeclaration ( type );
            foreach ( var argument in arguments )
                attribute.Arguments.Add ( new CodeAttributeArgument ( Code.Constant ( argument ) ) );

            return attribute;
        }

        public static CodeAttributeDeclaration Attribute ( CodeTypeReference type, params CodeExpression [ ] arguments )
        {
            var attribute = new CodeAttributeDeclaration ( type );
            foreach ( var argument in arguments )
                attribute.Arguments.Add ( new CodeAttributeArgument ( argument ) );

            return attribute;
        }

        public static CodeAttributeDeclaration Attribute < T > ( ) where T : Attribute
        {
            return Attribute ( Code.Type < T > ( ) );
        }

        public static CodeAttributeDeclaration Attribute < T > ( params object [ ] arguments ) where T : Attribute
        {
            return Attribute ( Code.Type < T > ( ), arguments );
        }

        public static CodeAttributeDeclaration Attribute < T > ( params CodeExpression [ ] arguments ) where T : Attribute
        {
            return Attribute ( Code.Type < T > ( ), arguments );
        }

        public static CodeAttributeDeclaration WithArgument ( this CodeAttributeDeclaration attribute, string name, object value )
        {
            return attribute.WithArgument ( name, Code.Constant ( value ) );
        }

        public static CodeAttributeDeclaration WithArgument ( this CodeAttributeDeclaration attribute, string name, CodeExpression value )
        {
            attribute.Arguments.Add ( new CodeAttributeArgument ( name, value ) );

            return attribute;
        }

        public static T Attributed < T > ( this T codeTypeMember, params CodeAttributeDeclaration [ ] attributes ) where T : CodeTypeMember
        {
            codeTypeMember.CustomAttributes.AddRange ( attributes );

            return codeTypeMember;
        }

        public static CodeTypeDeclaration Class ( string name )
        {
            return Type ( name, false );
        }

        public static CodeTypeDeclaration NestedClass ( string name )
        {
            return Type ( name, true );
        }

        public static CodeTypeDeclaration Struct ( string name )
        {
            return Type ( name, false, type => type.IsStruct = true );
        }

        public static CodeTypeDeclaration NestedStruct ( string name )
        {
            return Type ( name, true, type => type.IsStruct = true );
        }

        public static CodeTypeDeclaration Interface ( string name )
        {
            return Type ( name, false, type => type.IsInterface = true );
        }

        public static CodeTypeDeclaration NestedInterface ( string name )
        {
            return Type ( name, true, type => type.IsInterface = true );
        }

        public static CodeTypeDeclaration Enum ( string name )
        {
            return Type ( name, false, type => type.IsEnum = true );
        }

        public static CodeTypeDeclaration NestedEnum ( string name )
        {
            return Type ( name, true, type => type.IsEnum = true );
        }

        private static CodeTypeDeclaration Type ( string name, bool nested, Action < CodeTypeDeclaration > configure = null )
        {
            var type = new CodeTypeDeclaration ( name ) { Attributes     = MemberAttributes.Public,
                                                          TypeAttributes = MemberAttributes.Public.ToTypeAttributes ( nested ) };

            configure?.Invoke ( type );

            return type;
        }

        public static CodeMemberEvent Event ( CodeTypeReference handlerType, string name )
        {
            return new CodeMemberEvent ( ) { Name = name, Type = handlerType, Attributes = MemberAttributes.Public };
        }

        public static CodeMemberEvent Event < THandler > ( string name )
        {
            return Event ( Code.Type < THandler > ( ), name );
        }

        public static CodeMemberMethod Method ( string name, MemberAttributes accessModifiers )
        {
            return new CodeMemberMethod ( ) { Name = name, ReturnType = Code.Type ( typeof ( void ) ), Attributes = accessModifiers };
        }

        public static CodeMemberMethod Method ( CodeTypeReference returnType, string name, MemberAttributes accessModifiers )
        {
            return new CodeMemberMethod ( ) { Name = name, ReturnType = returnType, Attributes = accessModifiers };
        }

        public static CodeMemberMethod Method < TReturn > ( string name, MemberAttributes accessModifiers )
        {
            return new CodeMemberMethod ( ) { Name = name, ReturnType = Code.Type < TReturn > ( ), Attributes = accessModifiers };
        }

        public static CodeMemberMethod Define ( this CodeMemberMethod method, Action < CodeStatementCollection > define )
        {
            define ( method.Statements );

            return method;
        }

        public static CodeMemberField Field < T > ( string name )
        {
            return Field ( Code.Type < T > ( ), name );
        }

        public static CodeMemberField Field ( CodeTypeReference type, string name )
        {
            return new CodeMemberField ( type, name ) { Attributes = MemberAttributes.Private };
        }

        public static CodeMemberField Initialize ( this CodeMemberField field, CodeExpression initialValue )
        {
            field.InitExpression = initialValue;

            return field;
        }

        public static CodeMemberProperty Property < T > ( string name )
        {
            return Property ( Code.Type < T > ( ), name );
        }

        public static CodeMemberProperty Property ( CodeTypeReference type, string name )
        {
            return new CodeMemberProperty ( ) { Type = type, Name = name, Attributes = MemberAttributes.Public };
        }

        public static CodeMemberProperty Get ( this CodeMemberProperty property, Action < CodeStatementCollection > get )
        {
            property.HasGet = true;

            get ( property.GetStatements );

            return property;
        }

        public static CodeMemberProperty Set ( this CodeMemberProperty property, Action < CodeStatementCollection, CodePropertySetValueReferenceExpression > set )
        {
            property.HasSet = true;

            set ( property.SetStatements, new CodePropertySetValueReferenceExpression ( ) );

            return property;
        }

        public static CodeVariableDeclarationStatement Variable ( CodeTypeReference type, string name )
        {
            return new CodeVariableDeclarationStatement ( type, name );
        }

        public static CodeVariableDeclarationStatement Variable < T > ( string name )
        {
            return new CodeVariableDeclarationStatement ( Code.Type < T > ( ), name );
        }

        public static CodeVariableDeclarationStatement Initialize ( this CodeVariableDeclarationStatement variable, CodeExpression initialValue )
        {
            variable.InitExpression = initialValue;

            return variable;
        }

        public static CodeNamespace Add ( this CodeNamespaceCollection namespaces, string @namespace, params string [ ] imports )
        {
            var codeNamespace = new CodeNamespace ( @namespace );
            foreach ( var import in imports )
                codeNamespace.Imports.Add ( new CodeNamespaceImport ( import ) );

            namespaces.Add ( codeNamespace );

            return codeNamespace;
        }

        public static CodeTypeDeclaration AddTo ( this CodeTypeDeclaration type, CodeNamespace @namespace )
        {
            @namespace.Types.Add ( type );

            return type;
        }

        public static T AddTo < T > ( this T member, CodeTypeDeclaration type ) where T : CodeTypeMember
        {
            type.Members.Add ( member );

            return member;
        }
    }
}