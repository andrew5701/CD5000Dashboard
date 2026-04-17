using Microsoft.Data.Sqlite;

namespace CD5000Dashboard.Data
{
    public sealed class SqliteConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public SqliteConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqliteConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("CD5000");

            if (string.IsNullOrWhiteSpace(connectionString)) { 
                throw new InvalidOperationException("Connection string 'CD5000' was not found.");
            }

            return new SqliteConnection(connectionString);
        }
    }
}
