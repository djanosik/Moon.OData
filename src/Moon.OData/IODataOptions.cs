using System;
using Microsoft.OData.Core.UriParser.Semantic;

namespace Moon.OData
{
    /// <summary>
    /// Defines basic contract for OData query options.
    /// </summary>
    public interface IODataOptions
    {
        /// <summary>
        /// Gets the type of the entity you are building the query for.
        /// </summary>
        Type EntityType { get; }

        /// <summary>
        /// Gets a parsed $count query option.
        /// </summary>
        bool? Count { get; }

        /// <summary>
        /// Gets a parsed $deltatoken query option.
        /// </summary>
        string DeltaToken { get; }

        /// <summary>
        /// Gets a $filter clause parsed into semantic nodes.
        /// </summary>
        FilterClause Filter { get; }

        /// <summary>
        /// Gets a $oderby clause parsed into semantic nodes.
        /// </summary>
        OrderByClause OrderBy { get; }

        /// <summary>
        /// Gets a $search clause parsed into semantic nodes.
        /// </summary>
        SearchClause Search { get; }

        /// <summary>
        /// Gets a $select and $expand clauses parsed into semantic nodes.
        /// </summary>
        SelectExpandClause SelectAndExpand { get; }

        /// <summary>
        /// Gets a parsed $skip query option.
        /// </summary>
        long? Skip { get; }

        /// <summary>
        /// Gets a parsed $skiptoken query option.
        /// </summary>
        string SkipToken { get; }

        /// <summary>
        /// Gets a parsed $top query option.
        /// </summary>
        long? Top { get; }

        /// <summary>
        /// Gets raw OData query option values.
        /// </summary>
        ODataRawValues RawValues { get; }

        /// <summary>
        /// Validate all OData queries, including $skip, $top and $filter, based on the given settings.
        /// </summary>
        /// <param name="settings">The validation settings.</param>
        void Validate(ValidationSettings settings);
    }
}