namespace AdventOfCode2023_Day7;

public sealed record HandType(int Value, string Name)
{
    public static HandType FiveOfAKind() => new(7, "FiveOfAKind");
    public static HandType FourOfAKind() => new(6, "FourOfAKind");
    public static HandType FullHouse() => new(5, "FullHouse");
    public static HandType ThreeOfAKind() => new(4, "ThreeOfAKind");
    public static HandType TwoPairs() => new(3, "TwoPairs");
    public static HandType OnePair() => new(2, "OnePair");
    public static HandType HighCard() => new(1, "HighCard");
};