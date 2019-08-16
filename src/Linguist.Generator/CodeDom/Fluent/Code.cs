using System;
using System.CodeDom;

namespace Linguist.CodeDom.Fluent
{
    /// <summary>
    /// Fluent code builder to deal with CodeDom's verbosity
    /// </summary>
    public static class Code
    {
        public static CodeTypeReference Type < T > ( )
        {
            return Type ( typeof ( T ) );
        }

        public static CodeTypeReference Type ( Type type )
        {
            var typeReference = new CodeTypeReference ( type );
            if ( ! type.IsPrimitive )
                typeReference.Options = CodeTypeReferenceOptions.GlobalReference;

            return typeReference;
        }

        public static CodeTypeReference Type ( string type, params CodeTypeReference [ ] typeArguments )
        {
            return new CodeTypeReference ( type, typeArguments ) { Options = CodeTypeReferenceOptions.GlobalReference };
        }

        public static CodeTypeReference NestedType ( string type, string nestedType, params CodeTypeReference [ ] typeArguments )
        {
            return Type ( type + "+" + nestedType, typeArguments );
        }

        public static CodeTypeReference Local ( this CodeTypeReference type )
        {
            type.Options &= ~CodeTypeReferenceOptions.GlobalReference;

            return type;
        }

        public static CodeTypeReference Generic ( this CodeTypeReference type )
        {
            type.Options |= CodeTypeReferenceOptions.GenericTypeParameter;

            return type;
        }

        public static CodeThisReferenceExpression This ( )
        {
            return new CodeThisReferenceExpression ( );
        }

        public static CodeExpression Static ( )
        {
            return null;
        }

        public static CodeTypeReferenceExpression Static ( this CodeTypeReference type )
        {
            return new CodeTypeReferenceExpression ( type );
        }

        public static CodeExpression Instance ( this CodeTypeDeclaration type )
        {
            return Instance ( type.Attributes );
        }

        public static CodeExpression Instance ( MemberAttributes accessModifiers )
        {
            return accessModifiers.HasBitMask ( MemberAttributes.Static ) ? Static ( ) : This ( );
        }

