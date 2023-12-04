using advent2023;

namespace tests;

public class Day4
{
    [Fact]
    public void WinningNumbers()
    {
        var card = new Scratchcard("Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83");
        Assert.Collection(card.WinningNumbers(), n => Assert.Equal(84, n));
    }

    [Fact]
    public void Score()
    {
        var card = new Scratchcard("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53");
        Assert.Equal(4, card.WinningNumbers().Count());
        Assert.Equal(8, card.Score());
    }
}
