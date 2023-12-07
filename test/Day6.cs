using advent2023;

namespace tests;

public class Day6
{
    [Fact]
    public void ParseRaces()
    {
        var races = new BoatRaces("""
            Time:      7  15   30
            Distance:  9  40  200
            """);
        Assert.Collection(races,
            r => Assert.Equal(new(7, 9), r),
            r => Assert.Equal(new(15, 40), r),
            r => Assert.Equal(new(30, 200), r));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 6)]
    [InlineData(2, 10)]
    [InlineData(3, 12)]
    [InlineData(4, 12)]
    [InlineData(5, 10)]
    [InlineData(6, 6)]
    [InlineData(7, 0)]
    public void Distance(int accelerateDuration, int expectedDistance)
    {
        var race = new BoatRace(Duration: 7, RecordDistance: 9);
        var distance = race.DistanceTraveled(accelerateDuration);
        Assert.Equal(expectedDistance, distance);
    }

    [Fact]
    public void WinningAccelerations()
    {
        var race = new BoatRace(Duration: 7, RecordDistance: 9);
        List<long> expected = [2, 3, 4, 5];
        var inExpectedOnce = (long t) =>
        {
            Assert.Contains(t, expected);
            expected.Remove(t);
        };
        var expectations = expected.Select(_ => inExpectedOnce).ToArray();
        Assert.Collection(race.WinningAccelerateDurations(), expectations);
    }

    [Fact]
    public void ParseConcat()
    {
        var races = new BoatRaces("""
            Time:      7  15   30
            Distance:  9  40  200
            """, parseConcat: true);
        Assert.Collection(races, r => Assert.Equal(new(71530, 940200), r));
    }
}
