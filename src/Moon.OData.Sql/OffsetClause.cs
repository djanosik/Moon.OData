using System.Text;

namespace Moon.OData.Sql
{
    /// <summary>
    /// The <c>OFFSET</c> SQL clause builder.
    /// </summary>
    public class OffsetClause : SqlClauseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OffsetClause" /> class.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        public OffsetClause(IODataOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Builds an <c>OFFSET</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        public static string Build(IODataOptions options)
            => new OffsetClause(options).Build();

        /// <summary>
        /// Builds an <c>OFFSET</c> SQL clause. The method returns an empty string when either the
        /// $skip or the $orderby option is not defined.
        /// </summary>
        public override string Build()
        {
            var builder = new StringBuilder();

            if ((Options.Skip != null) && (Options.OrderBy != null))
            {
                builder.Append($"OFFSET {Options.Skip} ROWS");

                if (Options.Top != null)
                {
                    builder.Append($" FETCH NEXT {Options.Top} ROWS ONLY");
                }
            }

            return builder.ToString();
        }
    }
}