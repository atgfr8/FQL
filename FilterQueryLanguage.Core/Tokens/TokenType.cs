namespace FilterQueryLanguage.Core.Tokens
{
    internal enum TokenType
    {
        Find,
        StartsWith,
        Equal,
        NotEqual,
        Contains,
        OpenParenthesis,
        CloseParanthesis,
        And,
        Or,
        StringValue,
        SequenceTerminator
    }
}