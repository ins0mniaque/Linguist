using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;

namespace Localizer.CodeDom
{
    /// <summary>
    /// Fluent builder to deal with CodeDom's verbosity
    /// </summary>
    public static class Code
    {
        public static CodeThisReferenceExpression This ( )
        {
            return new CodeThisReferenceExpression ( );
        }

        public static CodeExpression Static ( )
        {
            return null;
        }

        public static CodeTypeReferenceExpression Type < T > ( )
        {
            return new CodeTypeReferenceExpression ( TypeRef < T > ( ) );
        }

        public static CodeTypeReferenceExpression Type ( Type type )
        {
            return new CodeTypeReferenceExpression ( TypeRef ( type ) );
        }

        public static CodeTypeReference TypeRef < T > ( )
        {
            return TypeRef ( typeof ( T ) );
        }

        public static CodeTypeReference TypeRef ( Type type )
        {
            var typeReference = new CodeTypeReference ( type );
            if ( ! type.IsPrimitive )
                typeReference.Options = CodeTypeReferenceOptions.GlobalReference;

            return typeReference;
        }

        public static CodeTypeReference TypeRef ( string type, CodeTypeReferenceOptions options = CodeTypeReferenceOptions.GlobalReference )
        {
            return new CodeTypeReference ( type, options );
        }

        public static CodeTypeReferenceExpression Type ( string type, CodeTypeReferenceOptions options = CodeTypeReferenceOptions.GlobalReference )
        {
            return new CodeTypeReferenceExpression ( TypeRef ( type, options ) );
        }

        public static CodeExpression Access ( MemberAttributes accessModifiers )
        {
            return accessModifiers.HasFlag ( MemberAttributes.Static ) ? Static ( ) : This ( );
        }

        public static CodeVariableReferenceExpression Variable ( string name )
        {
            return new CodeVariableReferenceExpression ( name );
        }

        public static CodeParameterDeclarationExpression Parameter < T > ( string name )
        {
            return new CodeParameterDeclarationExpression ( typeof ( T ), name );
        }

        public static CodeFieldReferenceExpression Field ( this CodeExpression expression, string name )
        {
            return new CodeFieldReferenceExpression ( expression, name );
        }

        public static CodePropertyReferenceExpression Property ( this CodeExpression expression, string name )
        {
            return new CodePropertyReferenceExpression ( expression, name );
        }

        public static CodeMethodReferenceExpression Method ( this CodeExpression expression, string name )
        {
            return new CodeMethodReferenceExpression ( expression, name );
        }

        public static CodeAssignStatement Assign ( this CodeExpression left, CodeExpression right )
        {
            return new CodeAssignStatement ( left, right );
        }

        public static void Return ( this CodeStatementCollection statements, CodeExpression expression )
        {
            statements.Add ( new CodeMethodReturnStatement ( expression ) );
        }

        public static CodeExpression Cast ( this CodeExpression expression, CodeTypeReference type )
        {
            return new CodeCastExpression ( type, expression );
        }

        public static CodeExpression Cast ( this CodeExpression expression, string type )
        {
            return new CodeCastExpression ( TypeRef ( type ), expression );
        }

        public static CodeExpression Cast < T > ( this CodeExpression expression )
        {
            return new CodeCastExpression ( TypeRef < T > ( ), expression );
        }

        public static CodeAttributeDeclaration Attribute < T > ( ) where T : Attribute
        {
            return Attribute < T > ( Array.Empty < CodeExpression > ( ) );
        }

        public static CodeAttributeDeclaration Attribute < T > ( params object [ ] arguments ) where T : Attribute
        {
            var attribute = new CodeAttributeDeclaration ( TypeRef < T > ( ) );
            foreach ( var argument in arguments )
                attribute.Arguments.Add ( new CodeAttributeArgument ( Constant ( argument ) ) );

            return attribute;
        }

        public static CodeAttributeDeclaration Attribute < T > ( params (string, object) [ ] arguments ) where T : Attribute
        {
            var attribute = new CodeAttributeDeclaration ( TypeRef < T > ( ) );
            foreach ( var argument in arguments )
                attribute.Arguments.Add ( new CodeAttributeArgument ( argument.Item1, Constant ( argument.Item2 ) ) );

            return attribute;
        }

        public static CodeAttributeDeclaration Attribute < T > ( params CodeExpression [ ] arguments ) where T : Attribute
        {
            var attribute = new CodeAttributeDeclaration ( TypeRef < T > ( ) );
            foreach ( var argument in arguments )
                attribute.Arguments.Add ( new CodeAttributeArgument ( argument ) );

            return attribute;
        }

