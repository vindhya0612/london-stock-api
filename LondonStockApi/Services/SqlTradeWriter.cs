using System.Data;
using Dapper;
using LondonStockApi.Models;
using Microsoft.Data.SqlClient;

namespace LondonStockApi.Services
{
    public sealed class SqlTradeWriter : ITradeWriter
    {
        private readonly string _connectionString;

        public SqlTradeWriter(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Sql")
                ?? throw new InvalidOperationException("SQL connection string is missing.");
        }

        public async Task IngestAsync(TradeDto dto)
        {
            await using var connection = new SqlConnection(_connectionString);

            var parameters = new
            {
                dto.TradeId,
                dto.Ticker,
                dto.Price,
                dto.Shares,
                dto.BrokerId,
                TimestampUtc = dto.TimestampUtc ?? DateTime.UtcNow
            };

            await connection.ExecuteAsync(
                "UpsertTrade",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}