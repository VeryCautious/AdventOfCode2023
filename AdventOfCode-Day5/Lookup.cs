using System.Collections.Immutable;

namespace AdventOfCode_Day5;

public class Lookup<TS,TD>(IImmutableList<LookupRule<TS,TD>> rules)
{
    public TD Map(TS source) => 
        rules.First(r => r.Applicable(source)).Lookup(source);
}