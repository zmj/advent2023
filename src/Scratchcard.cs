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
