using System.Collections;
using System.Collections.Generic;

namespace Moon.OData
{
    /// <summary>
    /// The default result of an OData query. It should be used whenever the $count option is false
    /// or is not defined.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class Default<TResult> : IOData<TResult>, IEnumerable<TResult>
    {
        readonly IEnumerable<TResult> results;

        /// <summary>
        /// Initializes a new instance of the <see cref="Default{TResult}" /> class.
        /// </summary>
        /// <param name="results">The results of the query.</param>
        public Default(IEnumerable<TResult> results)
        {
            this.results = results;
        }

        /// <summary>
        /// Enumerates results of the query.
        /// </summary>
        public IEnumerator<TResult> GetEnumerator()
            => results.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}