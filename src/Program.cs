using advent2023;

var report = new OasisReport(Inputs.Day9);
var sum = 0L;
foreach (var history in report.Histories)
{
    sum += history.ExtrapolatePast();
}

Console.WriteLine(sum);