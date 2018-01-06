using System;

namespace Moon.OData
{
    /// <summary>
    /// An exception thrown when an OData query is not valid.
    /// </summary>
    public class ODataException : Exception
    {
        public ODataException(string message)
            : base(message)
        {
        }
    }
}