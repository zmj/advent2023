namespace advent2023;

public class BoatRaces : List<BoatRace>
{
    public BoatRaces(string input)
    {
        using var reader = new StringReader(input);
        var maxRaces = 16;
        while (reader.ReadLine().AsSpan() is { IsEmpty: false } s)
        {
#pragma warning disable CA2014 // Do not use stackalloc in loops
            if (s.StartsWith("Time:"))
            {
                var times = s[5..].Split(' ', stackalloc int[maxRaces]);
                foreach (var t in times)
                {
                    Add(new(Duration: t, RecordDistance: 0));
                }
            }
            else if (s.StartsWith("Distance:"))
            {
                var distances = s[9..].Split(' ', stackalloc int[maxRaces]);
                if (distances.Length != Count) throw new ArgumentException("length mismatch");
                for (int i = 0; i < Count; i++)
                {
                    this[i] = this[i] with { RecordDistance = distances[i] };
                }
            }
            else throw new ArgumentException(s.ToString());
#pragma warning restore CA2014 // Do not use stackalloc in loops
        }


    }
}

public readonly record struct BoatRace(int Duration, int RecordDistance)
{
    public int DistanceTraveled(int accelerateDuration)
    {
        if (accelerateDuration <= 0 || accelerateDuration >= Duration) return 0;
        var speed = accelerateDuration;
        var time = Duration - accelerateDuration;
        return speed * time;
    }

    public IEnumerable<int> WinningAccelerateDurations()
    {
        var bestDuration = (int)Math.Round(Math.Sqrt(Duration));
        for (var t = bestDuration; t >= 0; t--)
        {
            if (DistanceTraveled(t) > RecordDistance)
            {
                yield return t;
            }
        }
        for (var t = bestDuration + 1; t <= Duration; t++)
        {
            if (DistanceTraveled(t) > RecordDistance)
            {
                yield return t;
            }
        }
    }
}
