namespace ChatAPI.Options
{
    public class DatabaseOptions
    {
        public const string DatabaseOption = "MongoDBSettings";
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
