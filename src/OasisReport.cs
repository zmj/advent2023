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

        public int ExtrapolateFuture()
        {
            Span<int> newValues = stackalloc int[Values.Length + 2];
            Extrapolate(source: Values, destination: newValues);
            return newValues[^1];
        }

        public int ExtrapolatePast()
        {
            Span<int> newValues = stackalloc int[Values.Length + 2];
            Extrapolate(source: Values, destination: newValues);
            return newValues[0];
        }

        private static void Extrapolate(ReadOnlySpan<int> source, Span<int> destination)
        {
            if (destination.Length < source.Length + 2) throw new ArgumentException("destination too small");
            if (source.Length < 2) throw new ArgumentException("source too small");

            Span<int> differences = stackalloc int[source.Length + 1];
            for (int i = 0; i < source.Length - 1; i++)
            {
                differences[i + 1] = source[i + 1] - source[i];
            }

            if (differences.All(d => d == 0))
            {
                differences[0] = 0;
                differences[^1] = 0;
            }
            else
            {
                Extrapolate(source: differences[1..^1], destination: differences);
            }

            source.CopyTo(destination[1..]);
            destination[0] = source[0] - differences[0];
            destination[^1] = source[^1] + differences[^1];
        }
    }
}
