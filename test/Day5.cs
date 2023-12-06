using advent2023;

namespace tests;

public class Day5
{
    [Fact]
    public void AddSeeds()
    {
        var alm = new Almanac("");
        alm.AddSeeds("seeds: 79 14 55 13", parseSeedRanges: false);
        Assert.Collection(alm.Seeds,
            n => Assert.Equal(new(79), n),
            n => Assert.Equal(new(14), n),
            n => Assert.Equal(new(55), n),
            n => Assert.Equal(new(13), n));
    }

    [Fact]
    public void MapFrom()
    {
        var map = new Almanac.Map<Almanac.Seed, Almanac.Soil>();
        map.AddRow("50 98 2");
        map.AddRow("52 50 47");
        Assert.Equal(new(49), map.From(new(49)));
        Assert.Equal(new(52), map.From(new(50)));
        Assert.Equal(new(97), map.From(new(97)));
        Assert.Equal(new(51), map.From(new(99)));
        Assert.Equal(new(100), map.From(new(100)));
    }

    [Fact]
    public void Locate()
    {
        var almanac = new Almanac(ExampleInput);
        Assert.Equal(new(82), almanac.Locate(new(79)));
        Assert.Equal(new(35), almanac.Locate(new(13)));
    }

    [Fact]
    public void LocateMinimum()
    {
        var almanac = new Almanac(ExampleInput);
        Assert.Equal(new(35), almanac.LocateMinimum());
    }

    [Fact]
    public void LocateMinimum_SeedRanges()
    {
        var almanac = new Almanac(ExampleInput, parseSeedRanges: true);
        Assert.Equal(new(46), almanac.LocateMinimum());
    }

    private const string ExampleInput = """
        seeds: 79 14 55 13

        seed-to-soil map:
        50 98 2
        52 50 48

        soil-to-fertilizer map:
        0 15 37
        37 52 2
        39 0 15

        fertilizer-to-water map:
        49 53 8
        0 11 42
        42 0 7
        57 7 4

        water-to-light map:
        88 18 7
        18 25 70

        light-to-temperature map:
        45 77 23
        81 45 19
        68 64 13

        temperature-to-humidity map:
        0 69 1
        1 0 69

        humidity-to-location map:
        60 56 37
        56 93 4
        """;
}
