using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FilterQueryLanguage.Core.Models;
using FilterQueryLanguage.Core;
using Humanizer;

namespace FilterQueryLanguage.QueryBuilders
{
    public class ESQueryBuilder : IQueryBuilder
    {
        public string Name => "ElasticSearch";

        public string GetQuery(QueryBuilderConfig config)
        {
            //first get the root of the tree
            var root = config.Query.GetRoot();
            //simple case is, its only one query without a logical operator
            if (root.PredicateNode)
            {
                var q = GetPredicateQuery(root.Predicate);
                return q;
            }

            //second simple case both left and right nodes are predicates
            if (root.Left.PredicateNode && root.Right.PredicateNode)
            {
                return GetLogicPredicateQuery(root);
            }
            //now we begin the fun part

            var left = TraverseTreeBuildingQuery(root.Left);
            var right = TraverseTreeBuildingQuery(root.Right);
            var logicQueryStart = GetQueryForLogic(root.Operator.Value);
            if (right.Count() == 1 && left.Count() == 1)
            {
                return $"{logicQueryStart}{left.Pop()},{right.Pop()}]}}}}";
            }

            throw new ApplicationException("Error in generating query");
        }

        //this method assumes a shallow tree and not complex logics
        private Stack<String> TraverseTreeBuildingQuery(QueryNode node)
        {
            var queryStack = new Stack<string>();
            if (node.LogicalOperatorNode && node.Left.PredicateNode && node.Right.PredicateNode)
            {
                queryStack.Push(GetLogicPredicateQuery(node));
                return queryStack;
            }

            if (node.LogicalOperatorNode && node.Left.LogicalOperatorNode)
            {
                var stack = TraverseTreeBuildingQuery(node.Left);
                foreach (var item in stack)
                {
                    queryStack.Push(item);
                }
            }

            if (node.LogicalOperatorNode && node.Right.LogicalOperatorNode)
            {
                var stack = TraverseTreeBuildingQuery(node.Left);
                foreach (var item in stack)
                {
                    queryStack.Push(item);
                }
            }

            if (node.PredicateNode)
            {
                queryStack.Push(GetPredicateQuery(node.Predicate));
            }

            return queryStack;
        }

        private string GetLogicPredicateQuery(QueryNode node)
        {
            var leftPredicateQuery = GetPredicateQuery(node.Left.Predicate);
            var rightPredicateQuery = GetPredicateQuery(node.Right.Predicate);
            var logicQueryStart = GetQueryForLogic(node.Operator.Value);
            return $"{logicQueryStart}{leftPredicateQuery},{rightPredicateQuery}]}}}}";
        }



        private string GetPredicateQuery(PredicateModel predicate)
        {
            if (predicate.Operator == FilterQueryOperator.equal)
            {
                return GetEqualQuery(predicate);
            }
            if (predicate.Operator == FilterQueryOperator.notEqual)
            {
                return GetNotEqualQuery(predicate);
            }
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            stringBuilder.Append($"\"{GetESOperator(predicate.Operator)}\" : {{");
            stringBuilder.Append($"\"{GetField(predicate.Field)}\" : \"{GetString(predicate.Value)}\" }}");
            stringBuilder.Append("}");
            var predicateQuery = stringBuilder.ToString();
            return predicateQuery;
        }

        private string GetEqualQuery(PredicateModel predicate)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("{ \"match\" : {");
            stringBuilder.Append($"\"{GetField(predicate.Field)}\" : {{");
            stringBuilder.Append($"\"query\" : \"{predicate.Value}\",");
            stringBuilder.Append($"\"operator\" : \"and\"");
            stringBuilder.Append("}}}");
            return stringBuilder.ToString();
        }

        private string GetNotEqualQuery(PredicateModel predicate)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{{\"bool\": {{");
            stringBuilder.Append("\"must_not\" : [");
            stringBuilder.Append("{ \"match\" : {");
            stringBuilder.Append($" \"{GetField(predicate.Field)}\" : {{");
            stringBuilder.Append($"\"query\" : \"{predicate.Value}\",");
            stringBuilder.Append($"\"operator\" : \"and\"");
            stringBuilder.Append("}}}]}}");
            var query = stringBuilder.ToString();
            return query;
        }

        private string GetQueryForLogic(FilterQueryLogicalOperator Operator)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{{\"bool\": {{");
            switch (Operator)
            {
                case FilterQueryLogicalOperator.and:
                    stringBuilder.Append("\"must\": [");
                    break;
                case FilterQueryLogicalOperator.or:
                    break;
            }

            return stringBuilder.ToString();
        }

        private string GetField(string str) => GetString(str).Camelize();
        public string GetString(string str)
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
                default:
                    throw new ApplicationException("Error in converting to ES Query, check your code");
            }
        }
    }
}