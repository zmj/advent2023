using advent2023;

var es = new EngineSchematic(Inputs.Day3);
var sum = 0L;
foreach (var gr in es.GearRatios()) 
{ 
    sum += gr;
}

Console.WriteLine(sum);
