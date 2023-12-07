using System.Runtime.CompilerServices;
using static advent2023.CamelCards;

namespace advent2023;

public class CamelCards : SortedList<Hand, int>
{
    public const int HandSize = 5;

    public CamelCards(string input)
    {
        using var reader = new StringReader(input);
        while (reader.ReadLine().AsSpan() is { IsEmpty: false } s)
        {
            var separator = s.IndexOf(' ').NotNegative();
            var hand = new Hand(s[..separator]);
            var bid = int.Parse(s[(separator + 1)..]);
            Add(hand, bid);
        }
    }

    public long Winnings()
    {
        var total = 0L;
        for (int i = 0; i < Count; i++)
        {
            var rank = Count - i;
            var bid = GetValueAtIndex(i);
            total += (long)rank * bid;
        }

        return total;
    }

    [InlineArray(HandSize)]
    private struct Buffer<T>
    {
        public T Element;
    }

    public readonly struct Hand : IComparable<Hand>
    {
        public HandType Type { get; }
        private readonly Buffer<int> _cards;

        public Hand(ReadOnlySpan<char> hand)
        {
            if (hand.Length != HandSize) throw new ArgumentException(hand.ToString());
            for (int i = 0; i < HandSize; i++)
            {
                SetCard(hand[i], ref _cards[i]);
            }

            Type = ChooseType();
        }

        private static void SetCard(char c, ref int card)
        {
            card = c switch
            {
                >= '2' and <= '9' => c - '0',
                'T' => 10,
                'J' => 11,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ => throw new ArgumentException(c.ToString())
            };
        }

        private HandType ChooseType()
        {
            Span<int> counts = stackalloc int[15];
            for (int i = 0; i < HandSize; i++)
            {
                var card = _cards[i];
                counts[card]++;
            }

            counts.Sort();
            return counts switch
            {
                [.., 5] => HandType.FiveOfAKind,
                [.., 4] => HandType.FourOfAKind,
                [.., 2, 3] => HandType.FullHouse,
                [.., 3] => HandType.ThreeOfAKind,
                [.., 2, 2] => HandType.TwoPair,
                [.., 2] => HandType.OnePair,
                _ => HandType.HighCard
            };
        }

        public int CompareTo(Hand other)
        {
            var typeComparison = ((int)Type).CompareTo((int)other.Type);
            if (typeComparison != 0) return typeComparison;

            for (int i = 0; i <HandSize; i++)
            {
                var cardComparison = _cards[i].CompareTo(other._cards[i]);
                if (cardComparison != 0) return -1 * cardComparison;
            }

            return 0;
        }
    }

    public enum HandType
    {
        FiveOfAKind,
        FourOfAKind,
        FullHouse,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        HighCard,
    }
}
