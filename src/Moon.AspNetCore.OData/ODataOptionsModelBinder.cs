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
        private readonly IEnumerable<IPrimitiveType> primitives;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataOptionsModelBinder" /> class.
        /// </summary>
        /// <param name="primitives">An enumeration of primitive types.</param>
        public ODataOptionsModelBinder(IEnumerable<IPrimitiveType> primitives)
        {
            this.primitives = primitives;
        }

        /// <summary>
        /// Binds an <see cref="ODataOptions{TEntity}" /> parameter if possible.
        /// </summary>
        /// <param name="bindingContext">The binding context.</param>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelType = bindingContext.ModelType;
            var request = bindingContext.HttpContext.Request;
            var model = Class.Create(modelType, GetOptions(request), primitives);
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }

        private IDictionary<string, string> GetOptions(HttpRequest request)
        {
            Requires.NotNull(request, nameof(request));

            return request.Query.ToDictionary(p => p.Key, p => p.Value.FirstOrDefault());
        }
    }
}