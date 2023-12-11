using advent2023;
using static advent2023.PipeMaze;

namespace tests;

public class Day10
{
    [Fact]
    public void MoveToDirection()
    {
        Assert.Equal(Tile.North, Move.North.Direction);
        Assert.Equal(Tile.East, Move.East.Direction);
        Assert.Equal(Tile.South, Move.South.Direction);
        Assert.Equal(Tile.West, Move.West.Direction);
    }

    [Fact]
    public void DirectionToMove()
    {
        Assert.Equal(Move.North, new Move(Tile.North));
        Assert.Equal(Move.East, new Move(Tile.East));
        Assert.Equal(Move.South, new Move(Tile.South));
        Assert.Equal(Move.West, new Move(Tile.West));
    }

    [Fact]
    public void LoopSimple()
    {
        var maze = new PipeMaze("""
            -L|F7
            7S-7|
            L|7||
            -L-J|
            L|-JF
            """);
        Assert.Equal(new(1, 1), maze.Start);
        Assert.Collection(maze.Loop(),
            t => Assert.Equal(Tile.Start | Tile.FBend, t),
            t => Assert.Equal(Tile.HorizontalPipe, t),
            t => Assert.Equal(Tile._7Bend, t),
            t => Assert.Equal(Tile.VerticalPipe, t),
            t => Assert.Equal(Tile.JBend, t),
            t => Assert.Equal(Tile.HorizontalPipe, t),
            t => Assert.Equal(Tile.LBend, t),
            t => Assert.Equal(Tile.VerticalPipe, t),
            t => Assert.Equal(Tile.Start | Tile.FBend, t));
        Assert.Equal(4, maze.FarthestDistance());
    }

    [Fact]
    public void LoopComplex()
    {
        var maze = new PipeMaze("""
            7-F7-
            .FJ|7
            SJLL7
            |F--J
            LJ.LJ
            """);
        Assert.Equal(new(2, 0), maze.Start);
        Assert.Equal(Tile.Start| Tile.FBend, maze[maze.Start]);
        Assert.Equal(8, maze.FarthestDistance());
    }
}
