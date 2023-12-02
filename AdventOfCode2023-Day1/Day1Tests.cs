namespace AdventOfCode2023_Day1;

public class Day1Tests
{

    [Fact]
    public void SmallExampleInput_GetCalibrationFromLine_142()
    {
        var lines = SmallExampleInput.AsLines();

        var result = lines.Select(GetCalibrationFromLine).Sum();

        result.Should().Be(142);
    }

    [Fact]
    public void PuzzleInput_GetCalibrationFromLine_54561()
    {
        var lines = InputLoader.LoadLineByLine();

        var result = lines.Select(GetCalibrationFromLine).Sum();

        result.Should().Be(54561);
    }

    [Fact]
    public void SecondSmallExample_SpelledToNumbers_281()
    {
        var lines = SecondSmallExample.AsLines();

        var result = lines.Select(SpelledToNumbers).Select(GetCalibrationFromLine).Sum();

        result.Should().Be(281);
    }

    [Fact]
    public void PuzzleInput_SpelledToNumbers_54076()
    {
        var lines = InputLoader.LoadLineByLine();

        var result = lines.Select(SpelledToNumbers).Select(GetCalibrationFromLine).Sum();

        result.Should().Be(54076);
    }

    private static string SpelledToNumbers(string line)
    {
        var numbers = Enumerable
            .Range(0, line.Length)
            .Select(i => line[i..])
            .Select(StartsToNumberString);

        return string.Join("", numbers);
    }

    private static string StartsToNumberString(string subLine)
    {
        var lookup = new Dictionary<string, string>
        {
            { "one", "1" },
            { "two", "2" },
            { "three", "3" },
            { "four", "4" },
            { "five", "5" },
            { "six", "6" },
            { "seven", "7" },
            { "eight", "8" },
            { "nine", "9" }
        };

        var matchSpelled = lookup.Keys.SingleOrDefault(subLine.StartsWith);
        if (matchSpelled is not null) return lookup[matchSpelled];

        var matchPlain = lookup.Values.SingleOrDefault(subLine.StartsWith);
        if (matchPlain is not null) return matchPlain;

        return "";
    }

    private static int GetCalibrationFromLine(string line)
    {
        var firstChar = line.First(char.IsDigit);
        var lastChar = line.Last(char.IsDigit);
        return int.Parse(new string(new[] { firstChar, lastChar }));
    }

    private static string SmallExampleInput =>
        @"1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet";

    private static string SecondSmallExample =>
        @"two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen";

}