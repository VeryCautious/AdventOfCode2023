namespace AdventOfCode2023_Day15;

public abstract record HashMapOperation(string Label, int Hash)
{
    public static HashMapOperation From(string text)
    {
        if (text.EndsWith('-'))
        {
            var label = text.TrimEnd('-');
            return new RemovingOperation(label, HashMap.ComputeHash(label));
        }

        var split = text.Split('=');
        return new AddingOperation(split[0], HashMap.ComputeHash(split[0]), int.Parse(split[1]));
    }
}

public sealed record RemovingOperation(string Label, int Hash) : HashMapOperation(Label, Hash);
public sealed record AddingOperation(string Label, int Hash, int FocalLength) : HashMapOperation(Label, Hash);