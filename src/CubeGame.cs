using System.Diagnostics.CodeAnalysis;

namespace advent2023;

public record CubeGame(int Id, RGB[] Reveals)
{
    public CubeGame(ReadOnlySpan<char> line) : this(Id: -1, Reveals: [])
    {
        var gamePrefix = "Game ";
        if (!line.StartsWith(gamePrefix)) throw new ArgumentException(line.ToString());
        var gameSeparator = line.IndexOf(':').NotNegative();
        var gameId = line[gamePrefix.Length..gameSeparator];
        Id = int.Parse(gameId);
        line = line[(gameSeparator + 1)..];

        var reveals = line.Split(';', stackalloc RGB[16]);
        if (reveals.IsEmpty) throw new ArgumentException(line.ToString());
        Reveals = reveals.ToArray();
    }

    public bool IsPossible(RGB bag)
    {
        foreach (var revealed in Reveals)
        {
            if (revealed.Red > bag.Red
                || revealed.Green > bag.Green
                || revealed.Blue > bag.Blue)
            {
                return false;
            }
        }

        return true;
    }

    public RGB Minimum()
    {
        var (red, green, blue) = (0, 0, 0);
        static int Max(int i, ref int acc) => acc = Math.Max(i, acc);
        foreach (var revealed in Reveals)
        {
            Max(revealed.Red, ref red);
            Max(revealed.Green, ref green);
            Max(revealed.Blue, ref blue);
        }

        return new(red, green, blue);
    }
}

public readonly record struct RGB(int Red, int Green, int Blue) : ISpanParsable<RGB>
{
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out RGB result)
    {
        var colorCounts = s.Split(',', stackalloc ColorCount[3]);
        if (colorCounts.IsEmpty) throw new ArgumentException(s.ToString());

        var (red, green, blue) = (0, 0, 0);
        foreach (var cc in colorCounts)
        {
            _ = cc.Color switch
            {
                Color.red => red = cc.Count,
                Color.green => green = cc.Count,
                Color.blue => blue = cc.Count,
                _ => throw new ArgumentException(s.ToString())
            };
        }

        result = new(red, green, blue);
        return true;
    }

    public long Power() => (long)Red * Green * Blue;
    
    public static RGB Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => TryParse(s, provider, out var value) ? value : throw new FormatException(s.ToString());
    public static RGB Parse(string s, IFormatProvider? provider) => Parse(s.AsSpan(), provider);
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out RGB result) => TryParse(s.AsSpan(), provider, out result);

    public readonly record struct ColorCount(Color Color, int Count) : ISpanParsable<ColorCount>
    {
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out ColorCount result)
        {
            var colors = Enum.GetNames<Color>(); // cached
            foreach (var color in colors)
            {
                if (!s.EndsWith(color)) continue;
                var number = s[..^(color.Length + 1)];
                var count = int.Parse(number);
                result = new(Enum.Parse<Color>(color), count);
                return true;
            }

            result = default;
            return false;
        }

        public static ColorCount Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => TryParse(s, provider, out var value) ? value : throw new FormatException(s.ToString());
        public static ColorCount Parse(string s, IFormatProvider? provider) => Parse(s.AsSpan(), provider);
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ColorCount result) => TryParse(s.AsSpan(), provider, out result);
    }
}

public enum Color
{
    Default = 0,
    red,
    green,
    blue,
}