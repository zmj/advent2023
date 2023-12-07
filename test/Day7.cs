using advent2023;
using static advent2023.CamelCards;

namespace tests;

public class Day7
{
    [Theory]
    [InlineData("AAAAA", HandType.FiveOfAKind)]
    [InlineData("AA8AA", HandType.FourOfAKind)]
    [InlineData("7AAAA", HandType.FourOfAKind)]
    [InlineData("23332", HandType.FullHouse)]
    [InlineData("TTT98", HandType.ThreeOfAKind)]
    [InlineData("7TT9T", HandType.ThreeOfAKind)]
    [InlineData("23432", HandType.TwoPair)]
    [InlineData("A23A4", HandType.OnePair)]
    [InlineData("2A3A4", HandType.OnePair)]
    [InlineData("23A4A", HandType.OnePair)]
    [InlineData("23456", HandType.HighCard)]
    public void HandTypes(string cards, HandType type)
    {
        var hand = new Hand(cards, jokers: false);
        Assert.Equal(type, hand.Type);
    }

    [Fact]
    public void Winnings()
    {
        var cards = new CamelCards("""
            32T3K 765
            T55J5 684
            KK677 28
            KTJJT 220
            QQQJA 483
            """);
        Assert.Equal(6440, cards.Winnings());
    }

    [Theory]
    [InlineData("J5555", HandType.FiveOfAKind)]
    [InlineData("T55J5", HandType.FourOfAKind)]
    [InlineData("2344J", HandType.ThreeOfAKind)]
    [InlineData("2345J", HandType.OnePair)]
    [InlineData("JJJJJ", HandType.FiveOfAKind)]
    [InlineData("JJJ32", HandType.FourOfAKind)]
    [InlineData("JJ332", HandType.FourOfAKind)]
    [InlineData("JJ234", HandType.ThreeOfAKind)]
    [InlineData("J2233", HandType.FullHouse)]
    public void HandTypes_Jokers(string cards, HandType type)
    {
        var hand = new Hand(cards, jokers: true);
        Assert.Equal(type, hand.Type);
    }

    [Fact]
    public void Winnings_Jokers()
    {
        var cards = new CamelCards("""
            32T3K 765
            T55J5 684
            KK677 28
            KTJJT 220
            QQQJA 483
            """, jokers: true);
        Assert.Equal(5905, cards.Winnings());
    }
}
