namespace TimeTrack.Core.Common
{
    public class HmacTokenValidator : TokenValidator
    {
        public HmacTokenValidator(string secret) : base(secret)
        {
        }

        public override bool GetValue(string key, out string value)
        {
            throw new System.NotImplementedException();
        }

        public override bool GetValue(string key, out int value)
        {
            throw new System.NotImplementedException();
        }

        public override bool GetValue(string key, out long value)
        {
            throw new System.NotImplementedException();
        }

        public override bool GetValue(string key, out bool value)
        {
            throw new System.NotImplementedException();
        }

        public override bool GetValue(string key, out float value)
        {
            throw new System.NotImplementedException();
        }

        public override bool GetValue(string key, out double value)
        {
            throw new System.NotImplementedException();
        }
    }
}