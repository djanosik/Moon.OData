using System;
using System.Reflection;
using System.Text;
using Microsoft.OData.Core.UriParser;
using Microsoft.OData.Core.UriParser.Semantic;
using Moon.OData.Edm;
using OBClause = Microsoft.OData.Core.UriParser.Semantic.OrderByClause;

namespace Moon.OData.Sql
{
    /// <summary>
    /// The <c>ORDER BY</c> SQL clause builder.
    /// </summary>
    public class OrderByClause
    {
        readonly IODataOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderByClause" /> class.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        public OrderByClause(IODataOptions options)
        {
            Requires.NotNull(options, nameof(options));

            this.options = options;
        }

        /// <summary>
        /// Gets or sets a function used to resolve column names.
        /// </summary>
        public Func<PropertyInfo, string> ResolveColumn { get; set; } = p => $"[{p.Name}]";

        /// <summary>
        /// Builds a <c>ORDER BY</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        public static string Build(IODataOptions options)
            => Build(options, null);

        /// <summary>
        /// Builds a <c>ORDER BY</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        /// <param name="resolveColumn">A function used to resolve column names.</param>
        public static string Build(IODataOptions options, Func<PropertyInfo, string> resolveColumn)
        {
            var clause = new OrderByClause(options);

            if (resolveColumn != null)
            {
                clause.ResolveColumn = resolveColumn;
            }

            return clause.Build();
        }

        /// <summary>
        /// Builds a <c>ORDER BY</c> SQL clause. The method returns an empty string when the
        /// $orderby option is not defined.
        /// </summary>
        public string Build()
        {
            var builder = new StringBuilder();

            if (options.OrderBy != null)
            {
                builder.Append("ORDER BY");
                AppendColumn(builder, options.OrderBy, true);
            }

            return builder.ToString();
        }

        void AppendColumn(StringBuilder builder, OBClause clause, bool isFirst)
        {
            var node = clause.Expression as SingleValuePropertyAccessNode;

            if (node == null)
            {
                throw new ODataException($"The '{clause.Expression.GetType().Name}' node is not supported.");
            }

            var property = node.Property as EdmClrProperty;

            if (property == null)
            {
                throw new ODataException($"The '{property.GetType().Name}' property is not supported.");
            }

            if (!isFirst)
            {
                builder.Append(",");
            }

            var column = ResolveColumn(property.Property);

            if (column == null)
            {
                throw new ODataException("The column name is invalid.");
            }

            builder.Append($" {column}");

            if (clause.Direction == OrderByDirection.Descending)
            {
                builder.Append(" DESC");
            }

            if (clause.ThenBy != null)
            {
                AppendColumn(builder, clause.ThenBy, false);
            }
        }
    }
}