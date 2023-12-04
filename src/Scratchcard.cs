using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace advent2023;

public class Scratchcard : Dictionary<int, int>
{
    public int Id { get; }

    public IEnumerable<int> WinningNumbers()
    {
        foreach (var (winningNumber, count) in this)
        {
            for (var i = 0; i < count; i++)
            {
                yield return winningNumber;
            }
        }
    }

    public Scratchcard(ReadOnlySpan<char> line)
    {
        var prefix = "Card ";
        if (!line.StartsWith(prefix)) throw new ArgumentException(line.ToString());
        var prefixSeparator = line.IndexOf(':').NotNegative();
        var id = line[prefix.Length..prefixSeparator];
        Id = int.Parse(id);
        line = line[(prefixSeparator + 1)..];

        var numberSeparator = line.IndexOf('|').NotNegative();
        var winningNumbers = line[..numberSeparator];
        foreach (var n in winningNumbers.Split(' ', stackalloc int[16]))
        {
            this[n] = 0;
        }

        var myNumbers = line[(numberSeparator + 1)..];
        foreach (var n in myNumbers.Split(' ', stackalloc int[32]))
        {
            ref var count = ref CollectionsMarshal.GetValueRefOrNullRef(this, n);
            if (!Unsafe.IsNullRef(ref count)) count++;
        }
    }

    public long Score()
    {
        var matches = WinningNumbers().Count();
        return matches switch
        {
            0 => 0,
            var n => (long)Math.Pow(2, n - 1),
        };
    }
}

public class ScratchcardSet
{
    private readonly Dictionary<int, int> _copies = [];
    private int _originalCount;

    public int Total()
    {
        var total = 0;
        foreach (var (cardId, count) in _copies)
        {
            if (cardId <= _originalCount)
            {
                total += count;
            }
        }

        return total + _originalCount;
    }

    public ScratchcardSet(string input)
    {
        using var reader = new StringReader(input);
        while (reader.ReadLine() is { } s)
        {
            var card = new Scratchcard(s);
            if (card.Id != ++_originalCount) throw new ArgumentException($"unexpected card id: {card.Id}");
            Add(card);
        }
    }

    private void Add(Scratchcard card)
    {
        var count = 1 + _copies.GetValueOrDefault(card.Id);
        var matches = card.WinningNumbers().Count();
        for (int i = 1; i <= matches; i++)
        {
            AddCopies(card.Id + i, count);
        }
    }

    private void AddCopies(int cardId, int count) => CollectionsMarshal.GetValueRefOrAddDefault(_copies, cardId, out _) += count;
}