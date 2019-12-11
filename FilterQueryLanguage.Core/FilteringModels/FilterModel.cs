using System.Collections.Generic;

namespace FilterQueryLanguage.Core.FilteringModels
{
    public class FilterModel<T>
    {
        public int TotalRecords {get;set;}
        public IEnumerable<T> Data {get;set;}
    }
}