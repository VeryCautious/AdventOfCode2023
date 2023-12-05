using System.Collections.Immutable;
using System.Diagnostics;

namespace AdventOfCode_Day5;

public class LookupRule<TS, TD>(Func<TS,bool> canBeApplied, Func<TS,TD> lookup)
{
    public bool Applicable(TS source) => canBeApplied(source);

    public TD Lookup(TS source) => lookup(source);
}

public class RangeRule(VRange from, VRange to) : LookupRule<IImmutableList<VRange>, IImmutableList<VRange>>(
    rs => ApplicableToRange(rs, from),
    rs => Map(rs, from, to)
)
{
    public VRange From => from;
    public VRange To => to;

    private static IImmutableList<VRange> Map(IImmutableList<VRange> inp, VRange from, VRange to)
    {
        Debug.Assert(inp.All(from.Contains));
        
        var newRanges = inp
            .Select(r => new VRange(r.Lower + to.Lower - from.Lower, r.Upper + to.Upper - from.Upper))
            .ToImmutableList();

        return newRanges;
    }

    private static bool ApplicableToRange(IImmutableList<VRange> rs, VRange from) => rs.Any(from.Contains);
}