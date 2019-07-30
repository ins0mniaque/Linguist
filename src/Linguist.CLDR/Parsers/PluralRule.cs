using System;
using System.Collections.Generic;
using System.Linq;

namespace Linguist.CLDR
{
    public class PluralRule
    {
        private PluralRule ( PluralForm pluralForm, Expression rule, string ruleCode, string [ ] integerSamples, string [ ] decimalSamples )
        {
            PluralForm     = pluralForm;
            Rule           = rule;
            RuleCode       = ruleCode;
            IntegerSamples = integerSamples;
            DecimalSamples = decimalSamples;
        }

        public PluralForm PluralForm     { get; }
        public Expression Rule           { get; }
        public string     RuleCode       { get; }
        public string [ ] IntegerSamples { get; }
        public string [ ] DecimalSamples { get; }

        public static PluralRule Parse ( string count, string rule, out string [ ] operands )
        {
            var pluralForm     = PluralFormParser.Parse ( count );
            var parts          = rule.Split ( '@' );
            var ruleCode       = parts [ 0 ].Trim ( );
            var integerSamples = parts.Length > 1 ? ParseSamples ( "integer", parts [ 1 ] ) : null;
            var decimalSamples = parts.Length > 2 ? ParseSamples ( "decimal", parts [ 2 ] ) : null;
            var expression     = ParseExpression ( ruleCode, out operands );

            return new PluralRule ( pluralForm, expression, ruleCode, integerSamples, decimalSamples );
        }

        private static string [ ] ParseSamples ( string name, string samples )
        {
            if ( samples.StartsWith ( name ) )
                return samples.Substring ( name.Length )
                              .Split     ( ',' )
                              .Select    ( sample => sample.Trim ( ) )
                              .ToArray   ( );

            return null;
        }

        private static Expression ParseExpression ( string ruleCode, out string [ ] operands )
        {
            if ( string.IsNullOrEmpty ( ruleCode ) )
            {
                operands = null;
                return null;
            }

            var rule      = (Expression) null;
            var variables = new HashSet < string > ( );

            foreach ( var or in Split ( ruleCode, " or " ) )
            {
                var subRule = (Expression) null;

                foreach ( var and in Split ( or, " and " ) )
                {
                    var condition = ParseSubExpression ( and, out var variable );

                    variables.Add ( variable.Variable );

                    if ( subRule == null )
                        subRule = condition;
                    else
                        subRule = new BinaryExpression ( subRule, "&&", condition );
                }

                if ( rule == null )
                    rule = subRule;
                else
                    rule = new BinaryExpression ( rule, "||", subRule );
            }

            operands = variables.ToArray ( );

            while ( rule is ParenthesisExpression parenthesis )
                rule = parenthesis.Expression;

            return rule;
        }

        private static Expression ParseSubExpression ( string rule, out VariableExpression variable )
        {
            var comparison = "!=";
            var leftRight  = Split ( rule, "!=" );
            if ( leftRight.Length == 1 )
            {
                comparison = "==";
                leftRight = Split ( rule, "=" );
            }

            if ( leftRight.Length != 2 )
                throw new FormatException ( "Invalid plural rule" );

            var modulo   = leftRight [ 0 ].Split ( '%' );
                variable = new VariableExpression ( modulo [ 0 ] );
            var left     = modulo.Length == 2 ? new BinaryExpression ( variable, "%", new NumberExpression ( modulo [ 1 ] ) ) :
                           modulo.Length == 1 ? (Expression) variable :
                                                throw new FormatException ( "Invalid plural rule" );

            var csv = leftRight [ 1 ].Split ( ',' );

            if ( csv.Length > 1 || csv [ 0 ].Contains ( ".." ) )
            {
                var ranges = csv.Where      ( value => value.Contains ( ".." ) )
                                .SelectMany ( range => Split ( range, ".." ) )
                                .Select     ( number => new NumberExpression ( number ) )
                                .ToArray    ( );
                var values = csv.Where   ( value => ! value.Contains ( ".." ) )
                                .Select  ( number => new NumberExpression ( number ) )
                                .ToArray ( );

                var between = ranges.Length == 0 ? null : new MethodExpression ( left, "between", ranges );
                var equals  = values.Length == 1 ? (Expression) new BinaryExpression ( left, "==", values [ 0 ] ) :
                              values.Length == 0 ? null : new MethodExpression ( left, "equals",  values );

                var expression = (Expression) null;

                if ( between != null && equals != null )
                    expression = new ParenthesisExpression ( new BinaryExpression ( between, "||", equals ) );
                else
                    expression = between ?? equals;

                if ( comparison == "!=" )
                    expression = new NotExpression ( expression );

                return expression;
            }

            return new BinaryExpression ( left, comparison, new NumberExpression ( leftRight [ 1 ] ) );
        }

        private static string [ ] Split ( string text, string separator )
        {
            return text.Replace ( separator, "\0" ).Split ( '\0' );
        }

        public override string ToString ( ) => RuleCode;

        public abstract class Expression { }
        public abstract class UnitExpression : Expression { }

        public class NumberExpression : UnitExpression
        {
            public NumberExpression ( string number )
            {
                Number = number.Trim ( );
            }

            public string Number { get; }

            public override string ToString ( ) => $"{ Number }m";
        }

        public class VariableExpression : UnitExpression
        {
            public VariableExpression ( string variable )
            {
                Variable = variable.Trim ( );
            }

            public string Variable { get; }

            public override string ToString ( ) => $"{ Variable }";
        }

        public class NotExpression : Expression
        {
            public NotExpression ( Expression expression )
            {
                Expression = expression as UnitExpression ?? new ParenthesisExpression ( expression );
            }

            public UnitExpression Expression { get; }

            public override string ToString ( ) => $"! { Expression }";
        }

        public class ParenthesisExpression : UnitExpression
        {
            public ParenthesisExpression ( Expression expression )
            {
                Expression = expression;
            }

            public Expression Expression { get; }

            public override string ToString ( ) => $"( { Expression } )";
        }

        public class BinaryExpression : Expression
        {
            public BinaryExpression ( Expression left, string @operator, Expression right )
            {
                Left     = left;
                Operator = @operator.Trim ( );
                Right    = right;
            }

            public Expression Left     { get; }
            public string     Operator { get; }
            public Expression Right    { get; }

            public override string ToString ( ) => $"{ Left } { Operator } { Right }";
        }

        public class MethodExpression : UnitExpression
        {
            public MethodExpression ( Expression target, string method, params Expression [ ] arguments )
            {
                Target    = target as UnitExpression ?? new ParenthesisExpression ( target );
                Method    = method.Trim ( );
                Arguments = arguments;
            }

            public UnitExpression Target    { get; }
            public string         Method    { get; }
            public Expression [ ] Arguments { get; }

            public override string ToString ( ) => $"{ Target }.{ Method } ( { string.Join < Expression > ( ", ", Arguments ) } )";
        }
    }
}