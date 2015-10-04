namespace Moon.OData.Validators
{
    /// <summary>
    /// Represents a validator used to validate a $skip query option value.
    /// </summary>
    public class SkipQueryValidator
    {
        /// <summary>
        /// Validates the parsed $skip query option.
        /// </summary>
        /// <param name="skip">The parsed $skip query option.</param>
        /// <param name="settings">The validation settings.</param>
        public virtual void Validate(long? skip, ValidationSettings settings)
        {
            Requires.NotNull(settings, nameof(settings));

            if (skip > settings.MaxSkip)
            {
                throw new ODataException($"The $skip query option exceeded the ValidationSettiongs.MaxSkip value.");
            }
        }
    }
}