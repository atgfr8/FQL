using System.Collections.Generic;
using System.Linq;

namespace FilterQueryLanguage.FQLParser.Syntax
{
    public static class FQLKeyWordHelper
    {
        private static Dictionary<string, FQLKeyword> _fqlKeywords = new Dictionary<string, FQLKeyword>
        {
            {
                "and", FQLKeyword.And
            },
            {
                "or", FQLKeyword.Or
            },
            {
                "find", FQLKeyword.Find
            },
            {
                "equal", FQLKeyword.Equal
            },
            {
                "notequal", FQLKeyword.NotEqual
            }
        };

        public static string GetString(this FQLKeyword keyword)
        {
           return _fqlKeywords.FirstOrDefault(x => x.Value == keyword).Key;
        }
    }
}