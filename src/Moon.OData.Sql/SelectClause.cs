using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.OData.UriParser;

namespace Moon.OData.Sql
{
    /// <summary>
    /// The <c>SELECT</c> SQL clause builder.
    /// </summary>
    public class SelectClause : SqlClauseBase
    {
        private readonly string commandText;

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
            : base(options)
        {
            Requires.NotNull(commandText, nameof(commandText));

            this.commandText = commandText.Trim();

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
        public override string Build()
        {
            return Regex.Replace(commandText, m =>
            {
                var builder = new StringBuilder("SELECT");
                builder.AppendWithSpace(Either(m.Groups[2].Value, () => TopClause.Build(Options)));
                builder.AppendWithSpace(Either(m.Groups[3].Value, BuildColumns));
                builder.AppendWithSpace(m.Groups[4].Value);
                return builder.ToString();
            });
        }

        private string Either(string value, Func<string> build)
            => string.IsNullOrEmpty(value) ? build() : value;

        private string BuildColumns()
        {
            var isFirst = true;
            var select = Options.SelectAndExpand;

            if (select == null || select.AllSelected)
            {
                return "*";
            }

            var builder = new StringBuilder();

            foreach (var item in select.SelectedItems)
            {
                if (isFirst && item is WildcardSelectItem)
                {
                    builder.Append("*");
                    break;
                }

                var property = GetProperty(item);
                var column = ResolveColumn(property.Property);

                if (column == null)
                {
                    throw new ODataException("The column name couldn't be resolved.");
                }

                if (!isFirst)
                {
                    builder.Append(", ");
                }

                builder.Append($"{column}");
                isFirst = false;
            }

            return builder.ToString();
        }
    }
}