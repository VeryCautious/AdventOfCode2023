namespace AdventOfCode2023_Utils;

public static class EnumerableExtensions
{
    public static char[][] AsCharArray(this IEnumerable<string> lines) =>
        lines.Select(l => l.ToCharArray()).ToArray();

    public static IEnumerable<(T1,T2)> Cartesian<T1,T2>(this IEnumerable<T1> first, IEnumerable<T2> second) => 
        first.SelectMany(_ => second, (l, r) => (l, r));

    public static IEnumerable<(int,int)> CartesianCoords(int firstDimensionSize, int secondDimensionSize) => 
        Enumerable.Range(0, firstDimensionSize).SelectMany(_ => Enumerable.Range(0, secondDimensionSize), (l, r) => (l, r));

    public static IEnumerable<(T,T)> CartesianSelf<T>(this IEnumerable<T> first)
    {
        var enumerated = first as T[] ?? first.ToArray();
        return Cartesian(enumerated, enumerated);
    }

    public static IEnumerable<(T Value, int Index)> ZipWithIndex<T>(this IEnumerable<T> items)
    {
        var array = items as T[] ?? items.ToArray();
        return array.Zip(Enumerable.Range(0, array.Length));
    }

    public static IEnumerable<(T, T)> SlidingWindow<T>(this IEnumerable<T> enumerable)
    {
        var array = enumerable as T[] ?? enumerable.ToArray();
        return array.Zip(array.Skip(1)).ToArray();
    }
}