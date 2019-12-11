using System.Linq;
using FilterQueryLanguage.Core.Models;
using FilterQueryLanguage.Core.Tokens;

namespace FilterQueryLanguage.Core
{
    public static class FilterQueryLanguageHelper
    {
        public static QueryTree GetQuery(string query)
        {
            var tokens = new PrecedenceBasedRegexTokenizer().Tokenize(query).ToList();
            return new Parser().Parse(tokens);
        }
    }
}