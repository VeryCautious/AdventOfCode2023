namespace AdventOfCode_Day5;

public sealed record VRange(ulong Lower, ulong Upper)
{
    public bool Contains(VRange range) => Lower <= range.Lower && Upper >= range.Upper;
    public bool Contains(ulong value) => Lower <= value && Upper >= value;
    public bool Overlaps(VRange range) => 
        Contains(range) || range.Contains(this) ||
        range.Contains(Lower) || range.Contains(Upper) ||
        Contains(range.Lower) || Contains(range.Upper);
    public static VRange FromCount(ulong lower, ulong count) => new(lower, lower + count - 1);
}