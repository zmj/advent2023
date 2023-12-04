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

    [Fact]
    public void TotalCount()
    {
        var set = new ScratchcardSet("""
            Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
            Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
            Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
            Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
            Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
            Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
            """);
        Assert.Equal(30, set.Total());
    }

    [Fact]
    public void TotalOverflow()
    {
        var set = new ScratchcardSet("""
            Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
            Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
            Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
            Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
            """);
        Assert.Equal(15, set.Total());
    }
}
