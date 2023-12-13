using System.Collections;

namespace AdventOfCode2023_Utils;

public static class IntExtensions
{
    public static bool[] GetBits(this int value, int length)
    {
        var b = new BitArray(new[] { value });
        var bits = new bool[b.Count];
        b.CopyTo(bits, 0);
        return bits[..length];
    }
}