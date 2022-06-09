using System;

namespace FilterQueryLanguage.FQLParser.Syntax
{
    public static class FilterQueryLogicalOperatorHelper
    {
        public static FilterQueryLogicalOperator GetOperator(string keyword)
        {
            switch(keyword.ToLower())
            {
                case "and":
                    return FilterQueryLogicalOperator.and;
                case "or":
                    return FilterQueryLogicalOperator.or;
            }
            throw new Exception("Logic operator provided is not supported or incorrect.");
        }
    }
}