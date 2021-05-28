using Microsoft.Extensions.Options;

namespace TimeTrack.Core.Configuration.Validators
{
    public class DatabaseConfigurationValidator : IValidateOptions<DatabaseConfiguration>
    {
        public ValidateOptionsResult Validate(string name, DatabaseConfiguration options)
        {
            return ValidateOptionsResult.Success;
        }
    }
}