using Microsoft.Extensions.Options;
using TimeTrack.Core.Configuration;

namespace TimeTrack.Web.Service.Validators
{
    public class DatabaseConfigurationValidator : IValidateOptions<DatabaseConfiguration>
    {
        public ValidateOptionsResult Validate(string name, DatabaseConfiguration options)
        {
            return ValidateOptionsResult.Success;
        }
    }
}