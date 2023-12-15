namespace AdventOfCode2023_Day15;

public class HashMap
{
    private const int BoxCount = 256;
    private readonly Box[] _boxes = Enumerable.Range(0, BoxCount).Select(_ => new Box()).ToArray();

    public void ApplyAll(IEnumerable<HashMapOperation> operations)
    {
        foreach (var operation in operations)
        {
            Apply(operation);
        }
    }

    private void Apply(HashMapOperation operation)
    {
        var target = _boxes[operation.Hash];

        switch (operation)
        {
            case RemovingOperation removingOperation:
                target.Remove(removingOperation.Label);
                break;
            case AddingOperation addingOperation:
                target.Put(addingOperation.Label, addingOperation.FocalLength);
                break;
            default:
                throw new ArgumentException();
        }
    }

    public int FocusingPower => _boxes.Select((b,i) => (i + 1) * b.FocusingPower).Sum();

    public static int ComputeHash(string text) => 
        text.Aggregate(0, (v, c) => ((v + c) * 17) % 256);

    public override string ToString() => 
        _boxes.ZipWithIndex().Where(t => !t.Value.IsEmpty).Select(t => $"Box {t.Index}: {t.Value}").CollectToString("\n");
}