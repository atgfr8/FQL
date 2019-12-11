using System.Collections.Generic;
using FilterQueryLanguage.Core.Models;

namespace FilterQueryLanguage.QueryBuilders
{
    public class QueryBuilderConfig
    {
        public QueryBuilderConfig(QueryTree query)
        {
            Query = query;
        }
        public QueryTree Query { get; set; }
        public IEnumerable<QueryBuilderSetting> QueryBuilderSettings { get; set; }
    }

    public class QueryBuilderSetting
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}