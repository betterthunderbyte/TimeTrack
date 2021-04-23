namespace TimeTrack.Core.Configuration
{
    public class JsonWebTokenConfiguration
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        /// <summary>
        /// Das Geheimnis für die Hashgeneration
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// Wie lange ist ein Token gültig
        /// </summary>
        public int AccessTokenExpiration { get; set; }
    }
}