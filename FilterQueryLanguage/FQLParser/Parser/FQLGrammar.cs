using FilterQueryLanguage.FQLParser.Syntax;
using Sprache;

namespace FilterQueryLanguage.FQLParser.Parser
{
    public class FQLGrammar
    {
        public Parser<string> Keyword(string text) =>
            text == null ? Parse.Return("") : Parse.IgnoreCase(text).Then(n => Parse.Not(Parse.LetterOrDigit.Or(Parse.Char('_')))).Return(text);

        public Parser<LogicalSyntax> LogicalType =>
            from op in Keyword(FQLKeyword.And.GetString()).Or(Keyword(FQLKeyword.Or.GetString()))
            select new LogicalSyntax
            {
                Value = FilterQueryLogicalOperatorHelper.GetOperator(op)
            };

        public Parser<StatementSyntax> SubExpression =>
            from field in Parse.Letter.Until(Parse.WhiteSpace.Once()).Text()
            from op in OperatorType
            from fieldValue in FieldValue
            select new StatementSyntax
            {
                Expression = new ExpressionSyntax
                {
                    Operator = FQLQueryOperatorHelper.GetFilterQueryOperator(op),
                    Field = field,
                    FieldValue = fieldValue
                }
            };

        public Parser<string> OperatorType =>
            Keyword(FQLKeyword.NotEqual.GetString()).Or(
            Keyword(FQLKeyword.Equal.GetString())).Or(
            Keyword(FQLKeyword.GreaterThan.GetString())).Or(
            Keyword(FQLKeyword.GreaterThanOrEqualTo.GetString())).Or(
            Keyword(FQLKeyword.LessThan.GetString())).Or(
            Keyword(FQLKeyword.LessThanOrEqualTo.GetString())).Or(
            Keyword(FQLKeyword.Contains.GetString())).Or(
            Keyword(FQLKeyword.StartsWith.GetString()));

        public Parser<string> FieldValue =>
            from first in Parse.Char('\'').Token()
            from rest in Parse.LetterOrDigit.Many().Text()
            from end in Parse.Char('\'').Token()
            select rest;

        public Parser<StatementSyntax> SyntaxTree =>
            Parse.ChainOperator(LogicalType, Statement, (op, left, right) =>
            {
                return new StatementSyntax
                {
                    Operator = op.Value,
                    LeftStatement = left,
                    RightStatement = right
                };
            });

        public Parser<StatementSyntax> Statement =>
            from lparen in Parse.Char('(').Token()
            from exp in ExpressionOrStatement.Optional()
            from rParen in Parse.Char(')').Token()
            select exp.GetOrElse(new StatementSyntax());

        public Parser<StatementSyntax> ExpressionOrStatement =>
           from sun in SubExpression.Or(SyntaxTree)
           select sun;

    }
}