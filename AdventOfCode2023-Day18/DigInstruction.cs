namespace AdventOfCode2023_Day18;

public sealed record DigInstruction(Point2D Direction)
{
    public static DigInstruction From(string line)
    {
        var split = line.Split(' ');
        var steps = int.Parse(split[1]);
        return new DigInstruction(DirectionByChar(split[0][0]).Times(steps));
    }

    public static DigInstruction FromSwapped(string line)
    {
        var split = line.Split(' ');
        var hex = split[2].Trim('(', ')', '#');
        var direction = DirectionByChar(NumberDirectionToChar((int)char.GetNumericValue(hex[5])));
        var hexSteps = hex[..5];
        var steps = Convert.ToInt32(hexSteps, 16);
        return new DigInstruction(direction.Times(steps));
    }

    private static char NumberDirectionToChar(int number) => "RDLU"[number];

    private static Point2D DirectionByChar(char c) => c switch
    {
        'R' => new Point2D(1, 0),
        'L' => new Point2D(-1, 0),
        'U' => new Point2D(0, -1),
        'D' => new Point2D(0, 1),
        _ => throw new ArgumentException()
    };

}