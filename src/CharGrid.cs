namespace advent2023;

public class CharGrid
{
    public readonly record struct Number(int Row, int ColStart, int ColEnd);
    public readonly record struct Symbol(int Row, int Col, char Value);

    private readonly List<string> _lines = [];
    public List<Number> Numbers { get; } = [];
    public List<Symbol> Symbols { get; } = [];

    public int Value(Number n) => int.Parse(_lines[n.Row][n.ColStart..n.ColEnd]);

    public void Add(string line)
    {
        var row = _lines.Count;
        int? numStart = null;
        for (int i = 0; i < line.Length; i++)
        {
            var c = line[i];
            switch (TypeOf(c))
            {
                case CharType.period:
                    EndNumber(i);
                    break;
                case CharType.digit:
                    numStart ??= i;
                    break;
                case CharType.symbol:
                    EndNumber(i);
                    Symbols.Add(new(row, i, c));
                    break;
            }
        }

        EndNumber(line.Length);
        _lines.Add(line);

        void EndNumber(int afterEnd)
        {
            if (numStart.HasValue)
            {
                Numbers.Add(new(row, numStart.Value, afterEnd));
                numStart = null;
            }
        }
    }

    private enum CharType { period, digit, symbol };

    private static CharType TypeOf(char c) => c switch
    {
        '.' => CharType.period,
        >= '0' and <= '9' => CharType.digit,
        '*' or '#' or '+' or '$' or '%' or '@' or '&' or '/' or '=' or '-' => CharType.symbol,
        _ => throw new ArgumentException($"unexpected char: {c}")
    };

    public static bool Adjacent(Number n, Symbol s)
    {
        return (s.Row >= n.Row - 1 && s.Row <= n.Row + 1)
            && (s.Col >= n.ColStart - 1 && s.Col <= n.ColEnd);
    }
}
