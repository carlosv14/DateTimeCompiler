using System;
using System.Collections.Generic;
using DateTimeCompiler.Lexer.Tokens;

namespace DateTimeCompiler.Core.Expressions
{
    public class BinaryExpression: Expression
    {
        private readonly Dictionary<(Type, Type), Type> _typeRules;
        public Expression LeftExpression { get; }
        public Expression RightExpression { get; }

        public BinaryExpression(Token token, Expression leftExpression, Expression rightExpression) 
            : base(token, null)
        {
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
            _typeRules = new Dictionary<(Type, Type), Type>
            {
                {(Type.Date, Type.Year), Type.Date},
                {(Type.Date, Type.Month), Type.Date},
                {(Type.Date, Type.Day), Type.Date},
                //-----------------------------
                {(Type.TimeStamp, Type.TimeStamp), Type.TimeStamp},
                {(Type.TimeStamp, Type.Hour), Type.TimeStamp},
                {(Type.TimeStamp, Type.Minute), Type.TimeStamp},
                {(Type.TimeStamp, Type.Second), Type.TimeStamp},
            };
        }

        public override dynamic Evaluate()
        {
            if (Token.TokenType== TokenType.Plus)
            {
                //TimeStamp + Something
                if (RightExpression.GetExpressionType()==Type.Hour)
                {
                    var leftExpressionDate = LeftExpression.Evaluate() is DateTime ? (DateTime)LeftExpression.Evaluate() : default;
                    var rightExpression = RightExpression.Evaluate();
                    return leftExpressionDate.AddHours(rightExpression);
                }
                else if (RightExpression.GetExpressionType() == Type.Minute)
                {
                    var leftExpressionDate = LeftExpression.Evaluate() is DateTime ? (DateTime)LeftExpression.Evaluate() : default;
                    var rightExpression = RightExpression.Evaluate();
                    return leftExpressionDate.AddMinutes(rightExpression);
                }
                else if (RightExpression.GetExpressionType() == Type.Second)
                {
                    var leftExpressionDate = LeftExpression.Evaluate() is DateTime ? (DateTime)LeftExpression.Evaluate() : default;
                    var rightExpression = RightExpression.Evaluate();
                    return leftExpressionDate.AddSeconds(rightExpression);
                }
                //DateTime + Something
                if (RightExpression.GetExpressionType() == Type.Year)
                {
                    var leftExpressionDate = LeftExpression.Evaluate() is DateTime ? (DateTime)LeftExpression.Evaluate() : default;
                    var rightExpression = RightExpression.Evaluate();
                    return leftExpressionDate.AddYears(rightExpression);
                }
                else if (RightExpression.GetExpressionType() == Type.Month)
                {
                    var leftExpressionDate = LeftExpression.Evaluate() is DateTime ? (DateTime)LeftExpression.Evaluate() : default;
                    var rightExpression = RightExpression.Evaluate();
                    return leftExpressionDate.AddMonths(rightExpression);
                }
                else if (RightExpression.GetExpressionType() == Type.Day)
                {
                    var leftExpressionDate = LeftExpression.Evaluate() is DateTime ? (DateTime)LeftExpression.Evaluate() : default;
                    var rightExpression = RightExpression.Evaluate();
                    return leftExpressionDate.AddDays(rightExpression);
                }

                return LeftExpression.Evaluate() + RightExpression.Evaluate();
            }
            else if(Token.TokenType == TokenType.Substract)
            {
                //TimeStamp + Something
                if (RightExpression.GetExpressionType() == Type.Hour)
                {
                    var leftExpressionDate = LeftExpression.Evaluate() is DateTime ? (DateTime)LeftExpression.Evaluate() : default;
                    var rightExpression = RightExpression.Evaluate();
                    return leftExpressionDate.AddHours(rightExpression * -1);
                }
                else if (RightExpression.GetExpressionType() == Type.Minute)
                {
                    var leftExpressionDate = LeftExpression.Evaluate() is DateTime ? (DateTime)LeftExpression.Evaluate() : default;
                    var rightExpression = RightExpression.Evaluate();
                    return leftExpressionDate.AddMinutes(rightExpression * -1);
                }
                else if (RightExpression.GetExpressionType() == Type.Second)
                {
                    var leftExpressionDate = LeftExpression.Evaluate() is DateTime ? (DateTime)LeftExpression.Evaluate() : default;
                    var rightExpression = RightExpression.Evaluate();
                    return leftExpressionDate.AddSeconds(rightExpression * -1);
                }
                //DateTime + Something
                if (RightExpression.GetExpressionType() == Type.Year)
                {
                    var leftExpressionDate = LeftExpression.Evaluate() is DateTime ? (DateTime)LeftExpression.Evaluate() : default;
                    var rightExpression = RightExpression.Evaluate();
                    return leftExpressionDate.AddYears(rightExpression * -1);
                }
                else if (RightExpression.GetExpressionType() == Type.Month)
                {
                    var leftExpressionDate = LeftExpression.Evaluate() is DateTime ? (DateTime)LeftExpression.Evaluate() : default;
                    var rightExpression = RightExpression.Evaluate();
                    return leftExpressionDate.AddMonths(rightExpression * -1);
                }
                else if (RightExpression.GetExpressionType() == Type.Day)
                {
                    var leftExpressionDate = LeftExpression.Evaluate() is DateTime ? (DateTime)LeftExpression.Evaluate() : default;
                    var rightExpression = RightExpression.Evaluate();
                    return leftExpressionDate.AddDays(rightExpression * -1);
                }
                return LeftExpression.Evaluate() - RightExpression.Evaluate();
            }
            throw new ApplicationException($"Cannot perform Binary operation on {LeftExpression.GetExpressionType()}, {RightExpression.GetExpressionType()}");
        }

        public override Type GetExpressionType()
        {
            if (_typeRules.TryGetValue((LeftExpression.GetExpressionType(), RightExpression.GetExpressionType()), out var resultType))
            {
                return resultType;
            }
            throw new ApplicationException($"Cannot perform Binary operation on {LeftExpression.GetExpressionType()}, {RightExpression.GetExpressionType()}");
        }
    }
}