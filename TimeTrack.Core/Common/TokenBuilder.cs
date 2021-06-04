namespace TimeTrack.Core.Common
{
    /// <summary>
    /// Baut ein Token die als Zeichenkette zurückgegeben wird
    /// </summary>
    public abstract class TokenBuilder
    {
        protected string Secret;

        public TokenBuilder(string secret)
        {
            Secret = secret;
        }

        public abstract TokenBuilder AddValue(string key, string value);

        public abstract TokenBuilder AddValue(string key, int value);

        public abstract TokenBuilder AddValue(string key, long value);

        public abstract TokenBuilder AddValue(string key, bool value);

        public abstract TokenBuilder AddValue(string key, float value);

        public abstract TokenBuilder AddValue(string key, double value);

        public abstract string Build();
    }
}