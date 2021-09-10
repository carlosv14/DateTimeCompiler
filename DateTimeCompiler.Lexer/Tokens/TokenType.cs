namespace DateTimeCompiler.Lexer
{
    public enum TokenType
    { 
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