using System.Collections.Immutable;

namespace AdventOfCode2023_Day7;

public class Day7Tests
{
    [Fact]
    public void PuzzleInput_GetScoresOfHand_a40()
    {
        var hands = InputLoader.LoadLineByLine().Select(LineToHand(Card.FromChar)).ToImmutableList();

        var score = GetScoresOf(hands, h => h.HandType);

        score.Should().Be(251927063);
    }

    [Fact]
    public void SmallExample_GetScoresOfHand_6440()
    {
        var hands = SmallExample.AsLines().Select(LineToHand(Card.FromChar)).ToImmutableList();
        
        var score = GetScoresOf(hands, h => h.HandType);

        score.Should().Be(6440);
    }

    [Fact]
    public void PuzzleInput_GetScoresOfHandWithJoker_255632664()
    {
        var hands = InputLoader.LoadLineByLine().Select(LineToHand(Card.FromCharWithJoker)).ToImmutableList();
        
        var score = GetScoresOf(hands, h => h.HandTypeWithJoker);

        score.Should().Be(255632664);
    }

    [Fact]
    public void SmallExample_GetScoresOfHandWithJoker_5905()
    {
        var hands = SmallExample.AsLines().Select(LineToHand(Card.FromCharWithJoker)).ToImmutableList();
        
        var score = GetScoresOf(hands, h => h.HandTypeWithJoker);

        score.Should().Be(5905);
    }

    private static int GetScoresOf(IImmutableList<Hand> hands, Func<Hand, HandType> handTypeSelection)
    {
        var handsByRank = hands.OrderBy(h => handTypeSelection(h).Value).ThenBy(h => h.AsNumberString());

        return handsByRank.Select((hand, i) => hand.Bid * (i + 1)).Sum();
    }

    private static Func<string, Hand> LineToHand(Func<char, Card> cardFactory) => line =>
    {
        var split = line.Split(' ');
        return new Hand(split[0].Select(cardFactory).ToImmutableList(), int.Parse(split[1]));
    };

    private static string SmallExample => 
@"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483";
}