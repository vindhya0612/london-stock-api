using FluentValidation;
using LondonStockApi.Models;

namespace LondonStockApi.Validation;
    
    public sealed class TradeDtoValidator:AbstractValidator<TradeDto> {
    
    public TradeDtoValidator()
    
    {
        RuleFor(x=>x.TradeId).NotEmpty();
        RuleFor(x=>x.Ticker).NotEmpty().MaximumLength(20);
        RuleFor(x=>x.Price).GreaterThan(0);
        RuleFor(x=>x.Shares).GreaterThan(0);
        RuleFor(x=>x.BrokerId).NotEmpty().MaximumLength(64);
    }
}