using LondonStockApi.Models;

namespace LondonStockApi.Services
{
    public interface ITradeWriter
    {
        Task IngestAsync(TradeDto dto);
    } 
}