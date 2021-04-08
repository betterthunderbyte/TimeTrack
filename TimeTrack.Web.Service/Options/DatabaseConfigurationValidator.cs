using Microsoft.Extensions.Options;

namespace TimeTrack.Web.Service.Options
{
    public class DatabaseConfigurationValidator : IValidateOptions<DatabaseConfiguration>
    {
        public ValidateOptionsResult Validate(string name, DatabaseConfiguration options)
        {
            return ValidateOptionsResult.Success;
        }
    }
}