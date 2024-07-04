using System.ComponentModel.DataAnnotations;
using TaxCalculator.Models;

namespace TaxCalculator.Services
{
    public interface ITaxCalculatorService
    {
        Taxes CalculateTaxes(TaxPayer taxpayer);
    }
}
