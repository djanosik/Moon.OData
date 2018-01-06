using System.Collections.Generic;
using System.Linq;
using Moon.AspNetCore.OData;
using Moon.OData;

// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Adds services necessary to bind <see cref="ODataOptions{TEntity}" /> as an action method parameter.
        /// </summary>
        /// <param name="builder">The ASP.NET MVC builder.</param>
        public static IMvcBuilder AddOData(this IMvcBuilder builder)
        {
            return builder.AddOData(Enumerable.Empty<IPrimitiveType>());
        }

        /// <summary>
        /// Adds services necessary to bind <see cref="ODataOptions{TEntity}" /> as an action method parameter.
        /// </summary>
        /// <param name="builder">The ASP.NET MVC builder.</param>
        /// <param name="primitiveTypes">An enumeration of additional primitive types.</param>
        public static IMvcBuilder AddOData(this IMvcBuilder builder, IEnumerable<IPrimitiveType> primitiveTypes)
        {
            return builder.AddOData(primitiveTypes, true);
        }

        /// <summary>
        /// Adds services necessary to bind <see cref="ODataOptions{TEntity}" /> as an action method parameter.
        /// </summary>
        /// <param name="builder">The ASP.NET MVC builder.</param>
        /// <param name="isCaseSensitive">Indicates whether the property binding is case sensitive.</param>
        public static IMvcBuilder AddOData(this IMvcBuilder builder, bool isCaseSensitive)
        {
            return builder.AddOData(Enumerable.Empty<IPrimitiveType>(), isCaseSensitive);
        }

        /// <summary>
        /// Adds services necessary to bind <see cref="ODataOptions{TEntity}" /> as an action method parameter.
        /// </summary>
        /// <param name="builder">The ASP.NET MVC builder.</param>
        /// <param name="primitiveTypes">An enumeration of additional primitive types.</param>
        /// <param name="isCaseSensitive">Indicates whether the property binding is case sensitive.</param>
        public static IMvcBuilder AddOData(this IMvcBuilder builder, IEnumerable<IPrimitiveType> primitiveTypes, bool isCaseSensitive)
        {
            return builder.AddMvcOptions(o => o.ModelBinderProviders.Insert(0, new ODataOptionsModelBinderProvider(primitiveTypes, isCaseSensitive)));
        }
    }
}