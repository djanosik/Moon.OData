using System.Collections;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace Moon.OData
{
    /// <summary>
    /// The default result of an OData query. It should be used whenever the $count option is false
    /// or is not defined.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class Default<TResult> : IOData<TResult>, IEnumerable<TResult>
    {
        private readonly IEnumerable<TResult> results;

        public Default(IEnumerable<TResult> results)
        {
            this.results = results;
        }

        /// <summary>
        /// Enumerates results of the query.
        /// </summary>
        public IEnumerator<TResult> GetEnumerator()
        {
            return results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}