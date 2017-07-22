using System;
using System.Text;

namespace Moon.OData.Sql
{
    /// <summary>
    /// Represents SQL query with OData $filter option applied. It can be used to select total number
    /// of results before $top and $skip options are applied.
    /// </summary>
    public class CountSqlQuery : SqlQueryBase
    {
        private readonly ODataSqlQuery query;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataSqlQuery" /> class.
        /// </summary>
        /// <param name="query">The parent OData SQL query.</param>
        /// <param name="commandText">The SQL command text to use as a starting point.</param>
        /// <param name="arguments">
        /// The SQL query arguments. Include an <see cref="IODataOptions" /> as the last item.
        /// </param>
        public CountSqlQuery(ODataSqlQuery query, string commandText, params object[] arguments)
            : base(commandText, arguments)
        {
            Requires.NotNull(query, nameof(query));

            this.query = query;
        }

        /// <inheritdoc />
        protected override string Build()
        {
            var builder = new StringBuilder();
            builder.Append(CountClause.Build(OriginalCommandText, Options, query.ResolveKey));
            builder.AppendWithSpace(WhereClause.Build(GetFilterOperator(), ArgumentsList, Options, query.ResolveColumn));
            return builder.ToString();
        }
    }
}