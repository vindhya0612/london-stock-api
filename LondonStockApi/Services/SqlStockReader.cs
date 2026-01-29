using Dapper;
using LondonStockApi.Models;
using Microsoft.Data.SqlClient;

namespace LondonStockApi.Services
{
    public sealed class SqlStockReader : IStockReader
    {
        private readonly string _connectionString;

        public SqlStockReader(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Sql")
                ?? throw new InvalidOperationException("SQL connection string is missing.");
        }

        public async Task<StockView?> GetOneAsync(string ticker)
        {
            const string sql = @"
                SELECT 
                    Ticker,
                    CAST(SumPrice / NULLIF(CountTrades, 0) AS DECIMAL(19,4)) AS AveragePrice,
                    CountTrades AS Trades,
                    LastUpdatedUtc
                FROM dbo.StockAggregates
                WHERE Ticker = @ticker;
            ";

            await using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<StockView>(sql, new { ticker });
        }

        public async Task<IEnumerable<StockView>> GetAllAsync(int page, int pageSize)
        {
            const string sql = @"
                SELECT 
                    Ticker,
                    CAST(SumPrice / NULLIF(CountTrades, 0) AS DECIMAL(19,4)) AS AveragePrice,
                    CountTrades AS Trades,
                    LastUpdatedUtc
                FROM dbo.StockAggregates
                ORDER BY Ticker
                OFFSET @offset ROWS
                FETCH NEXT @pageSize ROWS ONLY;
            ";

            await using var connection = new SqlConnection(_connectionString);

            var parameters = new
            {
                offset = (page - 1) * pageSize,
                pageSize
            };

            return await connection.QueryAsync<StockView>(sql, parameters);
        }

        public async Task<IEnumerable<StockView>> GetBatchAsync(IEnumerable<string> tickers)
        {
            const string sql = @"
                SELECT 
                    Ticker,
                    CAST(SumPrice / NULLIF(CountTrades, 0) AS DECIMAL(19,4)) AS AveragePrice,
                    CountTrades AS Trades,
                    LastUpdatedUtc
                FROM dbo.StockAggregates
                WHERE Ticker IN @tickers;
            ";

            await using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<StockView>(sql, new { tickers });
        }
    }
}