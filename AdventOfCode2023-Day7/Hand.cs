using System.Collections.Immutable;

namespace AdventOfCode2023_Day7;

public sealed record Hand(IImmutableList<Card> Cards, int Bid)
{
    public HandType HandType => ToHandType(this);
    public HandType HandTypeWithJoker => ToHandTypeWithJoker(this);

    private static HandType ToHandTypeWithJoker(Hand hand)
    {
        var jokerIndexes = hand.Cards.ZipWithIndex().Where(tuple => tuple.Item1.Value == 1).Select(tuple => tuple.Item2);

        var permutations = jokerIndexes.Aggregate(new[]{ hand }, (c, i) => c.SelectMany(h => Card.AllCardsButJoker.Select(newC => h.Replace(i, newC))).ToArray());

        return permutations.Select(h => h.HandType).MaxBy(handType => handType.Value)!;
    }

    private Hand Replace(int index, Card card) => this with { Cards = Cards.RemoveAt(index).Insert(index, card) };

    private static HandType ToHandType(Hand hand)
    {
        var grouped = hand.Cards.GroupBy(c => c.Value).OrderByDescending(g => g.Count()).ToImmutableList();

        if (XOfAKind(hand.Cards, 5))
        {
            return HandType.FiveOfAKind();
        }

        if (XOfAKind(hand.Cards, 4))
        {
            return HandType.FourOfAKind();
        }

        if (grouped.First().Count() == 3 && grouped.Skip(1).First().Count() == 2)
        {
            return HandType.FullHouse();
        }

        if (XOfAKind(hand.Cards, 3))
        {
            return HandType.ThreeOfAKind();
        }

        if (grouped.First().Count() == 2 && grouped.Skip(1).First().Count() == 2)
        {
            return HandType.TwoPairs();
        }

        if (XOfAKind(hand.Cards, 2))
        {
            return HandType.OnePair();
        }

        if (XOfAKind(hand.Cards, 1))
        {
            return HandType.HighCard();
        }

        throw new ArgumentException();
    }

    private static bool XOfAKind(IImmutableList<Card> cards, int x) =>
        cards.GroupBy(c => c.Value).OrderByDescending(g => g.Count()).First().Count() == x;

    public string AsNumberString() => Cards.Select(c => c.Value.ToString("00")).CollectToString();
};