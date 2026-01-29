using FluentValidation;
using LondonStockApi.Models;
using LondonStockApi.Services;
using LondonStockApi.Validation;

var builder = WebApplication.CreateBuilder(args);

// Controllers 
builder.Services.AddControllers();

// Swagger/OpenAPI 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Validation & Services
builder.Services.AddScoped<IValidator<TradeDto>, TradeDtoValidator>();
builder.Services.AddScoped<ITradeWriter, SqlTradeWriter>();
builder.Services.AddScoped<IStockReader, SqlStockReader>();

var app = builder.Build();

//Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LondonStockApi v1");
    c.RoutePrefix = "swagger"; 
});


app.MapControllers();

app.Run();
