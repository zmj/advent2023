namespace advent2023;

public class OasisReport
{
    public List<History> Histories { get; } = [];

    public OasisReport(string input)
    {
        using var reader = new StringReader(input);
        while (reader.ReadLine().AsSpan() is { IsEmpty: false } s)
        {
            Histories.Add(new(s));
        }
    }

    public readonly struct History
    {
        public int[] Values { get; }

        public History(ReadOnlySpan<char> line)
        {
            var values = line.Split(' ', stackalloc int[32]);
            Values = values.ToArray();
        }

        public int Extrapolate()
        {
            Span<int> newValues = stackalloc int[Values.Length + 1];
            Extrapolate(source: Values, destination: newValues);
            return newValues[^1];
        }

        private static void Extrapolate(ReadOnlySpan<int> source, Span<int> destination)
        {
            if (destination.Length < source.Length + 1) throw new ArgumentException("destination too small");
            if (source.Length < 2) throw new ArgumentException("source too small");

            Span<int> differences = stackalloc int[source.Length];
            for (int i = 0; i < source.Length - 1; i++)
            {
                differences[i] = source[i + 1] - source[i];
            }

            if (differences.All(d => d == 0))
            {
                differences[^1] = 0;
            }
            else
            {
                Extrapolate(source: differences[..^1], destination: differences);
            }

            source.CopyTo(destination);
            destination[^1] = source[^1] + differences[^1];
        }
    }
}
