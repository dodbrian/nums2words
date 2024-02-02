using System.Text;

namespace Nums2Words.Domain;

public class NumberToWordsConverter
{
    private static readonly Dictionary<int, string> Map = new()
    {
        { 0, "zero" },
        { 1, "one" },
        { 2, "two" },
        { 3, "three" },
        { 4, "four" },
        { 5, "five" },
        { 6, "six" },
        { 7, "seven" },
        { 8, "eight" },
        { 9, "nine" },
        { 10, "ten" },
        { 11, "eleven" },
        { 12, "twelve" },
        { 13, "thirteen" },
        { 14, "fourteen" },
        { 15, "fifteen" },
        { 16, "sixteen" },
        { 17, "seventeen" },
        { 18, "eighteen" },
        { 19, "nineteen" },
        { 20, "twenty" },
        { 30, "thirty" },
        { 40, "forty" },
        { 50, "fifty" },
        { 60, "sixty" },
        { 70, "seventy" },
        { 80, "eighty" },
        { 90, "ninety" },
    };

    private static readonly string[] Magnitude = { "thousand", "million" };
    private const string Hundred = "hundred";

    private StringBuilder? _builder;

    /// <summary>
    /// Converts a decimal number to its textual representation in words.
    /// </summary>
    /// <param name="number">The decimal number to convert.</param>
    /// <returns>The textual representation of the number in words.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the absolute value of the number is higher than 999,999,999.99.
    /// </exception>
    public string Convert(decimal number)
    {
        var absoluteNumber = Math.Abs(number);

        if (absoluteNumber > 999_999_999.99m)
            throw new InvalidOperationException("The absolute value should not be higher than 999,999,999.99");

        var integral = (int)absoluteNumber;
        var fractional = (int)((absoluteNumber - integral) * 100);

        _builder = new StringBuilder();

        if (fractional != 0)
        {
            _builder.Append(fractional == 1 ? " cent" : " cents");
            ConvertInternal(fractional);
            _builder.Insert(0, " and");
        }

        _builder.Insert(0, integral == 1 ? " dollar" : " dollars");
        ConvertInternal(integral);

        if (number < 0) _builder.Insert(0, "minus");

        return _builder.ToString().Trim();
    }

    /// <summary>
    /// Converts and appends the internal representation of an integer number to the StringBuilder.
    /// </summary>
    /// <param name="number">The number to convert and append.</param>
    /// <exception cref="InvalidOperationException">Thrown if the StringBuilder is not initialized.</exception>
    private void ConvertInternal(int number)
    {
        if (_builder is null) throw new InvalidOperationException("Initialize StringBuilder first");

        if (number == 0m) _builder.Insert(0, Map[0]);

        var iteration = 0;
        foreach (var (low, high) in GetTriplets(number))
        {
            if (low == 0 && high == 0)
            {
                iteration++;
                continue;
            }

            if (iteration > 0)
            {
                _builder.Insert(0, Magnitude[iteration - 1]);
                _builder.Insert(0, ' ');
            }

            if (low != 0) AppendWord(low);
            if (high != 0)
            {
                _builder.Insert(0, Hundred);
                _builder.Insert(0, ' ');
                AppendWord(high);
            }

            iteration++;
        }
    }

    /// <summary>
    /// Appends the word representation of a number to the internal StringBuilder.
    /// </summary>
    /// <param name="number">The number to convert and append.</param>
    /// <exception cref="InvalidOperationException">Thrown if the StringBuilder is not initialized.</exception>
    private void AppendWord(int number)
    {
        if (_builder is null) throw new InvalidOperationException("Initialize StringBuilder first");

        if (number < 20)
            _builder.Insert(0, Map[number]);
        else
        {
            var low = number % 10;
            if (low != 0)
            {
                _builder.Insert(0, Map[low]);
                _builder.Insert(0, '-');
            }

            var high = number - low;
            _builder.Insert(0, Map[high]);
        }

        _builder.Insert(0, ' ');
    }

    /// <summary>
    /// Gets the triplets of a number.
    /// </summary>
    /// <param name="number">The number to extract triplets from.</param>
    /// <returns>An enumerable of tuples representing the triplets.</returns>
    private static IEnumerable<(int Low, int High)> GetTriplets(int number)
    {
        var current = number;

        while (current != 0)
        {
            var low = current % 100;
            current /= 100;

            var high = current % 10;
            current /= 10;

            yield return (low, high);
        }
    }
}
