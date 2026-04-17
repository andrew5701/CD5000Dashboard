using CD5000Dashboard.Data.Models;
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

        public async Task<IEnumerable<HourlyTransactionRow>> GetTransactionsByHourAsync()
        {
            using var conn = _factory.CreateConnection();
            await conn.OpenAsync();

            var sql = """
            SELECT
                substr(Tr_Start_Time_Fld, 1, 2) AS Hour,
                COUNT(*) AS TransactionCount
            FROM [Transaction]
            WHERE Tr_Start_Time_Fld IS NOT NULL
              AND trim(Tr_Start_Time_Fld) <> ''
            GROUP BY substr(Tr_Start_Time_Fld, 1, 2)
            ORDER BY Hour;
            """;

            return await conn.QueryAsync<HourlyTransactionRow>(sql);
        }

        public async Task<IEnumerable<MissingRfidRow>> GetMissingRfidAsync()
        {
            using var conn = _factory.CreateConnection();
            await conn.OpenAsync();

            var sql = """
            SELECT
                Pk,
                Tr_Vehicle_Fld AS Vehicle,
                Tr_Tag_Id_Fld AS TagId,
                Tr_Unit_Train_Fld AS UnitTrain,
                Tr_Start_Date_Fld AS StartDate,
                Tr_Start_Time_Fld AS StartTime
            FROM [Transaction]
            WHERE Tr_Tag_Id_Fld IS NULL OR trim(Tr_Tag_Id_Fld) = ''
            ORDER BY Tr_Start_Date_Fld DESC, Tr_Start_Time_Fld DESC;
            """;

            return await conn.QueryAsync<MissingRfidRow>(sql);
        }

        public async Task<IEnumerable<AvgDurationByVehicleRow>> GetAverageDurationByVehicleAsync()
        {
            using var conn = _factory.CreateConnection();
            await conn.OpenAsync();

            var sql = """
            SELECT
                Tr_Vehicle_Fld AS Vehicle,
                ROUND(AVG(
                    (julianday(Tr_End_Date_Fld || ' ' || Tr_End_Time_Fld) -
                     julianday(Tr_Start_Date_Fld || ' ' || Tr_Start_Time_Fld)) * 86400
                ), 2) AS AverageDurationSeconds
            FROM [Transaction]
            WHERE Tr_Vehicle_Fld IS NOT NULL
              AND trim(Tr_Vehicle_Fld) <> ''
              AND Tr_Start_Date_Fld IS NOT NULL
              AND Tr_Start_Time_Fld IS NOT NULL
              AND Tr_End_Date_Fld IS NOT NULL
              AND Tr_End_Time_Fld IS NOT NULL
            GROUP BY Tr_Vehicle_Fld
            ORDER BY AverageDurationSeconds DESC
            LIMIT 10;
            """;

            return await conn.QueryAsync<AvgDurationByVehicleRow>(sql);
        }

        public async Task<IEnumerable<AvgDurationByUnitTrainRow>> GetAverageDurationByUnitTrainAsync()
        {
            using var conn = _factory.CreateConnection();
            await conn.OpenAsync();

            var sql = """
            SELECT
                Tr_Unit_Train_Fld AS UnitTrain,
                ROUND(AVG(
                    (julianday(Tr_End_Date_Fld || ' ' || Tr_End_Time_Fld) -
                     julianday(Tr_Start_Date_Fld || ' ' || Tr_Start_Time_Fld)) * 86400
                ), 2) AS AverageDurationSeconds
            FROM [Transaction]
            WHERE Tr_Unit_Train_Fld IS NOT NULL
              AND trim(Tr_Unit_Train_Fld) <> ''
              AND Tr_Start_Date_Fld IS NOT NULL
              AND Tr_Start_Time_Fld IS NOT NULL
              AND Tr_End_Date_Fld IS NOT NULL
              AND Tr_End_Time_Fld IS NOT NULL
            GROUP BY Tr_Unit_Train_Fld
            ORDER BY AverageDurationSeconds DESC
            LIMIT 10;
            """;

            return await conn.QueryAsync<AvgDurationByUnitTrainRow>(sql);
        }

        public async Task<IEnumerable<TopProductRow>> GetTopProductsAsync()
        {
            using var conn = _factory.CreateConnection();
            await conn.OpenAsync();

            var sql = """
            SELECT
                Tr_Prod_Id_Fld AS ProductCode,
                COUNT(*) AS TransactionCount
            FROM [Transaction]
            WHERE Tr_Prod_Id_Fld IS NOT NULL
              AND trim(Tr_Prod_Id_Fld) <> ''
            GROUP BY Tr_Prod_Id_Fld
            ORDER BY TransactionCount DESC
            LIMIT 10;
            """;

            return await conn.QueryAsync<TopProductRow>(sql);
        }
    }
}