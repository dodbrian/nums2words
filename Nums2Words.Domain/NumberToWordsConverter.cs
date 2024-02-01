using System.Text;

namespace Nums2Words.Domain;

public static class NumberToWordsConverter
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

    public static string Convert(decimal number)
    {
        var integral = (int)number;
        var fractional = (int)((number - integral) * 100);

        var builder = new StringBuilder();
        if (fractional != 0)
        {
            builder.Append(fractional == 1 ? " cent" : " cents");
            ConvertInternal(fractional, builder);
            builder.Insert(0, " and");
        }

        builder.Insert(0, integral == 1 ? " dollar" : " dollars");
        ConvertInternal(integral, builder);

        return builder.ToString().Trim();
    }

    private static void ConvertInternal(int number, StringBuilder builder)
    {
        if (number == 0m) builder.Insert(0, Map[0]);

        var iteration = 0;
        foreach (var (low, high) in Step(number))
        {
            if (low == 0 && high == 0)
            {
                iteration++;
                continue;
            }

            if (iteration > 0)
            {
                builder.Insert(0, Magnitude[iteration - 1]);
                builder.Insert(0, ' ');
            }

            if (low != 0) AppendWord(low, builder);
            if (high != 0)
            {
                builder.Insert(0, Hundred);
                builder.Insert(0, ' ');
                AppendWord(high, builder);
            }

            iteration++;
        }
    }

    private static void AppendWord(int number, StringBuilder builder)
    {
        if (number < 20)
            builder.Insert(0, Map[number]);
        else
        {
            var low = number % 10;
            if (low != 0)
            {
                builder.Insert(0, Map[low]);
                builder.Insert(0, '-');
            }

            var high = number - low;
            builder.Insert(0, Map[high]);
        }

        builder.Insert(0, ' ');
    }

    private static IEnumerable<(int Low, int High)> Step(int number)
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
