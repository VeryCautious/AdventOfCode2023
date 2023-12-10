using System.Collections.Immutable;

namespace AdventOfCode2023_Day7;

public sealed record Card(int Value)
{
    public static Card FromChar(char c) => new(c switch
    {
        'A' => 14,
        'K' => 13,
        'Q' => 12,
        'J' => 11,
        'T' => 10,
        _ => (int)char.GetNumericValue(c)
    });

    public static IImmutableList<Card> AllCardsButJoker => 
        "AKQT98765432".Select(FromChar).ToImmutableList();

    public static Card FromCharWithJoker(char c) => c == 'J' ? new Card(1) : FromChar(c);
};