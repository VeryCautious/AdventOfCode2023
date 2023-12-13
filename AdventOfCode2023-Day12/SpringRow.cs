using System.Collections.Immutable;
using System.Diagnostics;

namespace AdventOfCode2023_Day12;

public sealed record SpringRow(IImmutableList<SpringState> States, IImmutableList<int> BrokenGroups)
{
    public static SpringRow From(string line)
    {
        var split = line.Split(' ');
        Debug.Assert(split.Length == 2);

        var states = split[0].Select(ToSpringState).ToImmutableList();
        var brokenGroup = split[1].IntegersAsList(',');

        return new SpringRow(states, brokenGroup);
    }

    private bool IsValid()
    {
        var brokenParts = States
            .SplitBy(SpringState.Operational)
            .Where(part => part.Any())
            .Select(part => part.Count())
            .ToArray();

        if (brokenParts.Length != BrokenGroups.Count)
        {
            return false;
        }

        return brokenParts
            .Zip(BrokenGroups)
            .All(t => t.First == t.Second);
    }

    public int ArrangementCount()
    {
        var unknownIndices = States.IndexesOf(SpringState.Unknown).ToArray();

        var all = Enumerable
            .Range(0, 1 << unknownIndices.Length)
            .Select(i => i.GetBits(unknownIndices.Length))
            .Select(bits => FromUnknownAsBits(bits, unknownIndices))
            .ToArray();

        var g = all.GroupBy(r => r.IsValid()).ToArray();

        var valid = all.Where(row => row.IsValid()).ToArray();

        return valid.Length;
    }

    private SpringRow FromUnknownAsBits(bool[] bits, int[] unknownIndices)
    {
        var newState = bits
            .ZipWithIndex()
            .Aggregate(States, (s, t) => s.SetItem(unknownIndices[t.Index], BitToState(t.Value)))
            .ToImmutableList();

        return this with { States = newState };
    }

    private static SpringState BitToState(bool b) => b ? SpringState.Operational : SpringState.Damaged;

    public override string ToString()
    {
        var values = States.Select(s => s switch
        {
            SpringState.Unknown => '?',
            SpringState.Damaged => '#',
            _ => '.'
        });
        var brokenString = string.Join(",", BrokenGroups);
        return $"{values.CollectToString()} {brokenString}";
    }

    private static SpringState ToSpringState(char c) => c switch
    {
        '#' => SpringState.Damaged,
        '.' => SpringState.Operational,
        '?' => SpringState.Unknown,
        _ => throw new ArgumentException()
    };
}