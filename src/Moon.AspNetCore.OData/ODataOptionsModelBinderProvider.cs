using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moon.OData;

namespace Moon.AspNetCore.OData
{
    /// <summary>
    /// An <see cref="IModelBinderProvider" /> for binding <see cref="ODataOptions{TEntity}" />.
    /// </summary>
    public class ODataOptionsModelBinderProvider : IModelBinderProvider
    {
        private readonly IEnumerable<IPrimitiveType> primitives;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataOptionsModelBinderProvider" /> class.
        /// </summary>
        /// <param name="primitives">An enumeration of primitive types.</param>
        public ODataOptionsModelBinderProvider(IEnumerable<IPrimitiveType> primitives)
        {
            this.primitives = primitives;
        }

        /// <summary>
        /// Returns an instance of the <see cref="ODataOptionsModelBinder" />.
        /// </summary>
        /// <param name="context">The instance context.</param>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var modelType = context.Metadata.ModelType;
            var typeInfo = modelType.GetTypeInfo();

            if (typeInfo.IsGenericType && (typeInfo.GetGenericTypeDefinition() == typeof(ODataOptions<>)))
            {
                return new ODataOptionsModelBinder(primitives);
            }

            return null;
        }
    }
}