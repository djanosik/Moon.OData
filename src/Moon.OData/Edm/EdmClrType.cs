using System;
using Microsoft.OData.Edm.Library;

namespace Moon.OData.Edm
{
    /// <summary>
    /// Represents a definition of an EDM CLR type.
    /// </summary>
    public class EdmClrType : EdmEntityType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdmClrType" /> class.
        /// </summary>
        /// <param name="type">The underlying CLR type.</param>
        public EdmClrType(Type type)
            : base(type.Namespace, type.Name)
        {
            Requires.NotNull(type, nameof(type));

            Type = type;
        }

        /// <summary>
        /// Gets the underlying CRL type.
        /// </summary>
        public Type Type { get; }
    }
}