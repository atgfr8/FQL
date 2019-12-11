using System.Collections.Generic;

namespace FilterQueryLanguage.Core.Models
{
    public class SubQueryModel
    {
        public SubQueryModel()
        {
            SubQueries = new List<SubQueryModel>();
        }

        public FilterQueryLogicalOperator? LogicalOperator { get; set; }
        public IList<SubQueryModel> SubQueries { get; set; }
        public PredicateModel Predicate { get; set; }
    }
}