using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace AdventOfCode2023_Day4;

public class Day4Tests
{

    [Fact]
    public void SmallExampleInput_Score_13()
    {
        var cards = SmallExampleInput
            .AsLines()
            .Select(ParseLine)
            .ToImmutableList();

        var scores = cards.Select(Score).ToImmutableList();

        scores.Sum().Should().Be(13);
    }

    [Fact]
    public void PuzzleInput_Score_26914()
    {
        var cards = InputLoader.LoadLineByLine()
            .Select(ParseLine)
            .ToImmutableList();

        var scores = cards.Select(Score).ToImmutableList();

        scores.Sum().Should().Be(26914);
    }

    [Fact]
    public void SmallExampleInput_Play_30Cards()
    {
        var cards = SmallExampleInput
            .AsLines()
            .Select(ParseLine)
            .ToImmutableList();

        var cardGame = PlayWith(cards);

        var cardCount = cardGame.Values.Sum(amount => amount.Count);

        cardCount.Should().Be(30);
    }

    [Fact]
    public void PuzzleInput_Play_13080971Cards()
    {
        var cards = InputLoader.LoadLineByLine()
            .Select(ParseLine)
            .ToImmutableList();

        var cardGame = PlayWith(cards);

        var cardCount = cardGame.Values.Sum(amount => amount.Count);

        cardCount.Should().Be(13080971);
    }

    private static IImmutableDictionary<int, Amount<Card>> PlayWith(ImmutableList<Card> cards)
    {
        IImmutableDictionary<int, Amount<Card>> cardGame = cards
            .ToImmutableDictionary(c => c.Id, c => new Amount<Card>(c, 1));

        return cardGame.Values.Select(c => c.Item.Id).Aggregate(cardGame, Play);
    }

    private static int Score(Card card) => (int)Math.Floor(Math.Pow(2, Matches(card)-1));
    private static int Matches(Card card) => card.YourNumbers.Where(card.WinningNumbers.Contains).Count();

    private static Card ParseLine(string line)
    {
        var match = CardRegex.Match(line);
        if (!match.Success) throw new FormatException();

        var cardId = int.Parse(match.Groups[1].Value);
        var winningNumbers = match.Groups[2].AsIntList(' ');
        var yourNumbers = match.Groups[3].AsIntList(' ');

        return new Card(cardId, winningNumbers, yourNumbers);
    }

    private static IImmutableDictionary<int, Amount<Card>> Play(
        IImmutableDictionary<int, Amount<Card>> cards,
        int cardId
    )
    {
        var (playedCard, count) = cards[cardId];
        var matches = Matches(playedCard);
        var wonIds = Enumerable.Range(cardId + 1, matches);
        var newCards = cards;
        
        foreach (var wonId in wonIds)
        {
            if(!cards.ContainsKey(wonId)) continue;
            var oldCardAmount = newCards[wonId];
            newCards = newCards.SetItem(wonId, oldCardAmount with { Count = oldCardAmount.Count + count });
        }

        return newCards;
    }

    private sealed record Amount<T>(T Item, int Count);

    private sealed record Card(int Id, IImmutableList<int> WinningNumbers, IImmutableList<int> YourNumbers);

    private static Regex CardRegex => new(@"^Card(?:.*?)(\d*): ((?:\d| )*) \| ((?:\d| )*)$");

    private static string SmallExampleInput => 
@"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";
}