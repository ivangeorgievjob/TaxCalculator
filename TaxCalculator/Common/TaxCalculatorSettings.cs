namespace TaxCalculator.Common
{
    public class TaxCalculatorSettings
    {
        public decimal TaxThreshold { get; set; }
        public decimal IncomeTaxRate { get; set; }
        public decimal SocialContributionRate { get; set; }
        public decimal MaxCharityPercentage { get; set; }
    }
}
