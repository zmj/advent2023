namespace advent2023;

public class Almanac
{
    public readonly record struct Seed(long Value) : ITypedValue<long>;
    public readonly record struct Soil(long Value) : ITypedValue<long>;
    public readonly record struct Fertilizer(long Value) : ITypedValue<long>;
    public readonly record struct Water(long Value) : ITypedValue<long>;
    public readonly record struct Light(long Value) : ITypedValue<long>;
    public readonly record struct Temperature(long Value) : ITypedValue<long>;
    public readonly record struct Humidity(long Value) : ITypedValue<long>;
    public readonly record struct Location(long Value) : ITypedValue<long>;

    public List<Seed> Seeds { get; } = [];

    public Almanac(string input)
    {
        using var reader = new StringReader(input);
        while (reader.ReadLine() is { } s)
        {
            if (Seeds is []) AddSeeds(s);
            else if (s is "") continue;
            else _ = s switch
            {
                "seed-to-soil map:" => Map<Seed, Soil>.Add(ReadMapRows()),
                "soil-to-fertilizer map:" => Map<Soil, Fertilizer>.Add(ReadMapRows()),
                "fertilizer-to-water map:" => Map<Fertilizer, Water>.Add(ReadMapRows()),
                "water-to-light map:" => Map<Water, Light>.Add(ReadMapRows()),
                "light-to-temperature map:" => Map<Light, Temperature>.Add(ReadMapRows()),
                "temperature-to-humidity map:" => Map<Temperature, Humidity>.Add(ReadMapRows()),
                "humidity-to-location map:" => Map<Humidity, Location>.Add(ReadMapRows()),
                _ => throw new ArgumentException(s)
            };
        }

        IEnumerable<string> ReadMapRows()
        {
            string? line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                yield return line;
            }
        }
    }

    public void AddSeeds(ReadOnlySpan<char> line)
    {
        var prefix = "seeds:";
        if (!line.StartsWith(prefix)) throw new ArgumentException(line.ToString());
        line = line[prefix.Length..];

        var seeds = line.Split(' ', stackalloc long[32]);
        foreach (var s in seeds)
        {
            Seeds.Add(new(s));
        }
    }

    public Location Locate(Seed seed)
    {
        var soil = Map<Seed, Soil>.Instance.From(seed);
        var fertilizer = Map<Soil, Fertilizer>.Instance.From(soil);
        var water = Map<Fertilizer, Water>.Instance.From(fertilizer);
        var light = Map<Water, Light>.Instance.From(water);
        var temperature = Map<Light, Temperature>.Instance.From(light);
        var humidity = Map<Temperature, Humidity>.Instance.From(temperature);
        var location = Map<Humidity, Location>.Instance.From(humidity);
        return location;
    }

    public Location LocateMinimum() => Seeds.Select(Locate).MinBy(l => l.Value);

    public class Map<TSource, TDestination>
        where TSource : ITypedValue<long>, new()
        where TDestination : ITypedValue<long>, new()
    {
        public static Map<TSource, TDestination> Instance { get; } = new();

        private readonly List<Range> _ranges = [];
        private readonly record struct Range(TSource SourceStart, TDestination DestinationStart, long Length);

        private class SourceStartComparer : IComparer<Range>
        {
            public static SourceStartComparer Instance { get; } = new();
            public int Compare(Map<TSource, TDestination>.Range x, Map<TSource, TDestination>.Range y) => x.SourceStart.Value.CompareTo(y.SourceStart.Value);
        }

        public TDestination From(TSource source)
        {
            var index = _ranges.BinarySearch(new(source, default!, default), SourceStartComparer.Instance);
            if (index >= 0)
            {
                return _ranges[index].DestinationStart;
            }

            var beforeIndex = ~index - 1;
            if (beforeIndex == -1)
            {
                return new() { Value = source.Value };
            }

            var before = _ranges[~index - 1];
            var offset = source.Value - before.SourceStart.Value;
            if (before.Length > offset)
            {
                return new() { Value = before.DestinationStart.Value + offset };
            }

            return new() { Value = source.Value };
        }

        public void AddRow(ReadOnlySpan<char> line)
        {
            var values = line.Split(' ', stackalloc long[3]);
            var range = new Range
            {
                SourceStart = new() { Value = values[1] },
                DestinationStart = new() { Value = values[0] },
                Length = values[2],
            };
            var index = _ranges.BinarySearch(range, SourceStartComparer.Instance);
            if (index >= 0) throw new Exception();
            _ranges.Insert(~index, range);
        }

        public static object Add(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                Instance.AddRow(line);
            }

            return Instance;
        }
    }
}

public interface ITypedValue<T>
{
    T Value { get; init; }
}