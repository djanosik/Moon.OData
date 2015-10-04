using System;

namespace Moon.OData
{
    /// <summary>
    /// An exception thrown when an OData query is not valid.
    /// </summary>
    public class ODataException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODataException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public ODataException(string message)
            : base(message)
        {
        }
    }
}