using FluentValidation;
using LondonStockApi.Models;
using LondonStockApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LondonStockApi.Controllers
{
    [ApiController]
    [Route("trades")]
    public class TradesController : ControllerBase
    {
        private readonly IValidator<TradeDto> _validator;
        private readonly ITradeWriter _writer;

        public TradesController(
            IValidator<TradeDto> validator,
            ITradeWriter writer)
        {
            _validator = validator;
            _writer = writer;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TradeDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(e => e.ErrorMessage).ToArray());

                var problem = new ValidationProblemDetails(errors)
                {
                    Title = "One or more validation errors occurred.",
                    Status = StatusCodes.Status400BadRequest
                };

                return ValidationProblem(problem);
            }

            await _writer.IngestAsync(dto);
            return Accepted();
        }
    }
}