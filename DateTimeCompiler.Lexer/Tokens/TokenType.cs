namespace DateTimeCompiler.Lexer.Tokens
{
    public enum TokenType
    { 
        Basic,
        Number,
        Slash,
        Plus,
        Substract,
        Equal,
        Assignation,
        YearKeyword,
        MonthKeyword,
        DayKeyword,
        HourKeyword,
        MinuteKeyword,
        SecondKeyword,
        VarKeyword,
        Colon,
        Variable,
        EOF
    }
}