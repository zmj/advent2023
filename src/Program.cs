using advent2023;

var races = new BoatRaces(Inputs.Day6);
var margin = 1L;
foreach (var race in races)
{
    margin *= race.WinningAccelerateDurations().Count();
}

Console.WriteLine(margin);
