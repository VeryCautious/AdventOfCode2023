using System.Collections.Immutable;

namespace AdventOfCode2023_Day13;

public sealed record NoteBlock(IImmutableDictionary<Point2D, char> Notes, int Width, int Height)
{
    public static NoteBlock From(string noteBlockText)
    {
        var lines = noteBlockText.AsLines();
        return new NoteBlock(noteBlockText.CharsBy2dPosition(), lines[0].Length, lines.Count);
    }

    private IImmutableList<int> GetVerticalMirrors()
    {
        return Enumerable.Range(0, Width-1).Where(IsMirroredVerticallyAt).Select(i => i + 1).ToImmutableList();
    }

    private IImmutableList<int> GetHorizontalMirrors()
    {
        return Enumerable.Range(0, Height-1).Where(IsMirroredHorizontallyAt).Select(i => i + 1).ToImmutableList();
    }

    private bool IsMirroredHorizontallyAt(int y)
    {
        return Notes
            .Where(t => t.Key.Y <= y)
            .All(t => !Notes.TryGetValue(MirrorHorizontally(t.Key, y), out var refP) || refP == t.Value);
    }

    private bool IsMirroredVerticallyAt(int x)
    {
        return Notes
            .Where(t => t.Key.X <= x)
            .All(t => !Notes.TryGetValue(MirrorVertically(t.Key, x), out var refP) || refP == t.Value);
    }

    public long GetScore() => GetScore([], []);
    private long GetScore(IEnumerable<int> exceptVerticals, IEnumerable<int> exceptHorizontals) => 
        GetVerticalMirrors().Except(exceptVerticals).Sum() + GetHorizontalMirrors().Except(exceptHorizontals).Select(i => i * 100).Sum();

    public static Point2D MirrorVertically(Point2D p, int x) => p with { X =  Math.Abs(p.X - x - 1) + x };
    private static Point2D MirrorHorizontally(Point2D p, int y) => p with { Y =  Math.Abs(p.Y - y - 1) + y };

    public long GetSmudgedScore()
    {
        return GetSmudges()
            .Select(nb => nb.GetScore(GetVerticalMirrors(), GetHorizontalMirrors()))
            .First(s => s > 0);
    }

    private IEnumerable<NoteBlock> GetSmudges()
    {
        foreach (var (p, c) in Notes)
        {
            yield return new NoteBlock(Notes.SetItem(p, c == '.' ? '#' : '.'), Width, Height);
        }
    }
}