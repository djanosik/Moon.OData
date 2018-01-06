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