        public static CodeTypeOfExpression TypeOf ( this CodeTypeReference type )
        {
            return new CodeTypeOfExpression ( type );
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

        public static CodeEventReferenceExpression Event ( this CodeExpression expression, string name )
        {
            return new CodeEventReferenceExpression ( expression, name );
        }

        public static CodeMethodReferenceExpression Method ( this CodeExpression expression, string name )
        {
            return new CodeMethodReferenceExpression ( expression, name );
        }

        public static CodeIndexerExpression Indexer ( this CodeExpression expression, params CodeExpression [ ] parameters )
        {
            return new CodeIndexerExpression ( expression, parameters );
        }

        public static CodeArrayIndexerExpression ArrayIndex ( this CodeExpression expression, params CodeExpression [ ] indices )
        {
            return new CodeArrayIndexerExpression ( expression, indices );
        }

        public static CodeAssignStatement Assign ( this CodeExpression left, CodeExpression right )
        {
            return new CodeAssignStatement ( left, right );
        }

        public static CodeMethodReturnStatement Return ( )
        {
            return new CodeMethodReturnStatement ( );
        }

        public static CodeMethodReturnStatement Return ( CodeExpression expression )
        {
            return new CodeMethodReturnStatement ( expression );
        }

        public static void Return ( this CodeStatementCollection statements, CodeExpression expression )
        {
            statements.Add ( Return ( expression ) );
        }

        public static CodeThrowExceptionStatement Throw < T > ( params CodeExpression [ ] parameters ) where T : Exception
        {
            return new CodeThrowExceptionStatement ( Type < T > ( ).Construct ( parameters ) );
        }

        public static void Throw < T > ( this CodeStatementCollection statements, params CodeExpression [ ] parameters ) where T : Exception
        {
            statements.Add ( Throw < T > ( parameters ) );
        }

        public static CodeExpression Cast ( this CodeExpression expression, CodeTypeReference type )
        {
            return new CodeCastExpression ( type, expression );
        }

        public static CodeExpression Cast < T > ( this CodeExpression expression )
        {
            return new CodeCastExpression ( Type < T > ( ), expression );
        }

        public static CodePrimitiveExpression Null => Constant ( null );

        public static CodePrimitiveExpression Constant ( object value )
        {
            return new CodePrimitiveExpression ( value );
        }

        public static CodeFieldReferenceExpression Constant < TEnum > ( TEnum value ) where TEnum : Enum
        {
            return Type < TEnum > ( ).Static ( ).Field ( value.ToString ( ) );
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

        public static CodeDelegateInvokeExpression InvokeDelegate ( this CodeExpression @delegate, params CodeExpression [ ] parameters )
        {
            return new CodeDelegateInvokeExpression ( @delegate, parameters );
        }

        public static CodeConditionStatement If ( CodeExpression expression )
        {
            return new CodeConditionStatement ( expression );
        }

        public static CodeConditionStatement Then ( this CodeConditionStatement condition, params CodeStatement [ ] codeStatements )
        {
            condition.TrueStatements.AddRange ( codeStatements );

            return condition;
        }

        public static CodeConditionStatement Else ( this CodeConditionStatement condition, params CodeStatement [ ] codeStatements )
        {
            condition.FalseStatements.AddRange ( codeStatements );

            return condition;
        }

        public static CodeBinaryOperatorExpression And ( this CodeExpression left, CodeExpression right )
        {
            return new CodeBinaryOperatorExpression ( left, CodeBinaryOperatorType.BooleanAnd, right );
        }

        public static CodeBinaryOperatorExpression Or ( this CodeExpression left, CodeExpression right )
        {
            return new CodeBinaryOperatorExpression ( left, CodeBinaryOperatorType.BooleanOr, right );
        }

        public static CodeBinaryOperatorExpression ValueEquals ( this CodeExpression left, CodeExpression right )
        {
            return new CodeBinaryOperatorExpression ( left, CodeBinaryOperatorType.ValueEquality, right );
        }

        public static CodeBinaryOperatorExpression ReferenceEquals ( this CodeExpression left, CodeExpression right )
        {
            return new CodeBinaryOperatorExpression ( left, CodeBinaryOperatorType.IdentityEquality, right );
        }

        public static CodeBinaryOperatorExpression ReferenceNotEquals ( this CodeExpression left, CodeExpression right )
        {
            return new CodeBinaryOperatorExpression ( left, CodeBinaryOperatorType.IdentityInequality, right );
        }

        public static CodeMethodInvokeExpression ObjectEquals ( this CodeExpression left, CodeExpression right )
        {
            return Type < object > ( ).Static ( ).Method ( nameof ( object.Equals ) ).Invoke ( left, right );
        }

        public static CodeBinaryOperatorExpression IsLessThan ( this CodeExpression left, CodeExpression right )
        {
            return new CodeBinaryOperatorExpression ( left, CodeBinaryOperatorType.LessThan, right );
        }

        public static CodeBinaryOperatorExpression IsLessThanOrEquals ( this CodeExpression left, CodeExpression right )
        {
            return new CodeBinaryOperatorExpression ( left, CodeBinaryOperatorType.LessThanOrEqual, right );
        }

        public static CodeBinaryOperatorExpression IsGreaterThan ( this CodeExpression left, CodeExpression right )
        {
            return new CodeBinaryOperatorExpression ( left, CodeBinaryOperatorType.GreaterThan, right );
        }

        public static CodeBinaryOperatorExpression IsGreaterThanOrEquals ( this CodeExpression left, CodeExpression right )
        {
            return new CodeBinaryOperatorExpression ( left, CodeBinaryOperatorType.GreaterThanOrEqual, right );
        }
    }
}