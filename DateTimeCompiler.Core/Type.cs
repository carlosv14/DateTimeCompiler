using System;
using DateTimeCompiler.Lexer;
using DateTimeCompiler.Lexer.Tokens;

namespace DateTimeCompiler.Core
{
    public class Type : IEquatable<Type>
    {
        public string Lexeme { get; private set; }

        public TokenType TokenType { get; private set; }
        public Type(string lexeme, TokenType tokenType)
        {
            Lexeme = lexeme;
            TokenType = tokenType;
        }

        public static Type Date => new Type("Date", TokenType.Basic);
        public static Type TimeStamp => new Type("TimeStamp", TokenType.Basic);
        public static Type Day => new Type("Day", TokenType.Basic);
        public static Type Month => new Type("Month", TokenType.Basic);
        public static Type Year => new Type("Year", TokenType.Basic);
        public static Type Hour => new Type("Hour", TokenType.Basic);
        public static Type Minute => new Type("Minute", TokenType.Basic);
        public static Type Second => new Type("Second", TokenType.Basic);

        public bool Equals(Type other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Lexeme == other.Lexeme && TokenType == other.TokenType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Type)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Lexeme, (int)TokenType);
        }

        public static bool operator ==(Type a, Type b) => a.Equals(b);

        public static bool operator !=(Type a, Type b) => !a.Equals(b);

        public override string ToString()
        {
            return Lexeme;
        }
    }
}