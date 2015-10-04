using System;
using System.Text;

namespace Moon.OData.Sql
{
    /// <summary>
    /// The <c>SELECT COUNT(Key)</c> SQL clause builder.
    /// </summary>
    public class CountClause
    {
        readonly string commandText;
        readonly IODataOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="CountClause" /> class.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        public CountClause(IODataOptions options)
            : this("SELECT FROM", options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountClause" /> class.
        /// </summary>
        /// <param name="commandText">
        /// The SQL command text containing a <c>SELECT</c> clause to modify.
        /// </param>
        /// <param name="options">The OData query options.</param>
        public CountClause(string commandText, IODataOptions options)
        {
            Requires.NotNull(commandText, nameof(commandText));
            Requires.NotNull(options, nameof(options));

            this.commandText = commandText.Trim();
            this.options = options;

            if (!SelectClause.Regex.IsMatch(commandText))
            {
                throw new NotSupportedException("The SQL command text does not contain any of supported SELECT clauses.");
            }
        }

        /// <summary>
        /// Gets or sets a function used to resolve primary key column name.
        /// </summary>
        public Func<Type, string> ResolveKey { get; set; } = t => "[Id]";

        /// <summary>
        /// Builds a <c>SELECT COUNT(Key)</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        public static string Build(IODataOptions options)
            => Build(options, null);

        /// <summary>
        /// Builds a <c>SELECT COUNT(Key)</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="commandText">
        /// The SQL command text containing a <c>SELECT</c> clause to modify.
        /// </param>
        /// <param name="options">The OData query options.</param>
        public static string Build(string commandText, IODataOptions options)
            => Build(commandText, options, null);

        /// <summary>
        /// Builds a <c>SELECT COUNT(Key)</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        /// <param name="resolveKey">A function used to resolve primary key column name.</param>
        public static string Build(IODataOptions options, Func<Type, string> resolveKey)
        {
            var clause = new CountClause(options);

            if (resolveKey != null)
            {
                clause.ResolveKey = resolveKey;
            }

            return clause.Build();
        }

        /// <summary>
        /// Builds a <c>SELECT COUNT(Key)</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="commandText">
        /// The SQL command text containing a <c>SELECT</c> clause to modify.
        /// </param>
        /// <param name="options">The OData query options.</param>
        /// <param name="resolveKey">A function used to resolve primary key column name.</param>
        public static string Build(string commandText, IODataOptions options, Func<Type, string> resolveKey)
        {
            var clause = new CountClause(commandText, options);

            if (resolveKey != null)
            {
                clause.ResolveKey = resolveKey;
            }

            return clause.Build();
        }

        /// <summary>
        /// Builds a <c>SELECT</c> SQL clause.
        /// </summary>
        public string Build()
        {
            return SelectClause.Regex.Replace(commandText, m =>
            {
                var builder = new StringBuilder("SELECT");
                builder.AppendWithSpace($"COUNT({ResolveKey(options.EntityType)})");
                builder.AppendWithSpace(m.Groups[4].Value);
                return builder.ToString();
            });
        }

        string Either(string value, Func<string> build)
            => string.IsNullOrEmpty(value) ? build() : value;
    }
}