using FluentAssertions;
using FluentValidation;
using LondonStockApi.Controllers;
using LondonStockApi.Models;
using LondonStockApi.Services;
using LondonStockApi.Validation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LondonStockApi.Tests.Controllers

{
    public class TradesControllerTests{
        
        [Fact]
        public async Task Returns_400_on_validation_error()
        
        {
            IValidator<TradeDto> v=new TradeDtoValidator();
            var writer=new Mock<ITradeWriter>(MockBehavior.Strict);
            var c=new TradesController(v,writer.Object);
            var invalid=new TradeDto(Guid.Empty,"",0m,0m,"",null);
            
            var result=await c.Post(invalid);
           result.Should().BeOfType<BadRequestObjectResult>();
            
            writer.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task Returns_202_on_ok()
        {
            IValidator<TradeDto> v=new TradeDtoValidator();
            var writer=new Mock<ITradeWriter>();

            writer.Setup(w=>w.IngestAsync(It.IsAny<TradeDto>())).Returns(Task.CompletedTask);

            var c=new TradesController(v,writer.Object);
            var valid=new TradeDto(Guid.NewGuid(),"AAPL",12.34m,1m,"BRK-1",DateTime.UtcNow);
            
            var result=await c.Post(valid);
            result.Should().BeOfType<AcceptedResult>();
            
            writer.Verify(w=>w.IngestAsync(It.Is<TradeDto>(d=>d.Ticker=="AAPL")),Times.Once);
        }
    }
}
