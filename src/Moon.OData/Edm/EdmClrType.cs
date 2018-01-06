using System;
using Microsoft.OData.Edm;

namespace Moon.OData.Edm
{
    /// <summary>
    /// Represents a definition of an EDM CLR type.
    /// </summary>
    public class EdmClrType : EdmEntityType
    {
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