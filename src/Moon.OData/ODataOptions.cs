using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.OData.Core.UriParser;
using Microsoft.OData.Core.UriParser.Semantic;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;
using Moon.OData.Edm;
using Moon.OData.Validators;

namespace Moon.OData
{
    /// <summary>
    /// Represents OData query options that can be used to perform query composition.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity you are building the query for.</typeparam>
    public class ODataOptions<TEntity> : IODataOptions
    {
        private readonly Lazy<bool?> count;
        private readonly Lazy<string> deltaToken;
        private readonly Lazy<FilterClause> filter;
        private readonly Lazy<OrderByClause> orderBy;

        private readonly ODataQueryOptionParser parser;
        private readonly Lazy<SearchClause> search;
        private readonly Lazy<SelectExpandClause> selectAndExpand;
        private readonly Lazy<long?> skip;
        private readonly Lazy<string> skipToken;
        private readonly Lazy<long?> top;
        private readonly ODataQueryValidator validator = new ODataQueryValidator();

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataOptions{TEntity}" /> class.
        /// </summary>
        /// <param name="options">The dictionary storing query option key-value pairs.</param>
        public ODataOptions(IDictionary<string, string> options)
            : this(options, Enumerable.Empty<IPrimitiveType>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataOptions{TEntity}" /> class.
        /// </summary>
        /// <param name="options">The dictionary storing query option key-value pairs.</param>
        /// <param name="primitives">An enumeration of additional primitive types.</param>
        public ODataOptions(IDictionary<string, string> options, IEnumerable<IPrimitiveType> primitives)
        {
            Requires.NotNull(options, nameof(options));
            Requires.NotNull(primitives, nameof(primitives));

            RawValues = new ODataRawValues(options);

            parser = CreateParser(primitives);
            count = Lazy.From(parser.ParseCount);
            deltaToken = Lazy.From(parser.ParseDeltaToken);
            filter = Lazy.From(parser.ParseFilter);
            orderBy = Lazy.From(parser.ParseOrderBy);
            search = Lazy.From(parser.ParseSearch);
            selectAndExpand = Lazy.From(parser.ParseSelectAndExpand);
            skip = Lazy.From(parser.ParseSkip);
            skipToken = Lazy.From(parser.ParseSkipToken);
            top = Lazy.From(parser.ParseTop);

            IsCaseSensitive = true;
        }

        /// <summary>
        /// Gets the type of the entity you are building the query for.
        /// </summary>
        public Type EntityType
            => typeof(TEntity);

        /// <summary>
        /// Gets or sets whether the parser is case sensitive when matching names of properties. The
        /// default value is true.
        /// </summary>
        public bool IsCaseSensitive
        {
            get { return !parser.Resolver.EnableCaseInsensitive; }
            set { parser.Resolver.EnableCaseInsensitive = !value; }
        }

        /// <summary>
        /// Gets a parsed $count query option.
        /// </summary>
        public bool? Count
            => count.Value;

        /// <summary>
        /// Gets a parsed $deltatoken query option.
        /// </summary>
        public string DeltaToken
            => deltaToken.Value;

        /// <summary>
        /// Gets a $filter clause parsed into semantic nodes.
        /// </summary>
        public FilterClause Filter
            => filter.Value;

        /// <summary>
        /// Gets a $oderby clause parsed into semantic nodes.
        /// </summary>
        public OrderByClause OrderBy
            => orderBy.Value;

        /// <summary>
        /// Gets a $search clause parsed into semantic nodes.
        /// </summary>
        public SearchClause Search
            => search.Value;

        /// <summary>
        /// Gets a $select and $expand clauses parsed into semantic nodes.
        /// </summary>
        public SelectExpandClause SelectAndExpand
            => selectAndExpand.Value;

        /// <summary>
        /// Gets a parsed $skip query option.
        /// </summary>
        public long? Skip
            => skip.Value;

        /// <summary>
        /// Gets a parsed $skiptoken query option.
        /// </summary>
        public string SkipToken
            => skipToken.Value;

        /// <summary>
        /// Gets a parsed $top query option.
        /// </summary>
        public long? Top
            => top.Value;

        /// <summary>
        /// Gets raw OData query option values.
        /// </summary>
        public ODataRawValues RawValues { get; }

        /// <summary>
        /// Validate all OData queries, including $skip, $top and $filter, based on the given settings.
        /// </summary>
        /// <param name="settings">The validation settings.</param>
        public virtual void Validate(ValidationSettings settings)
        {
            Requires.NotNull(settings, nameof(settings));

            validator.Validate(this, settings);
        }

        private static IEnumerable<IPrimitiveType> GetPrimitives(IEnumerable<IPrimitiveType> primitives)
        {
            yield return new PrimitiveType<byte[]>(EdmPrimitiveTypeKind.Binary);
            yield return new PrimitiveType<bool>(EdmPrimitiveTypeKind.Boolean);
            yield return new PrimitiveType<byte>(EdmPrimitiveTypeKind.Byte);
            yield return new PrimitiveType<DateTime>(EdmPrimitiveTypeKind.Date);
            yield return new PrimitiveType<DateTimeOffset>(EdmPrimitiveTypeKind.DateTimeOffset);
            yield return new PrimitiveType<decimal>(EdmPrimitiveTypeKind.Decimal);
            yield return new PrimitiveType<double>(EdmPrimitiveTypeKind.Double);
            yield return new PrimitiveType<Guid>(EdmPrimitiveTypeKind.Guid);
            yield return new PrimitiveType<short>(EdmPrimitiveTypeKind.Int16);
            yield return new PrimitiveType<int>(EdmPrimitiveTypeKind.Int32);
            yield return new PrimitiveType<long>(EdmPrimitiveTypeKind.Int64);
            yield return new PrimitiveType<sbyte>(EdmPrimitiveTypeKind.SByte);
            yield return new PrimitiveType<float>(EdmPrimitiveTypeKind.Single);
            yield return new PrimitiveType<string>(EdmPrimitiveTypeKind.String);
            yield return new PrimitiveType<char>(EdmPrimitiveTypeKind.String);
            yield return new PrimitiveType<char[]>(EdmPrimitiveTypeKind.String);
            yield return new PrimitiveType<Type>(EdmPrimitiveTypeKind.String);
            yield return new PrimitiveType<Uri>(EdmPrimitiveTypeKind.String);
            yield return new PrimitiveType<TimeSpan>(EdmPrimitiveTypeKind.Duration);

            foreach (var primitive in primitives)
            {
                yield return primitive;
            }
        }

        private ODataQueryOptionParser CreateParser(IEnumerable<IPrimitiveType> primitives)
        {
            var model = GetEdmModel(GetPrimitives(primitives).ToDictionary(p => p.Type));
            var entities = model.FindDeclaredNavigationSource("Entities");

            return new ODataQueryOptionParser(model, entities.EntityType(),
                entities, RawValues.Values);
        }

        private EdmModel GetEdmModel(IDictionary<Type, IPrimitiveType> primitives)
        {
            var result = new EdmModel();

            var container = new EdmEntityContainer("Default", "Container");
            container.AddEntitySet("Entities", GetEdmType(typeof(TEntity), primitives));
            result.AddElement(container);

            return result;
        }

        private EdmClrType GetEdmType(Type type, IDictionary<Type, IPrimitiveType> primitives)
        {
            var result = new EdmClrType(type);

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (primitives.ContainsKey(property.PropertyType))
                {
                    var primitive = primitives[property.PropertyType];
                    result.AddProperty(new EdmClrProperty(result, property, primitive.Kind));
                }
                else
                {
                    var propertyType = GetEdmType(property.PropertyType, primitives);
                    result.AddProperty(new EdmClrProperty(result, property, propertyType));
                }
            }

            return result;
        }
    }
}