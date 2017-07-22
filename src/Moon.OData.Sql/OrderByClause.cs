using System;
using System.Reflection;
using System.Text;
using Microsoft.OData.UriParser;
using OBClause = Microsoft.OData.UriParser.OrderByClause;

namespace Moon.OData.Sql
{
    /// <summary>
    /// The <c>ORDER BY</c> SQL clause builder.
    /// </summary>
    public class OrderByClause : SqlClauseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderByClause" /> class.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        public OrderByClause(IODataOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Builds a <c>ORDER BY</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        public static string Build(IODataOptions options)
        {
            return Build(options, null);
        }

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
        public override string Build()
        {
            var builder = new StringBuilder();

            if (Options.OrderBy != null)
            {
                builder.Append("ORDER BY");
                AppendColumns(builder, Options.OrderBy, true);
            }

            return builder.ToString();
        }

        private void AppendColumns(StringBuilder builder, OBClause clause, bool isFirst)
        {
            var property = GetProperty(clause.Expression);
            var column = ResolveColumn(property.Property);

            if (column == null)
            {
                throw new ODataException("The column name couldn't be resolved.");
            }

            if (!isFirst)
            {
                builder.Append(",");
            }

            builder.Append($" {column}");

            if (clause.Direction == OrderByDirection.Descending)
            {
                builder.Append(" DESC");
            }

            if (clause.ThenBy != null)
            {
                AppendColumns(builder, clause.ThenBy, false);
            }
        }
    }
}