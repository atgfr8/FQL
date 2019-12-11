using System.Collections.Generic;

namespace FilterQueryLanguage.Core.FilteringModels
{
    public class FilterViewModel
    {
        public int TotalRecords { get; set; }
        public IEnumerable<object> Data {get;set;}
    }

}