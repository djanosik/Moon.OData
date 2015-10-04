using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Moon.OData.Sql
{
    /// <summary>
    /// Represents SQL query with OData $filter option applied. It can be used to select total
    /// number of results before $top and $skip options are applied.
    /// </summary>
    public class CountSqlQuery
    {
        readonly ODataSqlQuery query;
        readonly string commandText;
        readonly List<object> arguments;
        readonly IODataOptions options;
        readonly Lazy<string> result;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataSqlQuery" /> class.
        /// </summary>
        /// <param name="query">The parent OData SQL query.</param>
        /// <param name="commandText">The SQL command text to use as a starting point.</param>
        /// <param name="arguments">
        /// The argument values of the SQL command. Include an <see cref="IODataOptions" /> as the
        /// last item.
        /// </param>
        public CountSqlQuery(ODataSqlQuery query, string commandText, params object[] arguments)
        {
            Requires.NotNull(query, nameof(query));
            Requires.NotNull(commandText, nameof(commandText));
            Requires.NotNull(arguments, nameof(arguments));

            var last = arguments.Length - 1;
            options = (IODataOptions)arguments[last];

            this.query = query;
            this.commandText = commandText;
            this.arguments = new List<object>(arguments);
            this.arguments.Remove(options);

            result = Lazy.From(Build);
        }

        /// <summary>
        /// Gets the SQL command text with OData $filter option applied.
        /// </summary>
        public string CommandText
            => result.Value;

        /// <summary>
        /// Gets the argument values of the SQL command.
        /// </summary>
        public object[] Arguments
        {
            get
            {
                Debug.WriteLine(CommandText);
                return arguments.ToArray();
            }
        }

        string Build()
        {
            var builder = new StringBuilder();
            builder.Append(CountClause.Build(commandText, options, query.ResolveKey));
            builder.AppendWithSpace(WhereClause.Build(GetOperator(), arguments, options, query.ResolveColumn));
            return builder.ToString();
        }

        string GetOperator()
            => commandText.Contains("WHERE", StringComparison.OrdinalIgnoreCase) ? "AND" : "WHERE";
    }
}