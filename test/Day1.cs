using advent2023;

namespace tests;

public class Day1
{
    [Theory]
    [InlineData("1abc2", 12)]
    [InlineData("pqr3stu8vwx", 38)]
    [InlineData("a1b2c3d4e5f", 15)]
    [InlineData("treb7uchet", 77)]
    public void CalibrateLine(string line, int expected)
    {
        var actual = Reader.Value(line);
        Assert.Equal(expected, actual);
    }

    private const string TestDocument = """
1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet
""";

    [Theory]
    [InlineData(TestDocument, 142)]
    public void CalibrateDocument(string document, int expected)
    {
        var actual = new Reader(document).Sum();
        Assert.Equal(expected, actual);
    }
}