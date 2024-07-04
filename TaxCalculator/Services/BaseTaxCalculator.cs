// BaseTaxCalculator.cs
using Microsoft.Extensions.Options;
using TaxCalculator.Models;
using TaxCalculator.Services;
using TaxCalculator;
using TaxCalculator.Common;

public class BaseTaxCalculator : ITaxCalculatorComponent
{
    private readonly TaxCalculatorSettings _settings;

   public BaseTaxCalculator(IOptions<TaxCalculatorSettings> settings)
    {
        _settings = settings.Value;
    }

    public Taxes CalculateTaxes(TaxPayer taxpayer)
    {
        Taxes taxes = new Taxes();
        taxes.GrossIncome = taxpayer.GrossIncome;
        taxes.CharitySpent = taxpayer.CharitySpent ?? 0;

        // Check if the gross income is below or equal to the tax threshold
        if (taxpayer.GrossIncome <= _settings.TaxThreshold)
        {
            taxes.IncomeTax = 0;
        }
        else
        {
            // Calculate taxable income
            decimal taxableIncome = taxpayer.GrossIncome - _settings.TaxThreshold;

            // Calculate income tax
            taxes.IncomeTax = taxableIncome * _settings.IncomeTaxRate;
        }

        // Calculate social tax
        decimal socialTaxableIncome = Math.Min(taxpayer.GrossIncome - _settings.TaxThreshold, 2000); // Social contributions only apply up to 2000 IDR
        taxes.SocialTax = socialTaxableIncome * _settings.SocialContributionRate;

        // Calculate total tax
        taxes.TotalTax = taxes.IncomeTax + taxes.SocialTax;

        // Calculate net income
        taxes.NetIncome = taxpayer.GrossIncome - taxes.TotalTax;

        return taxes;
    }
}
