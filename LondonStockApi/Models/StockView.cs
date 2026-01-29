namespace LondonStockApi.Models;
public record StockView(string Ticker,decimal? AveragePrice,long Trades,DateTime LastUpdatedUtc);
