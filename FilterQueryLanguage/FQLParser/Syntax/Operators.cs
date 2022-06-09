namespace FilterQueryLanguage.FQLParser.Syntax
{
    public enum FilterQueryOperator
    {
        find,
        contains,
        startsWith,
        equal,
        notEqual
    }

    public enum FilterQueryLogicalOperator
    {
        or,
        and
    }
}