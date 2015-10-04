using System.Collections.Generic;

namespace Moon.AspNet.OData
{
    /// <summary>
    /// Defines the result of an OData query.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IOData<out TResult> : IEnumerable<TResult>
    {
    }
}