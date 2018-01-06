using System;
using System.Reflection;
using Microsoft.OData.UriParser;
using Moon.OData.Edm;

namespace Moon.OData.Sql
{
    /// <summary>
    /// The base class for SQL clause builders.
    /// </summary>
    public abstract class SqlClauseBase
    {
        protected SqlClauseBase(IODataOptions options)
        {
            Requires.NotNull(options, nameof(options));

            Options = options;
        }

        /// <summary>
        /// Gets or sets a function used to resolve primary key name.
        /// </summary>
        public Func<Type, string> ResolveKey { get; set; } = t => "[Id]";

        /// <summary>
        /// Gets or sets a function used to resolve column names.
        /// </summary>
        public Func<PropertyInfo, string> ResolveColumn { get; set; } = p => $"[{p.Name}]";

        /// <summary>
        /// Gets the OData options.
        /// </summary>
        protected IODataOptions Options { get; }

        /// <summary>
        /// Builds the SQL clause using the OData options.
        /// </summary>
        public abstract string Build();

        /// <summary>
        /// Retrieves <see cref="EdmClrProperty" /> from the given <see cref="SingleValueNode" />.
        /// </summary>
        /// <param name="node">The node to get the property from.</param>
        protected EdmClrProperty GetProperty(SingleValueNode node)
        {
            if (!(node is SingleValuePropertyAccessNode propertyAccessNode))
            {
                throw new ODataException($"The '{node.GetType().Name}' node is not supported.");
            }

            if (!(propertyAccessNode.Property is EdmClrProperty property))
            {
                throw new ODataException($"The '{propertyAccessNode.Property.GetType().Name}' property is not supported.");
            }

            if (!(propertyAccessNode.Source is ResourceRangeVariableReferenceNode))
            {
                throw new ODataException($"Nested properties are not supported.");
            }

            return property;
        }

        /// <summary>
        /// Retrieves <see cref="EdmClrProperty" /> from the given <see cref="SelectItem" />.
        /// </summary>
        /// <param name="item">The item to get the property from.</param>
        protected EdmClrProperty GetProperty(SelectItem item)
        {
            if (!(item is PathSelectItem pathItem))
            {
                throw new ODataException($"The '{item.GetType().Name}' select item is not supported.");
            }

            if (!(pathItem.SelectedPath.FirstSegment is PropertySegment propertySegment))
            {
                throw new ODataException($"The '{pathItem.SelectedPath.FirstSegment.GetType().Name}' segment is not supported.");
            }

            if (!(propertySegment.Property is EdmClrProperty property))
            {
                throw new ODataException($"The '{propertySegment.Property.GetType().Name}' property is not supported.");
            }

            return property;
        }
    }
}