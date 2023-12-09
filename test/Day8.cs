using advent2023;
using static advent2023.DesertMap;
using static advent2023.DesertMapExtensions;

namespace tests;

public class Day8
{
    [Fact]
    public void ParseInstructions()
    {
        var instructions = DesertMap.ParseInstructions("LLR");
        Assert.Collection(instructions.Instructions(),
            i => Assert.Equal(Instruction.Left, i),
            i => Assert.Equal(Instruction.Left, i),
            i => Assert.Equal(Instruction.Right, i));
    }

    [Fact]
    public void ParseNode()
    {
        var node = new MapNode("AAA = (BBB, CCC)");
        Assert.Equal(new("AAA"), node.Label);
        Assert.Equal(new("BBB"), node.Left);
        Assert.Equal(new("CCC"), node.Right);
    }

    [Fact]
    public void FollowInstructions()
    {
        var map = new DesertMap("""
            LLR

            AAA = (BBB, BBB)
            BBB = (AAA, ZZZ)
            ZZZ = (ZZZ, ZZZ)
            """);
        Assert.Equal(6, map.FollowInstructions());
    }

    [Fact]
    public void FollowInstructions_Ghost()
    {
        var map = new DesertMap("""
            LR

            11A = (11B, XXX)
            11B = (XXX, 11Z)
            11Z = (11B, XXX)
            22A = (22B, XXX)
            22B = (22C, 22C)
            22C = (22Z, 22Z)
            22Z = (22B, 22B)
            XXX = (XXX, XXX)
            """, ghost: true);
        Assert.Equal(6, map.FollowInstructions());
    }
}
