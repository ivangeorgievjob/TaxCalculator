using Xunit;
using TaxCalculator.Models;
using TaxCalculator.Services;
using Assert = Xunit.Assert;
using TaxCalculator.Common;
using Microsoft.Extensions.Options;

namespace TaxCalculator.Tests
{
    public class BaseTaxCalculatorTests
    {
        private readonly BaseTaxCalculator _calculator;
        private readonly ITaxCalculatorComponent _taxCalculator;
        private readonly TaxCalculatorSettings _settings;
        public BaseTaxCalculatorTests()
        {           
            _settings = new TaxCalculatorSettings
            {
                TaxThreshold = 1000,
                IncomeTaxRate = 0.10m,
                SocialContributionRate = 0.15m,
                MaxCharityPercentage = 0.10m
            };
            var options = Options.Create(_settings);

            _taxCalculator = new BaseTaxCalculator(options);
        }

        [Fact]
        public void CalculateTaxes_GrossIncomeAbove1000_IncomeTaxApplied()
        {
            // Arrange
            var taxpayer = new TaxPayer
            {
                FullName = "Ivan Ivanov",
                GrossIncome = 1200,
                CharitySpent = 0
            };

            // Act
            var taxes = _taxCalculator.CalculateTaxes(taxpayer);

            // Calculate expected income tax using settings
            decimal expectedIncomeTax = (taxpayer.GrossIncome - _settings.TaxThreshold) * _settings.IncomeTaxRate;

            // Assert
            Assert.Equal(expectedIncomeTax, taxes.IncomeTax);
        }
    }
}
