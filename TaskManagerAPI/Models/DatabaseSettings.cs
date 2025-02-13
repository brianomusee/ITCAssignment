namespace TaskManagerAPI.Models
{
    /// <summary>
    /// Represents database configuration from appsettings.json.
    /// </summary>
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
