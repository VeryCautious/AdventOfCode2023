using System.Collections.Immutable;
using System.Diagnostics;

namespace AdventOfCode_Day5;

public class Lookup<TS,TD>(IImmutableList<LookupRule<TS,TD>> rules)
{
    public virtual TD Map(TS source) => rules.First(r => r.Applicable(source)).Lookup(source);
}

public class RangeLookup(IImmutableList<RangeRule> rules) : Lookup<IImmutableList<VRange>, IImmutableList<VRange>>(Map(rules))
{
    public override IImmutableList<VRange> Map(IImmutableList<VRange> source) => 
        source
            .GroupBy(r => rules.First(rule => rule.Applicable(ImmutableList.Create(r))))
            .SelectMany(group => group.Key.Lookup(group.ToImmutableList()))
            .ToImmutableList();

    private static IImmutableList<LookupRule<IImmutableList<VRange>, IImmutableList<VRange>>> Map(
        IImmutableList<RangeRule> rules)
    {
        var orderedRules = rules.OrderBy(r => r.From.Upper).SkipLast(1).ToArray();
        var overLaps = orderedRules.Where(r => orderedRules.Except(new []{r}).Any(r2 => r.From.Overlaps(r2.From))).ToArray();
        
        Debug.Assert(overLaps.Length == 0);

        return rules.Select(r => (LookupRule<IImmutableList<VRange>, IImmutableList<VRange>>) r).ToImmutableList();
    }

    public IImmutableList<VRange> ChunkToFit(IImmutableList<VRange> ranges)
    {
        var orderedRules = rules.OrderBy(r => r.From.Upper).ToArray();

        var mappedRanges = new List<VRange>();
        var newRanges = new List<VRange>();

        foreach (var rule in orderedRules)
        {
            var overlapping = ranges.Except(mappedRanges).Where(rule.From.Overlaps).ToImmutableList();
            mappedRanges.AddRange(overlapping);

            var mapped = overlapping.SelectMany(r => SplitBy(r, rule.From));

            newRanges.AddRange(mapped);
        }

        Debug.Assert(mappedRanges.Count == ranges.Count);

        return newRanges.ToImmutableList();
    }

    public static IImmutableList<VRange> SplitBy(VRange source, VRange knife)
    {
        if (knife.Contains(source)) return ImmutableList.Create(source);

        var ranges = new List<VRange>
        {
            new(Math.Max(source.Lower, knife.Lower), Math.Min(source.Upper, knife.Upper))
        };

        if (!knife.Contains(source.Lower))
        {
            ranges.Add(source with { Upper = knife.Lower - 1 });
        }

        if (!knife.Contains(source.Upper))
        {
            ranges.Add(source with { Lower = knife.Upper + 1 });
        }

        var ordered = ranges.OrderBy(r => r.Lower).ToArray();
        for (var i = 1; i < ordered.Length; i++)
        {
            Debug.Assert(ordered[i-1].Upper + 1 == ordered[i].Lower);
        }

        return ranges.ToImmutableList();
    }

}