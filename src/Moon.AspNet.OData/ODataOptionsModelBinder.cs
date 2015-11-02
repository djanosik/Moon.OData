using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.ModelBinding;
using Moon.OData;
using Moon.Reflection;

namespace Moon.AspNet.OData
{
    /// <summary>
    /// <see cref="IModelBinder" /> implementation to bind models of type <see cref="ODataOptions{TEntity}" />.
    /// </summary>
    public class ODataOptionsModelBinder : IModelBinder
    {
        readonly IEnumerable<IPrimitiveType> primitives;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataOptionsModelBinder" /> class.
        /// </summary>
        /// <param name="primitives">An enumeration of primitive types.</param>
        public ODataOptionsModelBinder(IEnumerable<IPrimitiveType> primitives)
        {
            this.primitives = primitives;
        }

        /// <summary>
        /// Binds an <see cref="ODataQuery{TEntity}" /> parameter if possible.
        /// </summary>
        /// <param name="bindingContext">The binding context.</param>
        public Task<ModelBindingResult> BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelType = bindingContext.ModelType;
            var request = bindingContext.OperationBindingContext.HttpContext.Request;
            var typeInfo = modelType.GetTypeInfo();

            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(ODataOptions<>))
            {
                var model = Class.Create(modelType, GetOptions(request), primitives);
                return ModelBindingResult.SuccessAsync(bindingContext.ModelName, model);
            }

            return ModelBindingResult.NoResultAsync;
        }

        IDictionary<string, string> GetOptions(HttpRequest request)
        {
            Requires.NotNull(request, nameof(request));

            return request.Query.ToDictionary(p => p.Key, p => p.Value.FirstOrDefault());
        }
    }
}