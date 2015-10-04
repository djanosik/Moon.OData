using System;
using Microsoft.OData.Edm;

namespace Moon.OData
{
    /// <summary>
    /// A primitive type definition.
    /// </summary>
    public interface IPrimitiveType
    {
        /// <summary>
        /// Gets the <see cref="System.Type" /> of the primitive type.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the kind of the EDM primitive type.
        /// </summary>
        EdmPrimitiveTypeKind Kind { get; }
    }
}