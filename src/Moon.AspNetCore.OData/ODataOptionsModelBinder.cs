using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moon.OData;
using Moon.Reflection;

namespace Moon.AspNetCore.OData
{
    /// <summary>
    /// <see cref="IModelBinder" /> implementation to bind models of type <see cref="ODataOptions{TEntity}" />.
    /// </summary>
    public class ODataOptionsModelBinder : IModelBinder
    {
        private readonly bool isCaseSensitive;
        private readonly IEnumerable<IPrimitiveType> primitiveTypes;

        public ODataOptionsModelBinder(IEnumerable<IPrimitiveType> primitiveTypes)
            : this(primitiveTypes, true)
        {
        }

        public ODataOptionsModelBinder(IEnumerable<IPrimitiveType> primitiveTypes, bool isCaseSensitive)
        {
            this.primitiveTypes = primitiveTypes;
            this.isCaseSensitive = isCaseSensitive;
        }

        /// <inheritdoc />
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelType = bindingContext.ModelType;
            var request = bindingContext.HttpContext.Request;
            var model = Class.Activate(modelType, GetOptions(request), primitiveTypes.ToArray(), isCaseSensitive);
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }

        private Dictionary<string, string> GetOptions(HttpRequest request)
        {
            Requires.NotNull(request, nameof(request));

            return request.Query.ToDictionary(p => p.Key, p => p.Value.FirstOrDefault());
        }
    }
}