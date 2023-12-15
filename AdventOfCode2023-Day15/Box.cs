namespace AdventOfCode2023_Day15;

public class Box
{
    private readonly IDictionary<string, int> _focalValueByLabel = new Dictionary<string, int>();
    private readonly IList<string> _orderedLabels = new List<string>();

    public void Put(string label, int focalValue)
    {
        _focalValueByLabel[label] = focalValue;
        var index = _orderedLabels.IndexOf(label);

        if (index is -1)
        {
            _orderedLabels.Add(label);
        }
    }

    public void Remove(string label)
    {
        _focalValueByLabel.Remove(label);
        _orderedLabels.Remove(label);
    }

    public int FocusingPower => _orderedLabels.Select((l, i) => (i + 1) * _focalValueByLabel[l]).Sum();

    public bool IsEmpty => _orderedLabels.Count == 0;

    public override string ToString() => 
        _orderedLabels.Select(l => $"[{l} {_focalValueByLabel[l]}]").CollectToString(" ");
}