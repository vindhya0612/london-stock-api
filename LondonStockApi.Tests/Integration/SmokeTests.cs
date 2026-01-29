using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace LondonStockApi.Tests.Integration {
    
    public class SmokeTests:IClassFixture<WebApplicationFactory<Program>> {
        
        private readonly WebApplicationFactory<Program> _factory;
        
        public SmokeTests(WebApplicationFactory<Program> factory) {
            
            _factory=factory.WithWebHostBuilder(_=>{});
        
        } 
        
        [Fact]
        public async Task Swagger_available() {
            
            var client=_factory.CreateClient();
            var resp=await client.GetAsync("/swagger/index.html");
            
            Assert.Equal(HttpStatusCode.OK,resp.StatusCode);

        }
    }
}