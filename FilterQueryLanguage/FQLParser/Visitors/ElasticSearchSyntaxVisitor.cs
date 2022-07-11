using System;
using System.Collections.Generic;
using System.Text;
using FilterQueryLanguage.FQLParser.Syntax;
using Humanizer;

namespace FilterQueryLanguage.FQLParser.Visitors
{
    /*
    Warning, a lot of this code comes from the origional ES query builder that did work at a time in the past. 
    It is not tested and maybe incorrect or it may work as it should.
    Hopefully this structure will make it easier to fix or extend in the future should the prior prove to.
    */

    public class ElasticSearchSyntaxVisitor : BaseSyntaxVisitor
    {
        private StringBuilder _stringBuilder;

        public ElasticSearchSyntaxVisitor()
        {
            _stringBuilder = new StringBuilder();
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
                Visit(node.LeftStatement);
                return;
            }
            else if (node.HasLeftStatement && node.HasRightStatement)
            {
                Visit(node.LeftStatement);
                Visit(node.RightStatement);
                return;
            }
            throw new Exception("Transpiler error: invalid logical syntax detected. Check your fql query for syntax errors");
        }


        public override void Visit(ExpressionSyntax node)
        {
            _stringBuilder.Append(GetPredicateQuery(node));
        }


        private string GetPredicateQuery(ExpressionSyntax predicate)
        {
            if (predicate.Operator == FilterQueryOperator.equal)
            {
                return GetEqualQuery(predicate);
            }
            if (predicate.Operator == FilterQueryOperator.notEqual)
            {
                return GetNotEqualQuery(predicate);
            }
            if(predicate.OperatorIsGreaterThanLessThanEqualTo)
            {
                return GetRangeQuery(predicate);
            }
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append($"\"{GetESOperator(predicate.Operator)}\" : {{");
            stringBuilder.Append($"\"{GetField(predicate.Field)}\" : \"{GetString(predicate.FieldValue)}\" }}");
            stringBuilder.Append("}");
            var predicateQuery = stringBuilder.ToString();
            return predicateQuery;
        }
        private string GetEqualQuery(ExpressionSyntax expression)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("{ \"match\" : {");
            stringBuilder.Append($"\"{GetField(expression.Field)}\" : {{");
            stringBuilder.Append($"\"query\" : \"{expression.FieldValue}\",");
            stringBuilder.Append($"\"operator\" : \"and\"");
            stringBuilder.Append("}}}");
            return stringBuilder.ToString();
        }

        private string GetNotEqualQuery(ExpressionSyntax expression)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{{\"bool\": {{");
            stringBuilder.Append("\"must_not\" : [");
            stringBuilder.Append("{ \"match\" : {");
            stringBuilder.Append($" \"{GetField(expression.Field)}\" : {{");
            stringBuilder.Append($"\"query\" : \"{expression.FieldValue}\",");
            stringBuilder.Append($"\"operator\" : \"and\"");
            stringBuilder.Append("}}}]}}");
            var query = stringBuilder.ToString();
            return query;
        }

        private string GetRangeQuery(ExpressionSyntax expression)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("{ \"bool\" : {");
            stringBuilder.Append($"\"{GetField(expression.Field)}\" : {{");
            stringBuilder.Append($"\"range\" : \"{expression.FieldValue}\",");
            stringBuilder.Append($"\"{GetESOperator(expression.Operator)}\" : \"and\"");
            stringBuilder.Append("}}}");
            return stringBuilder.ToString();
        }

        private string GetField(string str) => GetString(str).Camelize();
        private string GetString(string str)
        {
            return str.Replace('\'', ' ').Trim();
        }

        private string GetESOperator(FilterQueryOperator op)
        {
            switch (op)
            {
                case FilterQueryOperator.contains:
                    return "match";
                case FilterQueryOperator.startsWith:
                    return "match_phrase_prefix";
                case FilterQueryOperator.greaterThan:
                    return "gt";
                case FilterQueryOperator.greaterThanOrEqualTo:
                    return "gte";
                case FilterQueryOperator.lessThan:
                    return "lt";
                case FilterQueryOperator.lessThanOrEqualTo:
                    return "lte";
                default:
                    throw new ApplicationException("Error in converting to ES Query, check your code");
            }
        }

        public override string GetQuery(IEnumerable<SyntaxVisitorOption> options)
        {
            return _stringBuilder.ToString();
        }
    }
}