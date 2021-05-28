using System.Text;
using Microsoft.Extensions.Options;

namespace TimeTrack.Core.Configuration.Validators
{
    public class JsonWebTokenConfigurationValidator : IValidateOptions<JsonWebTokenConfiguration>
    {
        public ValidateOptionsResult Validate(string name, JsonWebTokenConfiguration options)
        {
            if (string.IsNullOrWhiteSpace(options.Audience))
            {
                return ValidateOptionsResult.Fail(
                    $"Die Property {nameof(options.Audience)} für JWT-Token ist nicht vorhanden!"
                );
            }
            
            if (string.IsNullOrWhiteSpace(options.Issuer))
            {
                return ValidateOptionsResult.Fail(
                    $"Die Property {nameof(options.Issuer)} für JWT-Token ist nicht vorhanden!"
                );
            }
            
            if (string.IsNullOrWhiteSpace(options.Secret))
            {
                return ValidateOptionsResult.Fail(
                    $"Die Property {nameof(options.Secret)} für JWT-Token ist nicht vorhanden!"
                );
            }
            
            if (options.AccessTokenExpiration < 60)
            {
                return ValidateOptionsResult.Fail(
                    $"Die Property {nameof(options.AccessTokenExpiration)} für JWT-Token ist kleiner als 60!"
                );
            }
            
            if (Encoding.ASCII.GetByteCount(options.Secret) < 32)
            {
                return ValidateOptionsResult.Fail(
                    $"Die Property {nameof(options.Secret)} für JWT-Token ist zu kurz! {Encoding.ASCII.GetByteCount(options.Secret)} < 32"
                    );
            }
            
            return ValidateOptionsResult.Success;
        }
    }
}