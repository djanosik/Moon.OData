using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Moon.AspNet.OData;
using Moon.OData;

namespace Microsoft.Framework.DependencyInjection
{
    /// <summary>
    /// <see cref="IMvcBuilder" /> extension methods.
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Adds services necessary to bind <see cref="ODataQuery{TEntity}" /> as an action method parameter.
        /// </summary>
        /// <param name="services">The ASP.NET MVC builder.</param>
        public static IMvcBuilder AddOData(this IMvcBuilder builder)
            => builder.AddOData(Enumerable.Empty<IPrimitiveType>());

        /// <summary>
        /// Adds services necessary to bind <see cref="ODataQuery{TEntity}" /> as an action method parameter.
        /// </summary>
        /// <param name="services">The ASP.NET MVC builder.</param>
        /// <param name="primitives">An enumeration of additional primitive types.</param>
        public static IMvcBuilder AddOData(this IMvcBuilder builder, IEnumerable<IPrimitiveType> primitives)
        {
            var services = builder.Services;

            services.Configure<MvcOptions>(o =>
            {
                o.ModelBinders.Add(new ODataOptionsModelBinder(primitives));
            });

            return builder;
        }
    }
}