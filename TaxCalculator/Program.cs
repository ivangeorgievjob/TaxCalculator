using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TaxCalculator.Common;
using TaxCalculator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Bind("TaxCalculatorSettings", new TaxCalculatorSettings());
builder.Services.Configure<TaxCalculatorSettings>(builder.Configuration.GetSection("TaxCalculatorSettings"));

// Add services to the container.
builder.Services.AddControllers();
// Register the base service
builder.Services.AddScoped<ITaxCalculatorComponent, BaseTaxCalculator>();

// Register the decorator services
builder.Services.AddScoped<CharityCalculatorDecorator>();
builder.Services.AddScoped<CachingCalculatorDecorator>();

// Register the decorated service (order matters: apply caching first)
builder.Services.AddScoped<ITaxCalculatorComponent>(provider =>
{
    var baseCalculator = provider.GetRequiredService<BaseTaxCalculator>();
    var cachingDecorator = new CachingCalculatorDecorator(baseCalculator);
    var charityDecorator = new CharityCalculatorDecorator(cachingDecorator);
    return charityDecorator;
});

// Register Swagger generator
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tax Calculator API", Version = "v1" });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Enable middleware to serve generated Swagger as a JSON endpoint
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tax Calculator API V1");
    c.RoutePrefix = "swagger"; // URL: /swagger/index.html
});

app.UseAuthorization();

app.MapControllers();

app.Run();
