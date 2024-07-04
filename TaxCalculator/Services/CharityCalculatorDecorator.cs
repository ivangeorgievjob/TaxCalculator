using TaxCalculator;
using TaxCalculator.Models;
using TaxCalculator.Services;

namespace TaxCalculator.Services
{
    public class CharityCalculatorDecorator : ITaxCalculatorComponent
    {
        private readonly ITaxCalculatorComponent _inner;
        private const decimal CharityCapRate = 0.10m;

        public CharityCalculatorDecorator(ITaxCalculatorComponent inner)
        {
            _inner = inner;
        }

        public Taxes CalculateTaxes(TaxPayer taxpayer)
        {
            var grossIncome = taxpayer.GrossIncome;
            var charitySpent = taxpayer.CharitySpent ?? 0;

            // Apply charity cap
            if (charitySpent > grossIncome * CharityCapRate)
            {
                charitySpent = grossIncome * CharityCapRate;
            }

            taxpayer.GrossIncome -= charitySpent;

            var taxes = _inner.CalculateTaxes(taxpayer);
            taxes.CharitySpent = taxpayer.CharitySpent ?? 0;

            return taxes;
        }
    }
}
