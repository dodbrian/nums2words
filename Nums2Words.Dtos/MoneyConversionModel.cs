using Nums2Words.Dtos.Attributes;

namespace Nums2Words.Dtos;

public record MoneyConversionModel([ValidDecimal] string Amount);
