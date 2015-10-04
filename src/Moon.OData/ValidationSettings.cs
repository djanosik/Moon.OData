namespace Moon.OData
{
    /// <summary>
    /// This class describes the validation settings for querying.
    /// </summary>
    public class ValidationSettings
    {
        /// <summary>
        /// Gets or sets the query parameters that are allowed inside query. The default is all
        /// query options, including $count, $deltatoken, $format, $filter, $orderby, $search,
        /// $select, $expand, $skip, $skiptoken and $top.
        /// </summary>
        public AllowedOptions AllowedOptions { get; set; } = AllowedOptions.All;

        /// <summary>
        /// Gets or sets a list of allowed functions used in the $filter query.
        /// </summary>
        public AllowedFunctions AllowedFunctions { get; set; } = AllowedFunctions.All;

        /// <summary>
        /// Gets or sets a list of allowed operators in the $filter query.
        /// </summary>
        public AllowedOperators AllowedOperators { get; set; } = AllowedOperators.All;

        /// <summary>
        /// Gets or sets the max value of $skip that a client can request.
        /// </summary>
        public int? MaxSkip { get; set; }

        /// <summary>
        /// Gets or sets the max value of $top that a client can request.
        /// </summary>
        public int? MaxTop { get; set; }
    }
}