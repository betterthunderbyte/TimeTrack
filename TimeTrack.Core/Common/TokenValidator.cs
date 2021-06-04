namespace TimeTrack.Core.Common
{
    public abstract class TokenValidator
    {
        protected string Secret;

        public TokenValidator(string secret)
        {
            Secret = secret;
        }

        public abstract bool GetValue(string key, out string value);
        public abstract bool GetValue(string key, out int value);
        public abstract bool GetValue(string key, out long value);
        public abstract bool GetValue(string key, out bool value);
        public abstract bool GetValue(string key, out float value);
        public abstract bool GetValue(string key, out double value);
        
        public bool Validate()
        {
            return false;
        }
    }
}