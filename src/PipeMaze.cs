namespace advent2023;

public class PipeMaze
{
    public List<Tile[]> Rows { get; } = [];
    public Position Start { get; }
    public ref Tile this[Position p] => ref Rows[p.Row][p.Col];
    public bool Contains(Position p) => p.Row >= 0 && p.Col >= 0 && Rows.Count > p.Row && Rows[p.Row].Length > p.Col;
    public readonly record struct Position(int Row, int Col)
    {
        public Position Add(Move m)
        {
            var (row, col) = this;
            (m.Row ? ref row : ref col) += m.Incr ? 1 : -1;
            return new(row, col);
        }
    }

    public PipeMaze(string input)
    {
        using var reader = new StringReader(input);
        int animalRow = 0;
        int? animalCol = null;
        while (reader.ReadLine().AsSpan() is { IsEmpty: false } s)
        {
            AddRow(s, ref animalCol);
            if (animalCol is null) animalRow++;
        }

        Start = new(animalRow, animalCol!.Value);
        InferStartTile();
    }

    private void AddRow(ReadOnlySpan<char> s, ref int? animalCol)
    {
        var tiles = new Tile[s.Length];
        for (int i = 0; i < s.Length; i++)
        {
            tiles[i] = s[i] switch
            {
                '.' => Tile.Ground,
                '|' => Tile.VerticalPipe,
                '-' => Tile.HorizontalPipe,
                'L' => Tile.LBend,
                'J' => Tile.JBend,
                '7' => Tile._7Bend,
                'F' => Tile.FBend,
                'S' => Tile.Start,
                _ => throw new ArgumentException(s[i].ToString())
            };
            if (tiles[i] == Tile.Start) animalCol ??= i;
        }

        Rows.Add(tiles);
    }

    public void InferStartTile()
    {
        var directions = default(Tile);
        foreach (var move in new[] { Move.North, Move.East, Move.South, Move.West })
        {
            var neighbor = Start.Add(move);
            if (!Contains(neighbor)) continue;
            if (this[neighbor].HasFlag(move.Reverse().Direction))
            {
                directions |= move.Direction;
            }
        }

        this[Start] |= directions;
    }

    [Flags]
    public enum Tile : byte
    {
        Ground = 0,
        North = 4,
        East = 2,
        South = 8,
        West = 1,
        VerticalPipe = North | South,
        HorizontalPipe = East | West,
        LBend = North | East,
        JBend = North | West,
        _7Bend = South | West,
        FBend = South | East,
        Start = 16,
        NESW = North | East | South | West,
    }

    public readonly record struct Move(bool Row, bool Incr)
    {
        public static Move North { get; } = new(Row: true, Incr: false);
        public static Move East { get; } = new(Row: false, Incr: true);
        public static Move South { get; } = new(Row: true, Incr: true);
        public static Move West { get; } = new(Row: false, Incr: false);

        public Tile Direction
        {
            get
            {
                static int Bit(bool b) => b ? 1 : 0;
                var shift = Bit(Row) << 1 | Bit(Incr);
                return (Tile)(1 << shift);
            }
        }

        public Move(Tile direction) : this(default, default)
        {
            this = direction switch
            {
                Tile.North => North,
                Tile.East => East,
                Tile.South => South,
                Tile.West => West,
                _ => throw new ArgumentException(direction.ToString())
            };
        }

        public Move Reverse() => this with { Incr = !Incr };
    }

    public static Move Next(Tile tile, Move previous)
    {
        var incomingDirection = previous.Reverse().Direction;
        var availableDirections = tile & Tile.NESW;
        var outgoingDirection = availableDirections & ~incomingDirection;
        return new(outgoingDirection);
    }

    public IEnumerable<Tile> Loop()
    {
        var position = Start;
        var tile = this[position];
        yield return tile;
        var move = Choose0thMove(tile);
        do
        {
            move = Next(tile, move);
            position = position.Add(move);
            yield return tile = this[position];
        }
        while (position != Start);
    }

    private static Move Choose0thMove(Tile t)
    {
        var moves = new[] { Move.North, Move.East, Move.South, Move.West };
        foreach (var m in moves)
        {
            if (t.HasFlag(m.Reverse().Direction))
            {
                return m;
            }
        }

        throw new Exception();
    }

    public int FarthestDistance() => (Loop().Count() - 1) / 2;
}