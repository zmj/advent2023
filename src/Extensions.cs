using System.Collections;
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
        var count = s.Split(slices, separator, StringSplitOptions.RemoveEmptyEntries);
        if (count == slices.Length && s[slices[^1]].Contains(separator)) throw new Exception("destination too small");

        for (int i = 0; i < count; i++)
        {
            destination[i] = T.Parse(s[slices[i]], provider: null);
        }

        return destination[..count];
    }

    public static bool All<T>(this ReadOnlySpan<T> span, Func<T, bool> predicate)
    {
        foreach (var element in span)
        {
            if (!predicate(element)) return false;
        }

        return true;
    }

    public static bool All<T>(this Span<T> span, Func<T, bool> predicate) => All((ReadOnlySpan<T>)span, predicate);

    public static ReadOnlySpan<bool> AsSpan(this BitArray bits, Span<bool> destination)
    {
        if (destination.Length < bits.Length) throw new ArgumentException("destination too small");
        for (int i = 0; i < bits.Length; i++)
        {
            destination[i] = bits[i];
        }

        return destination[..bits.Length];
    }
}