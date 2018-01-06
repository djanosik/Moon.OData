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
        private readonly bool isCaseSensitive;
        private readonly IEnumerable<IPrimitiveType> primitiveTypes;

        public ODataOptionsModelBinderProvider(IEnumerable<IPrimitiveType> primitiveTypes, bool isCaseSensitive)
        {
            this.primitiveTypes = primitiveTypes;
            this.isCaseSensitive = isCaseSensitive;
        }

        /// <inheritdoc />
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var modelType = context.Metadata.ModelType;
            var typeInfo = modelType.GetTypeInfo();

            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(ODataOptions<>))
            {
                return new ODataOptionsModelBinder(primitiveTypes, isCaseSensitive);
            }

            return null;
        }
    }
}