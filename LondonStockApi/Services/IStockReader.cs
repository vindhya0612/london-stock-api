using LondonStockApi.Models;

namespace LondonStockApi.Services
{
    public interface IStockReader{
        Task<StockView?> GetOneAsync(string ticker);
        Task<IEnumerable<StockView>> GetAllAsync(int page,int pageSize);
        Task<IEnumerable<StockView>> GetBatchAsync(IEnumerable<string> tickers);
    } 
}