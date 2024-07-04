using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaxCalculator.Models;
using TaxCalculator.Services;

namespace TaxCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ITaxCalculatorComponent _taxCalculatorComponent;
        private static readonly Dictionary<string, Taxes> _cache = new Dictionary<string, Taxes>();
        private readonly ILogger<CalculatorController> _logger;
        private readonly ITaxCalculatorComponent _taxCalculator;
        public CalculatorController(ILogger<CalculatorController> logger, ITaxCalculatorComponent taxCalculator)
        {
            _logger = logger;
            _taxCalculator = taxCalculator;
            // Create a chain of decorators
            //var baseCalculator = new BaseTaxCalculator();
            _taxCalculatorComponent = new CharityCalculatorDecorator(_taxCalculator);
        }

        [HttpPost("Calculate")]
        public IActionResult Calculate([FromBody] TaxPayer taxpayer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Received request to calculate taxes for {FullName}", taxpayer.FullName);

            if (_cache.TryGetValue(taxpayer.SSN, out var cachedResult))
            {
                return Ok(cachedResult);
            }

            var netSalaryResponse = _taxCalculator.CalculateTaxes(taxpayer);

            return Ok(netSalaryResponse);
        }
    }
}
