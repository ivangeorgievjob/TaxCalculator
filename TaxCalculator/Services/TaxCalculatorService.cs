using System.ComponentModel.DataAnnotations;
using TaxCalculator.Models;

namespace TaxCalculator.Services
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        private const decimal TaxFreeAllowance = 1000m;
        private const decimal IncomeTaxRate = 0.10m;
        private const decimal SocialTaxRate = 0.15m;
        private const decimal SocialTaxCap = 3000m;
        private const decimal CharityCapRate = 0.10m;

        public Taxes CalculateTaxes(TaxPayer taxpayer)
        {
            var grossIncome = taxpayer.GrossIncome;
            var charitySpent = taxpayer.CharitySpent ?? 0;

            // Apply charity cap
            if (charitySpent > grossIncome * CharityCapRate)
            {
                charitySpent = grossIncome * CharityCapRate;
            }

            var taxableIncome = grossIncome - charitySpent;
            var excessIncome = taxableIncome > TaxFreeAllowance ? taxableIncome - TaxFreeAllowance : 0;

            var incomeTax = IncomeTaxRate * excessIncome;

            var socialTaxableIncome = Math.Min(excessIncome, SocialTaxCap - TaxFreeAllowance);
            var socialTax = SocialTaxRate * socialTaxableIncome;

            var totalTax = incomeTax + socialTax;
            var netIncome = grossIncome - totalTax;

            return new Taxes
            {
                GrossIncome = grossIncome,
                CharitySpent = taxpayer.CharitySpent ?? 0,
                IncomeTax = incomeTax,
                SocialTax = socialTax,
                TotalTax = totalTax,
                NetIncome = netIncome
            };
        }
    }
}
