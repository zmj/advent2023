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
}
