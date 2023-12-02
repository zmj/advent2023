using advent2023;

using var reader = new StringReader(Inputs.Day2);
var bag = new RGB(Red: 12, Green: 13, Blue: 14);
var sum = 0;
while (reader.ReadLine() is { } s)
{
    var game = new CubeGame(s);
    if (game.IsPossible(bag))
    {
        sum += game.Id;
    }
}

Console.WriteLine(sum);
