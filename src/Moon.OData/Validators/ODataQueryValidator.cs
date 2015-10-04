namespace Moon.OData.Validators
{
    /// <summary>
    /// Represents a validator used to validate OData queries based on the <see cref="ValidationSettings" />.
    /// </summary>
    public class ODataQueryValidator
    {
        readonly SkipQueryValidator skipValidator = new SkipQueryValidator();
        readonly FilterQueryValidator filterValidator = new FilterQueryValidator();
        readonly TopQueryValidator topValidator = new TopQueryValidator();

        /// <summary>
        /// Validates the given OData query using the specified validation settings..
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <param name="options">The OData query to validate.</param>
        /// <param name="settings">The validation settings.</param>
        public virtual void Validate<TEntity>(ODataOptions<TEntity> options, ValidationSettings settings)
        {
            Requires.NotNull(options, nameof(options));
            Requires.NotNull(settings, nameof(settings));

            if (options.Count != null)
            {
                ValidateAllowed(AllowedOptions.Count, settings);
            }

            if (options.RawValues.DeltaToken != null)
            {
                ValidateAllowed(AllowedOptions.DeltaToken, settings);
            }

            if (options.RawValues.Format != null)
            {
                ValidateAllowed(AllowedOptions.Format, settings);
            }

            if (options.Filter != null)
            {
                ValidateAllowed(AllowedOptions.Filter, settings);
                filterValidator.Validate(options.Filter, settings);
            }

            if (options.OrderBy != null)
            {
                ValidateAllowed(AllowedOptions.OrderBy, settings);
            }

            if (options.Search != null)
            {
                ValidateAllowed(AllowedOptions.Search, settings);
            }

            if (options.RawValues.Select != null)
            {
                ValidateAllowed(AllowedOptions.Select, settings);
            }

            if (options.RawValues.Expand != null)
            {
                ValidateAllowed(AllowedOptions.Expand, settings);
            }

            if (options.Skip != null)
            {
                ValidateAllowed(AllowedOptions.Skip, settings);
                skipValidator.Validate(options.Skip, settings);
            }

            if (options.RawValues.SkipToken != null)
            {
                ValidateAllowed(AllowedOptions.SkipToken, settings);
            }

            if (options.Top != null)
            {
                ValidateAllowed(AllowedOptions.Top, settings);
                topValidator.Validate(options.Top, settings);
            }
        }

        void ValidateAllowed(AllowedOptions option, ValidationSettings settings)
        {
            if (!settings.AllowedOptions.HasFlag(option))
            {
                throw new ODataException($"The '{option}' query option is not allowed.");
            }
        }
    }
}