using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace Moon.OData
{
    /// <summary>
    /// The paged result of an OData query. It's specifically designed for paging, but it should be
    /// used whenever the $count options is true.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <remarks>The result does not conform to the OData 4.0 JSON specification.</remarks>
    public class Paged<TResult> : IOData<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Paged{TResult}" /> class.
        /// </summary>
        /// <param name="results">The results of the query.</param>
        /// <param name="count">The number of items in the collection.</param>
        public Paged(IEnumerable<TResult> results, long count)
        {
            Results = results;
            Count = count;
        }

        /// <summary>
        /// Gets the results of the query.
        /// </summary>
        public IEnumerable<TResult> Results { get; }

        /// <summary>
        /// Gets the number of filtered items.
        /// </summary>
        public long Count { get; }
    }
}