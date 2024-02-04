using Nums2Words.Dtos.Attributes;

namespace Nums2Words.Dtos;

public record MoneyConversionModel([ValidDecimal(Max = 999_999_999.99)] string Amount);
