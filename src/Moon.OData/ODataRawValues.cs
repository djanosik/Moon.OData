using System.Collections.Generic;

namespace Moon.OData
{
    /// <summary>
    /// Represents a dictionary of raw OData query option values.
    /// </summary>
    public sealed class ODataRawValues
    {
        readonly HashSet<string> supportedOptions = GetSupportedOptions();

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataRawValues" /> class.
        /// </summary>
        /// <param name="options">The dictionary storing query option key-value pairs.</param>
        public ODataRawValues(IDictionary<string, string> options)
        {
            Requires.NotNull(options, nameof(options));

            Values = new Dictionary<string, string>();

            foreach (var pair in options)
            {
                if (supportedOptions.Contains(pair.Key))
                {
                    Values.Add(pair.Key, pair.Value);
                }
            }
        }

        /// <summary>
        /// Gets raw OData query option values.
        /// </summary>
        public IDictionary<string, string> Values { get; }

        /// <summary>
        /// Gets the raw $count option value.
        /// </summary>
        public string Count
            => TryGetValue("$count");

        /// <summary>
        /// Gets the raw $deltatoken option value.
        /// </summary>
        public string DeltaToken
            => TryGetValue("$deltatoken");

        /// <summary>
        /// Gets the raw $format option value.
        /// </summary>
        public string Format
            => TryGetValue("$format");

        /// <summary>
        /// Gets the raw $filter option value.
        /// </summary>
        public string Filter
            => TryGetValue("$filter");

        /// <summary>
        /// Gets the raw $orderby option value.
        /// </summary>
        public string OrderBy
            => TryGetValue("$orderby");

        /// <summary>
        /// Gets the raw $search option value.
        /// </summary>
        public string Search
            => TryGetValue("$search");

        /// <summary>
        /// Gets the raw $select option value.
        /// </summary>
        public string Select
            => TryGetValue("$select");

        /// <summary>
        /// Gets the raw $expand option value.
        /// </summary>
        public string Expand
            => TryGetValue("$expand");

        /// <summary>
        /// Gets the raw $skip option value.
        /// </summary>
        public string Skip
            => TryGetValue("$skip");

        /// <summary>
        /// Gets the raw $skiptoken option value.
        /// </summary>
        public string SkipToken
            => TryGetValue("$skiptoken");

        /// <summary>
        /// Gets the raw $top option value.
        /// </summary>
        public string Top
            => TryGetValue("$top");

        static HashSet<string> GetSupportedOptions()
        {
            return new HashSet<string>
            {
                "$count",
                "$format",
                "$deltatoken",
                "$filter",
                "$orderby",
                "$search",
                "$select",
                "$expand",
                "$skip",
                "$skiptoken",
                "$top"
            };
        }

        string TryGetValue(string key)
            => Values.ContainsKey(key) ? Values[key] : null;
    }
}