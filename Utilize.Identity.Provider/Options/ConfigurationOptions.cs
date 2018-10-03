namespace Utilize.Identity.Provider.Options
{
    public class ConfigurationOptions
    {
        public string MongoConnection { get; set; }
        public string MongoDatabaseName { get; set; }
        public string CockroachHost { get; set; }
        public int CockroachPort { get; set; }
        public string CockroachUsername { get; set; }
        public string CockroachDatabase { get; set; }
    }
}