using TaxCalculator.Models;
using TaxCalculator;

namespace TaxCalculator.Services
{
    public interface ITaxCalculatorComponent
    {
        Taxes CalculateTaxes(TaxPayer taxpayer);
    }
}
