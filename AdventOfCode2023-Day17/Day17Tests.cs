namespace AdventOfCode2023_Day17;

public class Day17Tests
{
    [Fact]
    public void Example1_GetShortestPath_102()
    {
        var pf = new PathFinder(Example1.CharsBy2dPosition());

        pf.GetShortestPath().Should().Be(102);
    }

    [Fact]
    public void PuzzleInput_GetShortestPath_1244()
    {
        var pf = new PathFinder(InputLoader.LoadText().CharsBy2dPosition());

        pf.GetShortestPath().Should().Be(1244);
    }

    [Fact]
    public void Example1_GetShortestPathForUltra_94()
    {
        var pf = new PathFinder(Example1.CharsBy2dPosition());

        pf.GetShortestPathForUltra().Should().Be(94);
    }

    [Fact]
    public void PuzzleInput_GetShortestPathForUltra_1367()
    {
        var pf = new PathFinder(InputLoader.LoadText().CharsBy2dPosition());

        pf.GetShortestPathForUltra().Should().Be(1367);
    }

    private static string Example1 => 
@"2413432311323
3215453535623
3255245654254
3446585845452
4546657867536
1438598798454
4457876987766
3637877979653
4654967986887
4564679986453
1224686865563
2546548887735
4322674655533";
}