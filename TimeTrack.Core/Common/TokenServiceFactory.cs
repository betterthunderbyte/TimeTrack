namespace TimeTrack.Core.Common
{
    public abstract class TokenServiceFactory
    {
        protected TokenConfiguration TokenConfiguration;
        
        public TokenServiceFactory(TokenConfiguration configuration)
        {
            TokenConfiguration = configuration;
        }
        
        public abstract TokenBuilder CreateTokenBuilder();

        public abstract TokenValidator CreateTokenValidator();
    }
}