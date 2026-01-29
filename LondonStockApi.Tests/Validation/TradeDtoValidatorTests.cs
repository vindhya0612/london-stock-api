using FluentAssertions;
using FluentValidation.TestHelper;
using LondonStockApi.Models;
using LondonStockApi.Validation;
using Xunit;

namespace LondonStockApi.Tests.Validation {

    public class TradeDtoValidatorTests {

        private readonly TradeDtoValidator _validator=new();

        [Fact]
        public void Fails_on_invalid()

        {
            var dto=new TradeDto(Guid.Empty,"",0,0,"",null);
            var result=_validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x=>x.TradeId);
            result.ShouldHaveValidationErrorFor(x=>x.Ticker);
            result.ShouldHaveValidationErrorFor(x=>x.Price);
            result.ShouldHaveValidationErrorFor(x=>x.Shares);
            result.ShouldHaveValidationErrorFor(x=>x.BrokerId);

        } 

        [Fact]
        public void Passes_on_valid()   {

            var dto=new TradeDto(Guid.NewGuid(),"AAPL",100.5m,10m,"BRK-1",DateTime.UtcNow);
            var result=_validator.TestValidate(dto);
            result.IsValid.Should().BeTrue();
        }
    }
}