using Nums2Words.Dtos;

namespace Nums2Words.AppServices;

public interface IMoneyConverterService
{
    MoneyConversionResult ConvertMoneyToWords(MoneyConversionModel model);
}
