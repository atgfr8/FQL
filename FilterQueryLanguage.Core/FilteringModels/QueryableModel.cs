using System;
using System.Collections.Generic;
using System.Linq;
using FilterQueryLanguage.Core.Models;

namespace FilterQueryLanguage.Core.FilteringModels
{
    public class QueryableModel
    {
        public int? Skip { get; set; }
        public int? Top { get; set; }
        public IEnumerable<string> Select { get; set; }
        public QueryTree Query { get; set; }
        public string OrderBy { get; set; }
    }
}