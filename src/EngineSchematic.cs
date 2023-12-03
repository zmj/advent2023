namespace advent2023;

public class EngineSchematic : CharGrid
{
    public EngineSchematic(string input)
    {
        using var reader = new StringReader(input);
        while (reader.ReadLine() is { } s)
        {
            Add(s);
        }
    }

    public IEnumerable<int> PartNumbers()
    {
        foreach (var n in Numbers)
        {
            if (Symbols.Any(s => Adjacent(n, s)))
            {
                yield return Value(n);
            }
        }
    }

    public readonly record struct Gear(Number One, Number Two);

    public IEnumerable<Gear> Gears()
    {
        var adjacent = new List<Number>();
        foreach (var symbol in Symbols.Where(s => s.Value is '*'))
        {
            adjacent.Clear();
            adjacent.AddRange(Numbers.Where(n => Adjacent(n, symbol)));
            if (adjacent.Count == 2)
            {
                yield return new(adjacent[0], adjacent[1]);
            }
        }
    }

    public IEnumerable<int> GearRatios()
    {
        foreach (var g in Gears())
        {
            yield return Value(g.One) * Value(g.Two);
        }
    }
}
