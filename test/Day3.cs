using advent2023;

namespace tests;

public class Day3
{
    [Fact]
    public void AddNumbers()
    {
        var grid = new CharGrid();
        grid.Add("467..114..");
        Assert.Collection(grid.Numbers,
            n => Assert.Equal(new(Row: 0, ColStart: 0, ColEnd: 3), n),
            n => Assert.Equal(new(Row: 0, ColStart: 5, ColEnd: 8), n));
    }

    [Fact]
    public void AddSymbols()
    {
        var grid = new CharGrid();
        grid.Add("...$.*....");
        Assert.Collection(grid.Symbols,
            s => Assert.Equal(new(Row: 0, Col: 3), s),
            s => Assert.Equal(new(Row: 0, Col: 5), s));
    }

    [Fact]
    public void NumberValues()
    {
        var grid = new CharGrid();
        grid.Add(".....+.58.");
        grid.Add("..592.....");
        Assert.Collection(grid.Numbers,
            n => Assert.Equal(58, grid.Value(n)),
            n => Assert.Equal(592, grid.Value(n)));
    }

    [Fact]
    public void Adjacent()
    {
        var grid = new CharGrid();
        grid.Add("467..114..");
        grid.Add("...*......");
        Assert.True(CharGrid.Adjacent(grid.Numbers[0], grid.Symbols[0]));
        Assert.False(CharGrid.Adjacent(grid.Numbers[1], grid.Symbols[0]));
    }

    [Fact]
    public void AdjacentBug()
    {
        var grid = new CharGrid();
        grid.Add("467..114.@");
        Assert.Empty(grid.PartNumbers());
    }

    [Fact]
    public void PartNumbers()
    {
        var grid = new CharGrid();
        grid.Add("617*......");
        grid.Add(".....+.58.");
        grid.Add("..592.....");
        Assert.Collection(grid.PartNumbers(),
            n => Assert.Equal(617, n),
            n => Assert.Equal(592, n));
    }
}
