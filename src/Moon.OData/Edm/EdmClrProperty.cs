using System;
using System.Reflection;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;

namespace Moon.OData.Edm
{
    /// <summary>
    /// Represents a definition of an EDM CLR property.
    /// </summary>
    public class EdmClrProperty : EdmStructuralProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdmClrProperty" /> class.
        /// </summary>
        /// <param name="declaringType">The type the property belongs to.</param>
        /// <param name="property">The underlying CLR property.</param>
        /// <param name="type">The type of the property.</param>
        public EdmClrProperty(EdmClrType declaringType, PropertyInfo property, EdmClrType type)
            : base(declaringType, property.Name, new EdmEntityTypeReference(type, IsNullable(property.PropertyType)))
        {
            Requires.NotNull(declaringType, nameof(declaringType));
            Requires.NotNull(property, nameof(property));
            Requires.NotNull(type, nameof(type));

            Property = property;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdmClrProperty" /> class.
        /// </summary>
        /// <param name="declaringType">The type the property belongs to.</param>
        /// <param name="property">The underlying CLR property.</param>
        /// <param name="type">The type of the property.</param>
        public EdmClrProperty(EdmClrType declaringType, PropertyInfo property, EdmPrimitiveTypeKind type)
            : base(declaringType, property.Name, EdmCoreModel.Instance.GetPrimitive(type, IsNullable(property.PropertyType)))
        {
            Requires.NotNull(declaringType, nameof(declaringType));
            Requires.NotNull(property, nameof(property));

            Property = property;
        }

        /// <summary>
        /// Gets the type that this property belongs to.
        /// </summary>
        public new EdmClrType DeclaringType
            => base.DeclaringType as EdmClrType;

        /// <summary>
        /// Gets the underlying CLR property.
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        /// Gets the type of this property.
        /// </summary>
        public new EdmClrType Type
            => base.Type as EdmClrType;

        static bool IsNullable(Type type)
            => !type.GetTypeInfo().IsValueType || Nullable.GetUnderlyingType(type) != null;
    }
}