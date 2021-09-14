namespace DateTimeCompiler.Lexer.Tokens
{
    public class Token
    {
        public TokenType TokenType { get; set; }

        public string Lexeme { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public override string ToString()
        {
            return $"{Lexeme} Type: {TokenType}  Line: {Line} Column: {Column}";
        }

        public static bool operator ==(Token a, Token b) => a.TokenType == b.TokenType;
        public static bool operator !=(Token a, Token b) => a.TokenType != b.TokenType;
    }
}
