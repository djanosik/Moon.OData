using System.Text;

namespace Moon.OData.Sql
{
    /// <summary>
    /// The <c>OFFSET</c> SQL clause builder.
    /// </summary>
    public class OffsetClause
    {
        readonly IODataOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="OffsetClause" /> class.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        public OffsetClause(IODataOptions options)
        {
            Requires.NotNull(options, nameof(options));

            this.options = options;
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
        public string Build()
        {
            var builder = new StringBuilder();

            if (options.Skip != null && options.OrderBy != null)
            {
                builder.Append($"OFFSET {options.Skip} ROWS");

                if (options.Top != null)
                {
                    builder.Append($" FETCH NEXT {options.Top} ROWS ONLY");
                }
            }

            return builder.ToString();
        }
    }
}