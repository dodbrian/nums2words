using Nums2Words.Domain;

namespace Nums2Words.Tests;

public class NumberToWordsConverterTests
{
    [Theory]
    [InlineData(0, "zero dollars")]
    [InlineData(1, "one dollar")]
    [InlineData(25.1, "twenty-five dollars and ten cents")]
    [InlineData(0.01, "zero dollars and one cent")]
    [InlineData(45_100, "forty-five thousand one hundred dollars")]
    [InlineData(
        999_999_999.99,
        "nine hundred ninety-nine million " +
        "nine hundred ninety-nine thousand " +
        "nine hundred ninety-nine dollars " +
        "and " +
        "ninety-nine cents")]
    public void Should_return_correct_wording_for_number(decimal number, string words)
    {
        // Arrange

        // Act
        var result = NumberToWordsConverter.Convert(number);

        // Assert
        result.Should().Be(words);
    }
}
