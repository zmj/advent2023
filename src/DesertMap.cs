using System.Collections;
using System.Runtime.InteropServices;
using static advent2023.DesertMapExtensions;

namespace advent2023;

public class DesertMap
{
    private readonly BitArray _instructions;
    private readonly List<Node> _nodes = [];
    private readonly Dictionary<Label, int> _labels = [];
    private readonly List<Node> _starts = [];

    public DesertMap(string input, bool ghost = false)
    {
        using var reader = new StringReader(input);
        while (reader.ReadLine() is { } s)
        {
            if (_instructions is null) _instructions = ParseInstructions(s);
            else if (s is "") continue;
            else
            {
                var node = new MapNode(s);
                AddMapNode(node, ghost);
            }
        }

        ArgumentNullException.ThrowIfNull(_instructions);
    }

    public readonly record struct Label(char One, char Two, char Three)
    {
        public Label(ReadOnlySpan<char> s) : this(s[0], s[1], s[2]) { }
        public override string ToString() => $"{One}{Two}{Three}";
    }

    public readonly record struct MapNode(Label Label, Label Left, Label Right)
    {
        public MapNode(ReadOnlySpan<char> line) : this(default, default, default)
        {
            if (line is [var n1, var n2, var n3, ' ', '=', ' ', '(', var l1, var l2, var l3, ',', ' ', var r1, var r2, var r3, ')'])
            {
                Label = new(n1, n2, n3);
                Left = new(l1, l2, l3);
                Right = new(r1, r2, r3);
            }
            else throw new ArgumentException(line.ToString());
        }
    }

    private readonly record struct Node(int Left, int Right, bool Terminal);

    private void AddMapNode(MapNode node, bool ghost)
    {
        var left = AddLabel(node.Left);
        var right = AddLabel(node.Right);
        var index = AddLabel(node.Label);
        var terminal = ghost ? node.Label.Three == 'Z' : node.Label.Equals(new("ZZZ"));
        var n = new Node(left, right, terminal);
        _nodes[index] = n;
        var start = ghost ? node.Label.Three == 'A' : node.Label.Equals(new("AAA"));
        if (start) _starts.Add(n);

        int AddLabel(Label l)
        {
            ref var index = ref CollectionsMarshal.GetValueRefOrAddDefault(_labels, l, out var exists);
            if (!exists)
            {
                index = _nodes.Count;
                _nodes.Add(default);
            }

            return index;
        }
    }

    public long FollowInstructions()
    {
        return FollowInstructions(
            CollectionsMarshal.AsSpan(_nodes),
            _instructions.AsSpan(stackalloc bool[_instructions.Count]),
            CollectionsMarshal.AsSpan(_starts));
    }

    private static long FollowInstructions(ReadOnlySpan<Node> nodes, ReadOnlySpan<bool> instructions, Span<Node> current)
    {
        var step = 0L;
        while (true)
        {
            if (current.All(static n => n.Terminal)) return step;
            var instruction = instructions[(int)(step++ % instructions.Length)];
            foreach (ref var node in current)
            {
                node = nodes[instruction ? node.Right : node.Left];
            }
        }
    }

    public static BitArray ParseInstructions(ReadOnlySpan<char> line)
    {
        var bits = new BitArray(line.Length);
        for (int i = 0; i < line.Length; i++)
        {
            bits[i] = line[i] switch
            {
                'L' => false,
                'R' => true,
                var c => throw new ArgumentException($"unexpected instruction: '{c}'")
            };
        }

        return bits;
    }
}

public static class DesertMapExtensions
{
    public enum Instruction { Left, Right }
    public static IEnumerable<Instruction> Instructions(this BitArray instructions)
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            yield return instructions[i] ? Instruction.Right : Instruction.Left;
        }
    }
}