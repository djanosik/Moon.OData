using System;
using System.Text;

namespace Moon.OData.Sql
{
    /// <summary>
    /// The <c>SELECT COUNT(Key)</c> SQL clause builder.
    /// </summary>
    public class CountClause : SqlClauseBase
    {
        private readonly string commandText;

        public CountClause(IODataOptions options)
            : this("SELECT FROM", options)
        {
        }

        public CountClause(string commandText, IODataOptions options)
            : base(options)
        {
            Requires.NotNull(commandText, nameof(commandText));

            this.commandText = commandText.Trim();

            if (!SelectClause.Regex.IsMatch(commandText))
            {
                throw new NotSupportedException("The SQL command text does not contain any of supported SELECT clauses.");
            }
        }

        /// <summary>
        /// Builds a <c>SELECT COUNT(Key)</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        public static string Build(IODataOptions options)
        {
            return Build(options, null);
        }

        /// <summary>
        /// Builds a <c>SELECT COUNT(Key)</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="commandText">
        /// The SQL command text containing a <c>SELECT</c> clause to modify.
        /// </param>
        /// <param name="options">The OData query options.</param>
        public static string Build(string commandText, IODataOptions options)
        {
            return Build(commandText, options, null);
        }

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
        public override string Build()
        {
            return SelectClause.Regex.Replace(commandText, m =>
            {
                var builder = new StringBuilder("SELECT");
                builder.AppendWithSpace($"COUNT({ResolveKey(Options.EntityType)})");
                builder.AppendWithSpace(m.Groups[4].Value);
                return builder.ToString();
            });
        }
    }
}