using System;
using System.Reflection;
using System.Text;

namespace Moon.OData.Sql
{
    /// <summary>
    /// Represents SQL query with OData query options applied.
    /// </summary>
    public class ODataSqlQuery : SqlQueryBase
    {
        public ODataSqlQuery(string commandText, params object[] arguments)
            : base(commandText, arguments)
        {
            if (Options.Count != null && Options.Count.Value)
            {
                Count = new CountSqlQuery(this, commandText, arguments);
            }
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
            get => Options.IsCaseSensitive;
            set => Options.IsCaseSensitive = value;
        }

        /// <summary>
        /// Gets the SQL command that can be used to select total number of results ($orderby, $top
        /// and $skip options are not applied). The value is null if $count option is false or is
        /// not defined.
        /// </summary>
        public CountSqlQuery Count { get; }

        /// <inheritdoc />
        protected override string Build()
        {
            var builder = new StringBuilder();
            builder.Append(SelectClause.Build(OriginalCommandText, Options, ResolveColumn));
            builder.AppendWithSpace(WhereClause.Build(GetFilterOperator(), ArgumentsList, Options, ResolveColumn));
            builder.AppendWithSpace(OrderByClause.Build(Options, ResolveColumn));
            builder.AppendWithSpace(OffsetClause.Build(Options));
            return builder.ToString();
        }
    }
}