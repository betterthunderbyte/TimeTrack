namespace TimeTrack.Core.Configuration
{
    public class DatabaseConfiguration
    {
        /// <summary>
        /// sqlite oder mariadb
        /// </summary>
        public string Driver { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}