        public static CodeAttributeDeclaration Attribute < T > ( params (string, CodeExpression) [ ] arguments ) where T : Attribute
        {
            var attribute = new CodeAttributeDeclaration ( TypeRef < T > ( ) );
            foreach ( var argument in arguments )
                attribute.Arguments.Add ( new CodeAttributeArgument ( argument.Item1, argument.Item2 ) );

            return attribute;
        }

        public static T Attributed < T > ( this T codeTypeMember, params CodeAttributeDeclaration [ ] attributes ) where T : CodeTypeMember
        {
            codeTypeMember.CustomAttributes.AddRange ( attributes );

            return codeTypeMember;
        }

        public static int CommentSingleLineThreshold { get; set; } = 80;
        public static int CommentMaxLineLength       { get; set; } = 100;

        public static T AddComment < T > ( this T codeTypeMember, string format, params object [ ] args ) where T : CodeTypeMember
        {
            var comment = args == null || args.Length > 0 ? string.Format ( format, args ) : format;
            foreach ( var line in comment.WordWrap ( CommentMaxLineLength ) )
                codeTypeMember.Comments.Add ( new CodeCommentStatement ( line ) );

            return codeTypeMember;
        }

        public static T AddSummary < T > ( this T codeTypeMember, string format, params object [ ] args ) where T : CodeTypeMember
        {
            var comment = Format ( format, args );

            return codeTypeMember.AddTagComments ( "summary", comment, codeTypeMember is CodeMemberField && comment.Length < CommentSingleLineThreshold );
        }

        public static T AddRemarks < T > ( this T codeTypeMember, string format, params object [ ] args ) where T : CodeTypeMember
        {
            var comment = Format ( format, args );

            return codeTypeMember.AddTagComments ( "remarks", comment, comment.Length < CommentSingleLineThreshold );
        }

        public static T AddParameterComment < T > ( this T codeTypeMember, string parameter, string format, params object [ ] args ) where T : CodeTypeMember
        {
            var comment = Format ( format, args );

            return codeTypeMember.AddTagComments ( $"param name=\"{ parameter }\"", comment, comment.Length < CommentSingleLineThreshold );
        }

        public static T AddReturnComment < T > ( this T codeTypeMember, string format, params object [ ] args ) where T : CodeTypeMember
        {
            var comment = Format ( format, args );

            return codeTypeMember.AddTagComments ( "returns", comment, comment.Length < CommentSingleLineThreshold );
        }

        private static T AddTagComments < T > ( this T codeTypeMember, string tag, string comment, bool singleLine ) where T : CodeTypeMember
        {
            var endTag = tag.Split ( ' ' ) [ 0 ];

            if ( ! singleLine )
            {
                codeTypeMember.Comments.Add ( new CodeCommentStatement ( $"<{ tag }>", true ) );

                foreach ( var line in comment.WordWrap ( CommentMaxLineLength ) )
                    codeTypeMember.Comments.Add ( new CodeCommentStatement ( line, true ) );

                codeTypeMember.Comments.Add ( new CodeCommentStatement ( $"</{ endTag }>", true ) );
            }
            else
                codeTypeMember.Comments.Add ( new CodeCommentStatement ( $"<{ tag }>{ comment }</{ endTag }>", true ) );

            return codeTypeMember;
        }

        private static readonly char [ ] lineBreaks = new [ ] { '\n', '\r' };
        private static readonly char [ ] wordBreaks = new [ ] { ' ', ',', '.', '?', '!', ':', ';', '-', '\n', '\r', '\t' };

        private static string Format ( string format, params object [ ] args )
        {
            return args == null || args.Length > 0 ? string.Format ( CultureInfo.InvariantCulture, format, args ) : format;
        }

        private static IEnumerable < string > WordWrap ( this string text, int maxLineLength )
        {
            var lineBreak     = 0;
            var lastLineBreak = 0;

            do
            {
                var cut = lastLineBreak + maxLineLength;

                lineBreak = text.IndexOfAny ( lineBreaks, lastLineBreak ) + 1;

                if ( lineBreak == 0 || lineBreak > cut )
                {
                    if ( cut < text.Length )
                    {
                        lineBreak = text.LastIndexOfAny ( wordBreaks, cut ) + 1;
                        if ( lineBreak <= lastLineBreak )
                        {
                            lineBreak = text.IndexOfAny ( wordBreaks, cut ) + 1;
                            if ( lineBreak == 0 )
                                lineBreak = text.Length;
                        }
                    }
                    else
                        lineBreak = text.Length;
                }

                yield return text.Substring ( lastLineBreak, lineBreak - lastLineBreak ).Trim ( );

                lastLineBreak = lineBreak;
            }
            while ( lineBreak < text.Length );
        }

