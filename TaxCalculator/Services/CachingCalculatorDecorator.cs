using System;
using System.Collections.Generic;
using TaxCalculator;
using TaxCalculator.Models;
using TaxCalculator.Services;

public class CachingCalculatorDecorator : ITaxCalculatorComponent
{
    private readonly ITaxCalculatorComponent _inner;
    private readonly Dictionary<string, Taxes> _cache = new Dictionary<string, Taxes>();

    public CachingCalculatorDecorator(ITaxCalculatorComponent inner)
    {
        _inner = inner;
    }

    public Taxes CalculateTaxes(TaxPayer taxpayer)
    {
        var cacheKey = $"{taxpayer.SSN}-{taxpayer.GrossIncome}-{taxpayer.CharitySpent}";
        if (_cache.TryGetValue(cacheKey, out Taxes cachedTaxes))
        {
            return cachedTaxes;
        }

        var calculatedTaxes = _inner.CalculateTaxes(taxpayer);
        _cache[cacheKey] = calculatedTaxes;
        return calculatedTaxes;
    }
}
