using Xunit;
using Moq;
using TaxCalculator.Models;
using TaxCalculator.Services;
using Assert = Xunit.Assert;

namespace TaxCalculator.Tests
{
    public class CachingCalculatorDecoratorTests
    {
        private CachingCalculatorDecorator _decorator;
        private Mock<ITaxCalculatorComponent> _mockInnerCalculator;

        public CachingCalculatorDecoratorTests()
        {
            _mockInnerCalculator = new Mock<ITaxCalculatorComponent>();
            _decorator = new CachingCalculatorDecorator(_mockInnerCalculator.Object);
        }

        [Fact]
        public void CalculateTaxes_Returns_Correct_Result()
        {
            // Arrange
            var taxpayer = new TaxPayer
            {
                FullName = "Ivan Ivanov",
                SSN = "12345",
                GrossIncome = 2500,
                CharitySpent = 150
            };

            var expectedTaxes = new Taxes
            {
                GrossIncome = taxpayer.GrossIncome,
                CharitySpent = (decimal)taxpayer.CharitySpent,
                IncomeTax = 135,
                SocialTax = 202.5m,
                TotalTax = 337.5m,
                NetIncome = 2162.5m
            };

            _mockInnerCalculator.Setup(c => c.CalculateTaxes(taxpayer)).Returns(expectedTaxes);

            // Act
            var result1 = _decorator.CalculateTaxes(taxpayer); // First calculation
            var result2 = _decorator.CalculateTaxes(taxpayer); // Second calculation should use cache

            // Assert
            Assert.Equal(expectedTaxes.GrossIncome, result1.GrossIncome);
            Assert.Equal(expectedTaxes.CharitySpent, result1.CharitySpent);
           

            Assert.Same(result1, result2); // The same result should be returned from cache.
            _mockInnerCalculator.Verify(c => c.CalculateTaxes(taxpayer), Times.Once); // Inner calculator should be called once.
        }
    }
}
