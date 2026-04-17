using Dapper;

namespace CD5000Dashboard.Data
{
    public sealed class TableBrowserService
    {
        private readonly SqliteConnectionFactory _factory;

        public TableBrowserService(SqliteConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<string>> GetTableNamesAsync()
        {
            using var conn = _factory.CreateConnection();
            await conn.OpenAsync();

            const string sql = """
            SELECT name
            FROM sqlite_master
            WHERE type = 'table'
              AND name NOT LIKE 'sqlite_%'
            ORDER BY name;
            """;

            return await conn.QueryAsync<string>(sql);
        }

        public async Task<List<Dictionary<string, object?>>> GetTableRowsAsync(string tableName, int limit = 100)
        {
            using var conn = _factory.CreateConnection();
            await conn.OpenAsync();

            var sql = $"SELECT * FROM [{tableName}] LIMIT {limit};";
            var rows = await conn.QueryAsync(sql);

            var results = new List<Dictionary<string, object?>>();

            foreach (var row in rows)
            {
                var dict = new Dictionary<string, object?>();
                foreach (var pair in (IDictionary<string, object>)row)
                {
                    dict[pair.Key] = pair.Value;
                }
                results.Add(dict);
            }

            return results;
        }
    }
}