        public static CodeNamespace CreateNamespace ( string @namespace, params string [ ] imports )
        {
            var codeNamespace = new CodeNamespace ( @namespace );
            foreach ( var import in imports )
                codeNamespace.Imports.Add ( new CodeNamespaceImport ( import ) );

            return codeNamespace;
        }

        public static CodeMemberField CreateField < T > ( string name, MemberAttributes accessModifiers )
        {
            return CreateField ( typeof ( T ), name, accessModifiers );
        }

        public static CodeMemberField CreateField ( Type type, string name, MemberAttributes accessModifiers )
        {
            return new CodeMemberField ( TypeRef ( type ), name ) { Attributes = accessModifiers };
        }

        public static CodeMemberProperty CreateProperty < T > ( string name, MemberAttributes accessModifiers, bool hasSet = true )
        {
            return CreateProperty ( typeof ( T ), name, accessModifiers, hasSet );
        }

        public static CodeMemberProperty CreateProperty ( Type type, string name, MemberAttributes accessModifiers, bool hasSet = true )
        {
            return CreateProperty ( TypeRef ( type ), name, accessModifiers, hasSet );
        }

        public static CodeMemberProperty CreateProperty ( CodeTypeReference type, string name, MemberAttributes accessModifiers, bool hasSet = true )
        {
            return new CodeMemberProperty ( ) { Type = type, Name = name, Attributes = accessModifiers, HasGet = true, HasSet = hasSet };
        }

        public static CodeMemberProperty Get ( this CodeMemberProperty property, Action < CodeStatementCollection > get )
        {
            get ( property.GetStatements );

            return property;
        }

        public static CodeMemberProperty Set ( this CodeMemberProperty property, Action < CodeStatementCollection, CodePropertySetValueReferenceExpression > set )
        {
            set ( property.SetStatements, new CodePropertySetValueReferenceExpression ( ) );

            return property;
        }

        public static CodeMemberMethod CreateMethod ( Type returnType, string name, MemberAttributes accessModifiers )
        {
            return new CodeMemberMethod ( ) { Name = name, ReturnType = TypeRef ( returnType ), Attributes = accessModifiers };
        }

        public static CodeMemberField Initialize ( this CodeMemberField field, CodeExpression initialValue )
        {
            field.InitExpression = initialValue;

            return field;
        }

        public static CodePrimitiveExpression Constant ( object value )
        {
            return new CodePrimitiveExpression ( value );
        }

        public static CodeTypeOfExpression TypeOf ( this CodeTypeReference type )
        {
            return new CodeTypeOfExpression ( type );
        }

        public static CodeParameterDeclarationExpression Parameter ( this CodeTypeReference type, string name )
        {
            return new CodeParameterDeclarationExpression ( type, name );
        }

        public static CodeObjectCreateExpression Construct ( this CodeTypeReference createType, params CodeExpression [ ] parameters )
        {
            return new CodeObjectCreateExpression ( createType, parameters );
        }

        public static CodeMethodInvokeExpression Invoke ( this CodeMethodReferenceExpression method, params CodeExpression [ ] parameters )
        {
            return new CodeMethodInvokeExpression ( method, parameters );
        }

        public static bool Contains ( this CodeTypeMemberCollection members, string name )
        {
            foreach ( CodeTypeMember member in members )
                if ( member?.Name == name )
                    return true;

            return false;
        }

        public static T AddTo < T > ( this T @namespace, CodeNamespaceCollection namespaces ) where T : CodeNamespace
        {
            namespaces.Add ( @namespace );

            return @namespace;
        }

        public static T AddTo < T > ( this T type, CodeTypeDeclarationCollection types ) where T : CodeTypeDeclaration
        {
            types.Add ( type );

            return type;
        }

        public static T AddTo < T > ( this T member, CodeTypeMemberCollection members ) where T : CodeTypeMember
        {
            members.Add ( member );

            return member;
        }

        public static T AddTo < T > ( this T parameter, CodeParameterDeclarationExpressionCollection parameters ) where T : CodeParameterDeclarationExpression
        {
            parameters.Add ( parameter );

            return parameter;
        }
    }
}