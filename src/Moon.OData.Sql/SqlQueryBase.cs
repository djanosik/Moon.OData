using System;
using System.Collections.Generic;

namespace Moon.OData.Sql
{
    /// <summary>
    /// The base class for OData SQL queries.
    /// </summary>
    public abstract class SqlQueryBase
    {
        private readonly Lazy<string> result;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataSqlQuery" /> class.
        /// </summary>
        /// <param name="commandText">The SQL command text to use as a starting point.</param>
        /// <param name="arguments">
        /// The SQL query arguments. Include an <see cref="IODataOptions" /> as the last item.
        /// </param>
        protected SqlQueryBase(string commandText, params object[] arguments)
        {
            Requires.NotNull(commandText, nameof(commandText));
            Requires.NotNull(arguments, nameof(arguments));

            var last = arguments.Length - 1;

            if (last < 0 || !(arguments[last] is IODataOptions options))
            {
                throw new InvalidOperationException("You've forgot to include ODataOptions as the last argument.");
            }

            Options = options;
            OriginalCommandText = commandText;
            ArgumentsList = new List<object>(arguments);
            ArgumentsList.Remove(Options);
            result = Lazy.From(Build);
        }

        /// <summary>
        /// Gets the SQL command text.
        /// </summary>
        public string CommandText => result.Value;

        /// <summary>
        /// Gets the argument values of the SQL command.
        /// </summary>
        public object[] Arguments => CommandText != null ? ArgumentsList.ToArray() : Array.Empty<object>();

        /// <summary>
        /// Gets the OData query options.
        /// </summary>
        protected IODataOptions Options { get; }

        /// <summary>
        /// Gets the original SQL command passed to constructor.
        /// </summary>
        protected string OriginalCommandText { get; }

        /// <summary>
        /// Gets the list of SQL query arguments.
        /// </summary>
        protected List<object> ArgumentsList { get; }

        /// <summary>
        /// Builds a specifiec SQL clause.
        /// </summary>
        protected abstract string Build();

        /// <summary>
        /// Gets the next operator to be used in the WHERE clause.
        /// </summary>
        protected string GetFilterOperator()
        {
            return OriginalCommandText.Contains("WHERE", StringComparison.OrdinalIgnoreCase) ? "AND" : "WHERE";
        }
    }
}