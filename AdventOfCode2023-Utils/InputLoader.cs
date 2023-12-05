using System.Collections.Immutable;

namespace AdventOfCode2023_Utils;

public static class InputLoader
{
    public static IImmutableList<string> LoadLineByLine(string fileName = "PuzzleInput.txt") =>
        LoadText(fileName).AsLines();

    public static string LoadText(string fileName = "PuzzleInput.txt") => File.ReadAllText(fileName);

    
}