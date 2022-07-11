namespace FilterQueryLanguage.FQLParser.Syntax
{
    public enum FilterQueryOperator
    {
        find,
        contains,
        startsWith,
        equal,
        notEqual,
        greaterThan,
        greaterThanOrEqualTo,
        lessThan,
        lessThanOrEqualTo,
    }

    public enum FilterQueryLogicalOperator
    {
        or,
        and
    }
}