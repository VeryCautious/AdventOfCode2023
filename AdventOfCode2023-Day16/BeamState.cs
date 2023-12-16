namespace AdventOfCode2023_Utils;

public sealed record BeamState(Direction Direction, Point2D Position)
{
    private const char EmptySpace = '.';
    private const char Mirror1 = '/';
    private const char Mirror2 = '\\';
    private const char Splitter1 = '-';
    private const char Splitter2 = '|';

    public IEnumerable<BeamState> InteractWith(char c)
    {
        return c switch
        {
            EmptySpace => [this with { Position = AsPoint(Direction).Add(Position) }],
            Mirror1 => [new BeamState(ReflectAtMirror1(Direction), AsPoint(ReflectAtMirror1(Direction)).Add(Position))],
            Mirror2 => [new BeamState(ReflectAtMirror2(Direction), AsPoint(ReflectAtMirror2(Direction)).Add(Position))],
            Splitter1 => (Direction is Direction.Right or Direction.Left) ?
                InteractWith(EmptySpace) :
                [
                    new BeamState(Direction.Left, AsPoint(Direction.Left).Add(Position)),
                    new BeamState(Direction.Right, AsPoint(Direction.Right).Add(Position))
                ],
            Splitter2 => (Direction is Direction.Up or Direction.Down) ?
                InteractWith(EmptySpace) :
                [
                    new BeamState(Direction.Up, AsPoint(Direction.Up).Add(Position)),
                    new BeamState(Direction.Down, AsPoint(Direction.Down).Add(Position))
                ],
            _ => throw new InvalidOperationException()
        };
    }

    private static Direction ReflectAtMirror2(Direction direction) => direction switch
    {
        Direction.Right => Direction.Down,
        Direction.Left => Direction.Up,
        Direction.Up => Direction.Left,
        Direction.Down => Direction.Right,
        _ => throw new InvalidOperationException()
    };

    private static Direction ReflectAtMirror1(Direction direction) => direction switch
    {
        Direction.Right => Direction.Up,
        Direction.Left => Direction.Down,
        Direction.Up => Direction.Right,
        Direction.Down => Direction.Left,
        _ => throw new InvalidOperationException()
    };

    private static Point2D AsPoint(Direction direction) => direction switch
    {
        Direction.Right => new Point2D(1, 0),
        Direction.Left => new Point2D(-1, 0),
        Direction.Up => new Point2D(0, -1),
        Direction.Down => new Point2D(0, 1),
        _ => throw new InvalidOperationException()
    };

}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}