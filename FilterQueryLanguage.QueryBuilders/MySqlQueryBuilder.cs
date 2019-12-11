using System;
using System.Collections.Generic;
using System.Linq;
using FilterQueryLanguage.Core.Models;
using FilterQueryLanguage.Core;

namespace FilterQueryLanguage.QueryBuilders
{
    public class MySqlQueryBuilder : IQueryBuilder
    {
        public string Name => "MySql";

        public string GetQuery(QueryBuilderConfig config)
        {
            var tableName = config.QueryBuilderSettings.FirstOrDefault(x => x.Name == "TableName");
            var whereClause = GetWhereClause(config);
            var sqlQuery = $"SELECT * FROM {tableName.Value} WHERE {whereClause};";
            return sqlQuery;
        }

        private string GetWhereClause(QueryBuilderConfig config)
        {
            var entityName = config.QueryBuilderSettings.FirstOrDefault(x => x.Name == "EntityName").Value;
            var root = config.Query.GetRoot();
            if (root.PredicateNode)
            {
                return GetPredicateQuery(root.Predicate, entityName);
            }

            if (root.Left.PredicateNode && root.Right.PredicateNode)
            {
                return GetLogicPredicateQuery(root, entityName);
            }

            var left = TraverseTreeBuildingQuery(root.Left, entityName);
            var right = TraverseTreeBuildingQuery(root.Right, entityName);
            if (right.Count() == 1 && left.Count() == 1)
            {
                return $"{left.Pop()} {GetLogicOperator(root.Operator.Value)} {right.Pop()}";
            }

            throw new ApplicationException("Error in generating query");
        }

        private string GetLogicPredicateQuery(QueryNode node, string entityName)
        {
            return $"({GetPredicateQuery(node.Left.Predicate, entityName)} {GetLogicOperator(node.Operator.Value)} {GetPredicateQuery(node.Right.Predicate, entityName)})";
        }

        private string GetPredicateQuery(PredicateModel predicate, string entityName)
        {
            var columnName = GetColumnName(RemoveApostrophes(predicate.Field), entityName);
            switch (predicate.Operator)
            {
                case FilterQueryOperator.startsWith:
                    return GetStartsWithQuery(predicate, columnName);
                case FilterQueryOperator.contains:
                    return GetContainsQuery(predicate, columnName);
                default:
                    return $"{columnName} {GetSqlOperator(predicate.Operator)} {predicate.Value}";
            }
        }

        private string GetStartsWithQuery(PredicateModel predicate, string columnName)
        {
            var value = RemoveApostrophes(predicate.Value);
            var operatorAndValue = $"{GetSqlOperator(predicate.Operator)}'{value}%'";
            return $"{columnName} {operatorAndValue}";
        }

        private string GetContainsQuery(PredicateModel predicate, string columnName)
        {
            var value = RemoveApostrophes(predicate.Value);
            var operatorAndValue = $"{GetSqlOperator(predicate.Operator)}'%{value}%'";
            return $"{columnName} {operatorAndValue}";
        }

        private Stack<String> TraverseTreeBuildingQuery(QueryNode node, string entityName)
        {
            var queryStack = new Stack<string>();
            if (node.LogicalOperatorNode && node.Left.PredicateNode && node.Right.PredicateNode)
            {
                queryStack.Push(GetLogicPredicateQuery(node, entityName));
            }


            if (node.LogicalOperatorNode && node.Left.LogicalOperatorNode)
            {
                var stack = TraverseTreeBuildingQuery(node.Left, entityName);
                foreach (var item in stack)
                {
                    queryStack.Push(item);
                }
            }

            if (node.LogicalOperatorNode && node.Right.LogicalOperatorNode)
            {
                var stack = TraverseTreeBuildingQuery(node.Left, entityName);
                foreach (var item in stack)
                {
                    queryStack.Push(item);
                }
            }

            if (node.PredicateNode)
            {
                queryStack.Push(GetPredicateQuery(node.Predicate, entityName));
            }

            return queryStack;
        }

        private string RemoveApostrophes(string str) => str.Replace("'", string.Empty);

        private string GetLogicOperator(FilterQueryLogicalOperator op)
        {
            switch (op)
            {
                case FilterQueryLogicalOperator.and:
                    return "and";
                case FilterQueryLogicalOperator.or:
                    return "or";
                default:
                    throw new ApplicationException("Unknown logical operator");
            }
        }
        private string GetColumnName(string field, string entityName)
        {
            switch (field.ToLower())
            {
                case "name":
                case "status":
                    return $"{entityName}{field}";
                default:
                    return field;
            }
        }

        private string GetSqlOperator(FilterQueryOperator op)
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
                default:
                    throw new ApplicationException("Error in converting to SQL Query, check your code");
            }
        }
    }
}