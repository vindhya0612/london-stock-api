using LondonStockApi.Models;
using LondonStockApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LondonStockApi.Controllers
{
    [ApiController]
    [Route("stocks")]
    public class StocksController : ControllerBase
    {
        private readonly IStockReader _reader;

        public StocksController(IStockReader reader)
        {
            _reader = reader;
        }

        // GET /stocks/{ticker}
        [HttpGet("{ticker}")]
        [ProducesResponseType(typeof(StockView), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StockView>> GetOne([FromRoute] string ticker)
        {
            var stock = await _reader.GetOneAsync(ticker);

            if (stock is null)
                return NotFound();

            return Ok(stock);
        }

        // GET /stocks?page=&pageSize=
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StockView>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 100)
        {
            page = page <= 0 ? 1 : page;
            pageSize = (pageSize <= 0 || pageSize > 500) ? 100 : pageSize;

            var stocks = await _reader.GetAllAsync(page, pageSize);
            return Ok(stocks);
        }

        // GET /stocks/batch?tickers=AAPL,MSFT,...
        [HttpGet("batch")]
        [ProducesResponseType(typeof(IEnumerable<StockView>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBatch([FromQuery] string tickers)
        {
            if (string.IsNullOrWhiteSpace(tickers))
            {
                return BadRequest(new
                {
                    error = "Query parameter 'tickers' is required."
                });
            }

            var tickerList = tickers
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (tickerList.Length == 0)
            {
                return BadRequest(new
                {
                    error = "Provide at least one ticker."
                });
            }

            var result = await _reader.GetBatchAsync(tickerList);
            return Ok(result);
        }
    }
}