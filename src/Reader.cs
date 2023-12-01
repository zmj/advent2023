using System.Buffers;

namespace advent2023;

public readonly struct Reader(string document)
{
    private static readonly SearchValues<char> _numbers = SearchValues.Create(Enumerable.Range(0, 10).Select(i => (char)('0' + i)).ToArray());

    public static int Value(ReadOnlySpan<char> line)
    {
        var first = line[line.IndexOfAny(_numbers)];
        var last = line[line.LastIndexOfAny(_numbers)];
        return int.Parse([first, last]);
    }

    public long Sum()
    {
        using var reader = new StringReader(document);
        var sum = 0L;
        while (reader.ReadLine() is { } s)
        {
            sum += Value(s);
        }

        return sum;
    }
}