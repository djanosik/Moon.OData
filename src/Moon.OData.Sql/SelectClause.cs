using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.OData.Core.UriParser.Semantic;
using Moon.OData.Edm;

namespace Moon.OData.Sql
{
    /// <summary>
    /// The <c>SELECT</c> SQL clause builder.
    /// </summary>
    public class SelectClause
    {
        readonly string commandText;
        readonly IODataOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectClause" /> class.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        public SelectClause(IODataOptions options)
            : this("SELECT FROM", options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectClause" /> class.
        /// </summary>
        /// <param name="commandText">
        /// The SQL command text containing a <c>SELECT</c> clause to modify.
        /// </param>
        /// <param name="options">The OData query options.</param>
        public SelectClause(string commandText, IODataOptions options)
        {
            Requires.NotNull(commandText, nameof(commandText));
            Requires.NotNull(options, nameof(options));

            this.commandText = commandText.Trim();
            this.options = options;

            if (!Regex.IsMatch(commandText))
            {
                throw new NotSupportedException("The SQL command text does not contain any of supported SELECT clauses.");
            }
        }

        /// <summary>
        /// Gets a regular expression matching the <c>SELECT</c> SQL clause.
        /// </summary>
        public static Regex Regex { get; } = new Regex(@"^(SELECT)\s+(TOP\(\d+\))?\s*(.*?)\s*(FROM.*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Gets or sets a function used to resolve column names.
        /// </summary>
        public Func<PropertyInfo, string> ResolveColumn { get; set; } = p => $"[{p.Name}]";

        /// <summary>
        /// Builds a <c>SELECT</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        public static string Build(IODataOptions options)
            => Build(options, null);

        /// <summary>
        /// Builds a <c>SELECT</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="commandText">
        /// The SQL command text containing a <c>SELECT</c> clause to modify.
        /// </param>
        /// <param name="options">The OData query options.</param>
        public static string Build(string commandText, IODataOptions options)
            => Build(commandText, options, null);

        /// <summary>
        /// Builds a <c>SELECT</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        /// <param name="resolveColumn">A function used to resolve column names.</param>
        public static string Build(IODataOptions options, Func<PropertyInfo, string> resolveColumn)

        {
            var clause = new SelectClause(options);

            if (resolveColumn != null)
            {
                clause.ResolveColumn = resolveColumn;
            }

            return clause.Build();
        }

        /// <summary>
        /// Builds a <c>SELECT</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="commandText">
        /// The SQL command text containing a <c>SELECT</c> clause to modify.
        /// </param>
        /// <param name="options">The OData query options.</param>
        /// <param name="resolveColumn">A function used to resolve column names.</param>
        public static string Build(string commandText, IODataOptions options, Func<PropertyInfo, string> resolveColumn)
        {
            var clause = new SelectClause(commandText, options);

            if (resolveColumn != null)
            {
                clause.ResolveColumn = resolveColumn;
            }

            return clause.Build();
        }

        /// <summary>
        /// Builds a <c>SELECT</c> SQL clause.
        /// </summary>
        public string Build()
        {
            return Regex.Replace(commandText, m =>
            {
                var builder = new StringBuilder("SELECT");
                builder.AppendWithSpace(Either(m.Groups[2].Value, () => TopClause.Build(options)));
                builder.AppendWithSpace(Either(m.Groups[3].Value, BuildColumns));
                builder.AppendWithSpace(m.Groups[4].Value);
                return builder.ToString();
            });
        }

        string Either(string value, Func<string> build)
            => string.IsNullOrEmpty(value) ? build() : value;

        string BuildColumns()
        {
            var isFirst = true;
            var select = options.SelectAndExpand;

            if (select == null || select.AllSelected)
            {
                return "*";
            }

            var builder = new StringBuilder();

            foreach (var item in select.SelectedItems)
            {
                var path = item as PathSelectItem;
                var wildcard = item as WildcardSelectItem;

                if (isFirst && wildcard != null)
                {
                    builder.Append("*");
                    break;
                }

                if (path == null)
                {
                    throw new ODataException($"The '{item.GetType().Name}' select item is not supported.");
                }

                var segment = path.SelectedPath.FirstSegment as PropertySegment;

                if (segment == null)
                {
                    throw new ODataException($"The '{segment.GetType().Name}' path segment is not supported.");
                }

                var property = segment.Property as EdmClrProperty;

                if (property == null)
                {
                    throw new ODataException($"The '{property.GetType().Name}' property is not supported.");
                }

                if (!isFirst)
                {
                    builder.Append(", ");
                }

                var column = ResolveColumn(property.Property);

                if (column == null)
                {
                    throw new ODataException("The column name is invalid.");
                }

                builder.Append($"{column}");
                isFirst = false;
            }

            return builder.ToString();
        }
    }
}