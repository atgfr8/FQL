using System;
using FilterQueryLanguage.Core.FilteringModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace FilterQueryLanguage.Web.Filtering
{
    public class QueryableBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(QueryableModel))
            {
                return new BinderTypeModelBinder(typeof(QueryableBinder));
            }

            return null;
        }
    }
}