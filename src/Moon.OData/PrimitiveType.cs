﻿using System;
using Microsoft.OData.Edm;

namespace Moon.OData
{
    /// <summary>
    /// The primitive type definition.
    /// </summary>
    /// <typeparam name="TPrimitive">The type of the primitive type.</typeparam>
    public sealed class PrimitiveType<TPrimitive> : IPrimitiveType
    {
        public PrimitiveType(EdmPrimitiveTypeKind kind)
        {
            Kind = kind;
        }

        /// <summary>
        /// Gets the <see cref="System.Type" /> of the primitive type.
        /// </summary>
        public Type Type => typeof(TPrimitive);

        /// <summary>
        /// Gets the kind of the EDM primitive type.
        /// </summary>
        public EdmPrimitiveTypeKind Kind { get; }
    }
}