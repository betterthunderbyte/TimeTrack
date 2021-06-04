namespace TimeTrack.Core.Common
{
    public class HmacTokenServiceFactory : TokenServiceFactory
    {
        public HmacTokenServiceFactory(TokenConfiguration configuration) : base(configuration)
        {
        }

        public override TokenBuilder CreateTokenBuilder()
        {
            return new HmacTokenBuilder(TokenConfiguration.Secret);
        }

        public override TokenValidator CreateTokenValidator()
        {
            return new HmacTokenValidator(TokenConfiguration.Secret);
        }
    }
}