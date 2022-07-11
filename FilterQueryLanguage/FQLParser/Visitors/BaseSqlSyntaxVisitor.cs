using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FilterQueryLanguage.FQLParser.Syntax;

namespace FilterQueryLanguage.FQLParser.Visitors
{
    public abstract class BaseSqlSyntaxVisitor : BaseSyntaxVisitor
    {
        protected StringBuilder _whereClauseBuilder;

        public BaseSqlSyntaxVisitor()
        {
            _whereClauseBuilder = new StringBuilder();
        }

        public override string GetQuery(IEnumerable<SyntaxVisitorOption> options)
        {
            var tableNameSetting = options.FirstOrDefault(x => x.Name == "TableName");
            if (tableNameSetting == null)
                throw new Exception("Sql Query Requires a 'TableName' setting");
            var whereClause = GetWhereClause();
            var sqlQuery = $"SELECT * FROM {tableNameSetting.Value} WHERE {whereClause};";
            return sqlQuery;
        }

        protected string GetWhereClause()
        {
            return _whereClauseBuilder.ToString();
        }

        public override void Visit(ExpressionSyntax node)
        {
            var op = GetSqlOperator(node.Operator);
            string opStatement;
            switch (node.Operator)
            {
                case FilterQueryOperator.contains:
                    opStatement = $"like '%{node.FieldValue}%'";
                    break;
                case FilterQueryOperator.startsWith:
                    opStatement = $"like '%{node.FieldValue}'";
                    break;
                default:
                    opStatement = $"{GetSqlOperator(node.Operator)} '{node.FieldValue}'";
                    break;

            }

            _whereClauseBuilder.Append($"{GetColumnName(node.Field)} {opStatement}");
        }

        public override void Visit(StatementSyntax node)
        {
            if (!node.HasLeftStatement && !node.HasRightStatement)
            {
                Visit(node.Expression);
                return;
            }
            else if (node.HasLeftStatement && !node.HasRightStatement)
            {
                Visit(node.Expression);
                AppendOperator(node.Operator);
                Visit(node.LeftStatement);
                return;
            }
            else if (node.HasLeftStatement && node.HasRightStatement)
            {
                if (AppendParanthesisToFirstPart(node))
                {
                    _whereClauseBuilder.Append("(");
                }
                Visit(node.LeftStatement);
                if (AppendParanthesisToFirstPart(node))
                {
                    _whereClauseBuilder.Append(")");
                }
                AppendOperator(node.Operator);
                if (AppendParanthesisToSecondPart(node))
                {
                    _whereClauseBuilder.Append("(");
                }
                Visit(node.RightStatement);
                if (AppendParanthesisToSecondPart(node))
                {
                    _whereClauseBuilder.Append(")");
                }
                return;
            }
            throw new Exception("Transpiler error: invalid logical syntax detected. Check your fql query for syntax errors");
        }

        private bool AppendParanthesisToFirstPart(StatementSyntax node) => node.LeftStatement.HasLeftStatement && node.LeftStatement.HasRightStatement && node.RightStatement.HasLeftStatement;
        private bool AppendParanthesisToSecondPart(StatementSyntax node) => node.RightStatement.HasLeftStatement;

        protected virtual void AppendOperator(FilterQueryLogicalOperator? oper)
        {
            _whereClauseBuilder.Append($" {oper.ToString().ToLower()} ");
        }

        protected virtual string GetColumnName(string field)
        {
            switch (field.ToLower())
            {
                default:
                    return field;
            }
        }
        protected virtual string GetSqlOperator(FilterQueryOperator op)
        {
            switch (op)
            {
                case FilterQueryOperator.equal:
                    return "=";
                case FilterQueryOperator.notEqual:
                    return "<>";
                case FilterQueryOperator.contains:
                case FilterQueryOperator.startsWith:
                    return "like";
                case FilterQueryOperator.greaterThan:
                    return ">";
                case FilterQueryOperator.greaterThanOrEqualTo:
                    return ">=";
                case FilterQueryOperator.lessThan:
                    return "<";
                case FilterQueryOperator.lessThanOrEqualTo:
                    return "<=";
                default:
                    throw new ApplicationException("Error in converting to SQL Query, check your code");
            }
        }
    }
}