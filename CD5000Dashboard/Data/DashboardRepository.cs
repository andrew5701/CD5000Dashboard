using Dapper;
namespace CD5000Dashboard.Data
{
    public sealed class DashboardRepository
    {
        private readonly SqliteConnectionFactory _factory;

        public DashboardRepository(SqliteConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<int> GetTransactionCountAsync()
        {
            using var conn = _factory.CreateConnection();
            await conn.OpenAsync();

            return await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM [Transaction];");
        }
        public async Task<IEnumerable<ProductHourlyStat>> GetTransactionsByHourAsync()
        {
            using var conn = _factory.CreateConnection();
            await conn.OpenAsync();

            var sql = """
            SELECT
                substr(Tr_Start_Time_Fld, 1, 2) AS Hour,
                COUNT(*) AS Count
            FROM [Transaction]
            WHERE Tr_Start_Time_Fld IS NOT NULL
            GROUP BY Hour
            ORDER BY Hour;
            """;

            return await conn.QueryAsync<ProductHourlyStat>(sql);
        }

        public class ProductHourlyStat
        {
            public string? Hour { get; set; }
            public int Count { get; set; }
        }
        public async Task<IEnumerable<dynamic>> GetMissingRfidAsync()
        {
            using var conn = _factory.CreateConnection();
            await conn.OpenAsync();

            var sql = """
            SELECT
                Tr_Vehicle_Fld AS Vehicle,
                Tr_Tag_Id_Fld AS TagId,
                Tr_Start_Date_Fld AS Date
            FROM [Transaction]
            WHERE Tr_Tag_Id_Fld IS NULL OR trim(Tr_Tag_Id_Fld) = ''
            """;

            return await conn.QueryAsync(sql);
        }
    }
}