namespace FilterQueryLanguage.QueryBuilders
{
    public interface IQueryBuilder
    {
        string Name { get; }
        string GetQuery(QueryBuilderConfig config);
    }
}