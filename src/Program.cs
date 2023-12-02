using advent2023;

using var reader = new StringReader(Inputs.Day2);
var sum = 0L;
while (reader.ReadLine() is { } s)
{
    var game = new CubeGame(s);
    var min = game.Minimum();
    sum += min.Power();
}

Console.WriteLine(sum);
