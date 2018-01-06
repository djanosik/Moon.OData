using System;
using System.Reflection;
using Microsoft.OData.Edm;

namespace Moon.OData.Edm
{
    /// <summary>
    /// Represents a definition of an EDM CLR property.
    /// </summary>
    public class EdmClrProperty : EdmStructuralProperty
    {
        public EdmClrProperty(EdmClrType declaringType, PropertyInfo property, EdmClrType type)
            : base(declaringType, property.Name, new EdmEntityTypeReference(type, IsNullable(property.PropertyType)))
        {
            Requires.NotNull(declaringType, nameof(declaringType));
            Requires.NotNull(property, nameof(property));
            Requires.NotNull(type, nameof(type));

            Property = property;
        }

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

        private static bool IsNullable(Type type)
        {
            return !type.GetTypeInfo().IsValueType || Nullable.GetUnderlyingType(type) != null;
        }
    }
}