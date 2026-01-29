using FluentAssertions;
using LondonStockApi.Controllers;
using LondonStockApi.Models;
using LondonStockApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LondonStockApi.Tests.Controllers

{
    public class StocksControllerTests
    
    {
        [Fact]
        public async Task GetOne_ok() {
            
            var r=new Mock<IStockReader>();
            r.Setup(x=>x.GetOneAsync("AAPL")).ReturnsAsync(new StockView("AAPL",123.45m,10,DateTime.UtcNow));
            
            var c=new StocksController(r.Object);
            var result=await c.GetOne("AAPL");
            result.Result.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public async Task GetOne_notfound() {
            
            var r=new Mock<IStockReader>();
            r.Setup(x=>x.GetOneAsync("MSFT")).ReturnsAsync((StockView?)null);
            
            var c=new StocksController(r.Object);
            var result=await c.GetOne("MSFT");
            result.Result.Should().BeOfType<NotFoundResult>();
        }
    }
}