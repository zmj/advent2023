using static advent2023.OasisReport;

namespace tests;

public class Day9
{
    [Fact]
    public void ParseHistory()
    {
        var h = new History("1 3 6 10 15 21");
        Assert.Collection(h.Values,
            v => Assert.Equal(1, v),
            v => Assert.Equal(3, v),
            v => Assert.Equal(6, v),
            v => Assert.Equal(10, v),
            v => Assert.Equal(15, v),
            v => Assert.Equal(21, v));
    }

    [Theory]
    [InlineData("0 3 6 9 12 15", 18)]
    [InlineData("1 3 6 10 15 21", 28)]
    [InlineData("10 13 16 21 30 45", 68)]
    public void ExtrapolateFuture(string line, int expected)
    {
        var h = new History(line);
        var actual = h.ExtrapolateFuture();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ExtrapolatePast()
    {
        var h = new History("10 13 16 21 30 45");
        Assert.Equal(5, h.ExtrapolatePast());
    }
}
