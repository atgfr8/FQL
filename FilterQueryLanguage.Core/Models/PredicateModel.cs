namespace FilterQueryLanguage.Core.Models
{
    public class PredicateModel
    {
        public string Field { get; set; }
        public FilterQueryOperator Operator { get; set; }
        public string Value { get; set; }
    }
}