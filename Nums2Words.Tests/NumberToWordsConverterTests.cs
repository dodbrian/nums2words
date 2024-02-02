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
    [InlineData(-17_811, "minus seventeen thousand eight hundred eleven dollars")]
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
        var converter = new NumberToWordsConverter();

        // Act
        var result = converter.Convert(number);

        // Assert
        result.Should().Be(words);
    }

    [Theory]
    [InlineData(1_000_000_000)]
    [InlineData(-1_000_000_000)]
    public void Should_throw_if_number_is_outside_the_range(decimal number)
    {
        // Arrange
        var converter = new NumberToWordsConverter();

        // Act
        Action act = () => converter.Convert(number);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("The absolute value should not be higher than 999 999 999,99");
    }
}
