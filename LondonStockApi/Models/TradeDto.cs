namespace LondonStockApi.Models;
public record TradeDto(Guid TradeId,string Ticker,decimal Price,decimal Shares,string BrokerId,DateTime? TimestampUtc);
