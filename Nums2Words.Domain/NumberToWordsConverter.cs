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

        var dollars = ConvertInternal(integral);
        var cents = ConvertInternal(fractional);

        return $"{dollars} dollars and {cents} cents";
    }

    private static string ConvertInternal(int number)
    {
        if (number == 0m) return Map[0];

        var builder = new StringBuilder();
        var stack = new Stack<string>();

        var iteration = 0;
        foreach (var (low, high) in Step(number))
        {
            if (low == 0 && high == 0)
            {
                iteration++;
                continue;
            }

            if (iteration > 0) stack.Push(Magnitude[iteration - 1]);

            if (low != 0) AppendWord(low, stack);
            if (high != 0)
            {
                stack.Push(Hundred);
                AppendWord(high, stack);
            }

            iteration++;
        }

        while (stack.Count != 0)
        {
            var word = stack.Pop();
            builder.Append(word);
            builder.Append(' ');
        }

        return builder.ToString().Trim();
    }

    private static void AppendWord(int number, Stack<string> stack)
    {
        if (number < 20)
            stack.Push(Map[number]);
        else
        {
            var low = number % 10;
            if (low != 0) stack.Push(Map[low]);
            var high = number - low;
            stack.Push(Map[high]);
        }
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
