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

        private readonly bool isCaseSensitive;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataOptionsModelBinder" /> class.
        /// </summary>
        /// <param name="primitives">An enumeration of primitive types.</param>
        public ODataOptionsModelBinder(IEnumerable<IPrimitiveType> primitives) :this(primitives,true)
        {
            this.primitives = primitives;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataOptionsModelBinder" /> class.
        /// </summary>
        /// <param name="primitives">An enumeration of primitive types.</param>
        /// <param name="isCaseSensitive">Properties are case sensitive</param>
        public ODataOptionsModelBinder(IEnumerable<IPrimitiveType> primitives, bool isCaseSensitive)
        {
            this.primitives = primitives;
            this.isCaseSensitive = isCaseSensitive;
        }

        /// <summary>
        /// Binds an <see cref="ODataOptions{TEntity}" /> parameter if possible.
        /// </summary>
        /// <param name="bindingContext">The binding context.</param>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelType = bindingContext.ModelType;
            var request = bindingContext.HttpContext.Request;
            var model = Class.Activate(modelType, GetOptions(request), primitives.ToArray(), isCaseSensitive);
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