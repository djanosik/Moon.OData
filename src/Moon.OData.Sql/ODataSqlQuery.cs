using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Moon.OData.Sql
{
    /// <summary>
    /// Represents SQL query with OData query options applied.
    /// </summary>
    public class ODataSqlQuery
    {
        readonly string commandText;
        readonly List<object> arguments = new List<object>();
        readonly IODataOptions options;
        readonly Lazy<string> result;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataSqlQuery" /> class.
        /// </summary>
        /// <param name="commandText">The SQL command text to use as a starting point.</param>
        /// <param name="arguments">
        /// The argument values of the SQL command. Include an <see cref="IODataOptions" /> as the
        /// last item.
        /// </param>
        public ODataSqlQuery(string commandText, params object[] arguments)
        {
            Requires.NotNull(commandText, nameof(commandText));
            Requires.NotNull(arguments, nameof(arguments));

            var last = arguments.Length - 1;
            options = (IODataOptions)arguments[last];

            this.commandText = commandText;
            this.arguments = new List<object>(arguments);
            this.arguments.Remove(options);

            if (options.Count != null && options.Count.Value)
            {
                Count = new CountSqlQuery(this, commandText, arguments);
            }

            result = Lazy.From(Build);
        }

        /// <summary>
        /// Gets or sets a function used to resolve primary key column name.
        /// </summary>
        public Func<Type, string> ResolveKey { get; set; }

        /// <summary>
        /// Gets or sets a function used to resolve column names.
        /// </summary>
        public Func<PropertyInfo, string> ResolveColumn { get; set; }

        /// <summary>
        /// Gets or sets whether the query options parser is case sensitive when matching names of
        /// properties. The default value is true.
        /// </summary>
        public bool IsCaseSensitive
        {
            get { return options.IsCaseSensitive; }
            set { options.IsCaseSensitive = !value; }
        }

        /// <summary>
        /// Gets the SQL command that can be used to select total number of results ($orderby, $top
        /// and $skip options are not applied). The value is null if $count option is false or is
        /// not defined.
        /// </summary>
        public CountSqlQuery Count { get; }

        /// <summary>
        /// Gets the SQL command text with OData options applied.
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
            builder.Append(SelectClause.Build(commandText, options, ResolveColumn));
            builder.AppendWithSpace(WhereClause.Build(GetOperator(), arguments, options, ResolveColumn));
            builder.AppendWithSpace(OrderByClause.Build(options, ResolveColumn));
            builder.AppendWithSpace(OffsetClause.Build(options));
            return builder.ToString();
        }

        string GetOperator()
            => commandText.Contains("WHERE", StringComparison.OrdinalIgnoreCase) ? "AND" : "WHERE";
    }
}