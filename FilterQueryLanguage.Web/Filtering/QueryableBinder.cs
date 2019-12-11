using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilterQueryLanguage.Core;
using FilterQueryLanguage.Core.FilteringModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace FilterQueryLanguage.Web.Filtering
{
    public class QueryableBinder : IModelBinder
    {
        private const string Skip = "skip";
        private const string Top = "top";
        private const string Select = "select";
        private const string Find = "find";
        private const string OrderBy = "orderby";
        private const string QueryPrefix = "@";

        private IEnumerable<string> AllowedOperators = new string[] {
            Skip, Top, Select, Find, OrderBy
        };

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var query = bindingContext.HttpContext.Request.Query;

            if (!AllowedOperators.Any(x => query.ContainsKey($"{QueryPrefix}{x}")))
            {
                return Task.CompletedTask;
            }

            var queryable = new QueryableModel();

            if (QueryHasKey(query, Skip))
            {
                queryable.Skip = GetIntValue(query, Skip);
            }

            if (QueryHasKey(query, Top))
            {
                queryable.Top = GetIntValue(query, Top);
            }

            if (QueryHasKey(query, Select))
            {
                queryable.Select = GetListStringValue(query, Select);
            }

            if(QueryHasKey(query, OrderBy))
            {
                queryable.OrderBy = GetStringValue(query, OrderBy);
            }

            if (QueryHasKey(query, Find))
            {
                var fullQuery = GetValue(query, Find);
                if(!fullQuery.Any(x => x == '\'')) throw new ApplicationException("You are required to enclose strings with a beginning ' and a ending ' like 'someString'");
                queryable.Query = FilterQueryLanguageHelper.GetQuery(fullQuery);
            }

            bindingContext.Result = ModelBindingResult.Success(queryable);
            return Task.CompletedTask;
        }
        
        private bool QueryHasKey(IQueryCollection query, string key)
        {
            return query.ContainsKey($"{QueryPrefix}{key}");
        }

        private IEnumerable<string> GetListStringValue(IQueryCollection query, string key)
        {
            var values = GetValue(query, key);
            return values.Split(',');
        }

        private int GetIntValue(IQueryCollection query, string key)
        {
            var value = GetStringValue(query, key);
            if (int.TryParse(value, out int intValue))
            {
                return intValue;
            }
            else
            {
                throw new ApplicationException($"Illegal value supplied to {key}");
            }
        }

        private string GetValue(IQueryCollection query, string key)
        {
            var value = GetStringValue(query, key);
            return $"{key}{value}";
        }

        private string GetStringValue(IQueryCollection query, string key)
        {
            query.TryGetValue($"{QueryPrefix}{key}", out StringValues value);
            return $"{value.ToString()}";
        }
    }
}