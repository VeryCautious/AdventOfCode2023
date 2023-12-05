namespace AdventOfCode_Day5;

public class LookupRule<TS, TD>(Func<TS,bool> canBeApplied, Func<TS,TD> lookup)
{
    public bool Applicable(TS source) => canBeApplied(source);

    public TD Lookup(TS source) => lookup(source);
}