using System;

namespace FilterQueryLanguage.FQLParser.Syntax
{
    public static class FQLQueryOperatorHelper
    {
        public static FilterQueryOperator GetFilterQueryOperator(string op)
        {
            switch(op.ToLower())
            {
                case "notequal":
                    return FilterQueryOperator.notEqual;
                case "equal":
                    return FilterQueryOperator.equal;
                case "contains":
                    return FilterQueryOperator.contains;
                case "startswith":
                    return FilterQueryOperator.startsWith;
                default:
                    throw new Exception("Invalid operator detected, check your query for errors");
            }
        }
    }
}