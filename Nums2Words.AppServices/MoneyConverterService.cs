using System.Globalization;
using Microsoft.Extensions.Logging;
using Nums2Words.Domain;
using Nums2Words.Dtos;

namespace Nums2Words.AppServices;

public class MoneyConverterService : IMoneyConverterService
{
    private readonly ILogger<MoneyConverterService> _logger;

    public MoneyConverterService(ILogger<MoneyConverterService> logger) => _logger = logger;

    public MoneyConversionResult ConvertMoneyToWords(MoneyConversionModel model)
    {
        // Number format provided in the requirements matches fr-FR culture info.
        var specificCulture = CultureInfo.CreateSpecificCulture("fr-FR");

        if (!decimal.TryParse(model.Amount, specificCulture, out var decimalAmount))
            throw new InvalidOperationException("Provided value cannot be parsed as decimal");

        _logger.LogInformation("Converting value {Amount} to words", decimalAmount);

        var converter = new DollarsToWordsConverter();
        var result = converter.Convert(decimalAmount);

        return new MoneyConversionResult(result);
    }
}
