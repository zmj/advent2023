using advent2023;

using var reader = new StringReader(Inputs.Day3);
var grid = new CharGrid();
while (reader.ReadLine() is { } s)
{
    grid.Add(s);
}

var sum = 0L;
foreach (var pn in grid.PartNumbers()) 
{ 
    sum += pn;
}

Console.WriteLine(sum);
