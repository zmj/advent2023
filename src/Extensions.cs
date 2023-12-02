using System.Numerics;

namespace advent2023;

public static class Extensions
{
    public static T NotNegative<T>(this T number) where T : INumber<T>
    {
        if (number < T.Zero) throw new ArgumentOutOfRangeException(nameof(number));
        return number;
    }

    public static ReadOnlySpan<T> Split<T>(this ReadOnlySpan<char> s, char separator, Span<T> destination)
        where T : ISpanParsable<T>
    {
        Span<Range> slices = stackalloc Range[destination.Length];
        var count = s.Split(slices, separator);
        if (count == slices.Length && s[slices[^1]].Contains(separator)) throw new Exception("destination too small");

        for (int i = 0; i < count; i++)
        {
            destination[i] = T.Parse(s[slices[i]], provider: null);
        }

        return destination[..count];
    }
}