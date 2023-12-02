using advent2023;

namespace tests;

public class Day2
{
    [Fact]
    public void ParseGame()
    {
        var line = "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue";
        var game = new CubeGame(line);
        Assert.Equal(2, game.Id);
        Assert.Collection(game.Reveals,
            r => Assert.Equal(new(Red: 0, Green: 2, Blue: 1), r),
            r => Assert.Equal(new(Red: 1, Green: 3, Blue: 4), r),
            r => Assert.Equal(new(Red: 0, Green: 1, Blue: 1), r));
    }

    [Fact]
    public void ParseColors()
    {
        var s = "2 blue, 1 red, 2 green";
        var cc = s.AsSpan().Split(',', stackalloc RGB.ColorCount[3]).ToArray();
        Assert.Collection(cc,
            r => Assert.Equal(new(Color.blue, 2), r),
            r => Assert.Equal(new(Color.red, 1), r),
            r => Assert.Equal(new(Color.green, 2), r));
    }

    [Fact]
    public void ParseReveal()
    {
        var s = "2 blue, 1 red, 2 green";
        var rgb = RGB.Parse(s, provider: null);
        Assert.Equal(new(Red: 1, Green: 2, Blue: 2), rgb);
    }

    [Fact]
    public void IsPossible()
    {
        var line = "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red";
        var game = new CubeGame(line);
        var bag = new RGB(Red: 12, Green: 13, Blue: 14);
        Assert.False(game.IsPossible(bag));
    }

    [Fact]
    public void Minimum()
    {
        var line = "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red";
        var game = new CubeGame(line);
        var min = game.Minimum();
        Assert.Equal(new(Red: 14, Green: 3, Blue: 15), min);
    }

    [Fact]
    public void Power()
    {
        var rgb = new RGB(Red: 14, Green: 3, Blue: 15);
        var power = rgb.Power();
        Assert.Equal(630, power);
    }
}
