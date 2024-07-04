using Xunit;
using Moq;
using TaxCalculator.Services;
using TaxCalculator.Models;
using Assert = Xunit.Assert;

namespace TaxCalculator.Tests
{
    public class CharityCalculatorDecoratorTests
    {
        private readonly CharityCalculatorDecorator _charityCalculator;
        private readonly Mock<ITaxCalculatorComponent> _mockTaxCalculator;

        public CharityCalculatorDecoratorTests()
        {
            _mockTaxCalculator = new Mock<ITaxCalculatorComponent>();
            _charityCalculator = new CharityCalculatorDecorator(_mockTaxCalculator.Object);
        }

        [Fact]
        public void CalculateTaxes_CharitySpentApplied()
        {
            var taxpayer = new TaxPayer
            {
                FullName = "Ivan Ivanov",
                GrossIncome = 2500m,
                CharitySpent = 200m
            };

            _mockTaxCalculator.Setup(tc => tc.CalculateTaxes(It.IsAny<TaxPayer>()))
                .Returns(new Taxes
                {
                    GrossIncome = 2300m,  // 2500 - 200
                    CharitySpent = 200m,
                    IncomeTax = 130m,    // (2300 - 1000) * 0.10
                    SocialTax = 195m,    // (2300 - 1000) * 0.15
                    TotalTax = 325m,     // 130 + 195
                    NetIncome = 1975m    // 2500 - 325
                });

            var taxes = _charityCalculator.CalculateTaxes(taxpayer);

            Assert.Equal(200m, taxes.CharitySpent);
            Assert.Equal(130m, taxes.IncomeTax);
            Assert.Equal(195m, taxes.SocialTax);
            Assert.Equal(325m, taxes.TotalTax);
            Assert.Equal(1975m, taxes.NetIncome);
        }
    }
}
