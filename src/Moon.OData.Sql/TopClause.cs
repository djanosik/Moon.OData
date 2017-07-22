namespace Moon.OData.Sql
{
    /// <summary>
    /// The <c>TOP(n)</c> SQL clause builder.
    /// </summary>
    public class TopClause : SqlClauseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopClause" /> class.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        public TopClause(IODataOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Builds a <c>TOP(n)</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="options">The OData query options.</param>
        public static string Build(IODataOptions options)
            => new TopClause(options).Build();

        /// <summary>
        /// Builds a <c>TOP(n)</c> SQL clause. The method returns an empty string when the $top
        /// option is not defined or when the $top and $skip options are both defined.
        /// </summary>
        public override string Build()
        {
            if (Options.Top != null && Options.Skip == null)
            {
                return $"TOP({Options.Top})";
            }

            return string.Empty;
        }
    }
}