using advent2023;

using var reader = new StringReader(Inputs.Day4);
var sum = 0L;
while (reader.ReadLine() is { } s)
{
    var card = new Scratchcard(s);
    sum += card.Score();
}

Console.WriteLine(sum);
