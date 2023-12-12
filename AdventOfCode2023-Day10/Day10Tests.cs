namespace AdventOfCode2023_Day10;

public class Day10Tests
{
    [Fact]
    public void Maze1Example_From_Maze()
    {
        var maze = PipeMaze.From(Maze1Example);

        maze.Connections.Count.Should().Be(8);
        maze.Connections.Values.Should().AllSatisfy(connections => connections.Should().HaveCount(2));
    }

    [Fact]
    public void Maze1Example_GetCircle_Circle()
    {
        var maze = PipeMaze.From(Maze1Example);

        var circlePath = maze.Circle.ToArray();

        circlePath.Should().HaveCount(9);
        circlePath.First().Should().Be(maze.StartPipe);
        circlePath.Last().Should().Be(maze.StartPipe);
    }

    [Fact]
    public void Maze2Example_GetStepsToFarthest_8()
    {
        var circlePath = PipeMaze.From(Maze2Example).Circle.ToArray();

        var stepsToFarthest = circlePath.Length / 2;

        stepsToFarthest.Should().Be(8);
    }

    [Fact]
    public void PuzzleInput_GetStepsToFarthest_6717()
    {
        var circlePath = PipeMaze.From(InputLoader.LoadText()).Circle.ToArray();

        var stepsToFarthest = circlePath.Length / 2;

        stepsToFarthest.Should().Be(6717);
    }

    [Fact]
    public void Maze3Example_Traverse_44()
    {
        var inTheLoop = PipeMaze.From(Maze3Example).Traverse((2, 2));

        inTheLoop.Should().Be(4);
    }

    [Fact]
    public void Maze4Example_Traverse_44()
    {
        var inTheLoop = PipeMaze.From(Maze4Example).Traverse((8, 5));

        inTheLoop.Should().Be(8);
    }

    [Fact]
    public void PuzzleInput_Traverse_381()
    {
        var inTheLoop = PipeMaze.From(InputLoader.LoadText()).Traverse((70, 70));

        inTheLoop.Should().Be(381);
    }


    private static string Maze1Example => 
@".....
.S-7.
.|.|.
.L-J.
.....";

    private static string Maze2Example => 
@"..F7.
.FJ|.
SJ.L7
|F--J
LJ...";

    private static string Maze3Example => 
@"..........
.S------7.
.|F----7|.
.||....||.
.||....||.
.|L-7F-J|.
.|..||..|.
.L--JL--J.
..........";

    private static string Maze4Example => 
@".F----7F7F7F7F-7....
.|F--7||||||||FJ....
.||.FJ||||||||L7....
FJL7L7LJLJ||LJ.L-7..
L--J.L7...LJS7F-7L7.
....F-J..F7FJ|L7L7L7
....L7.F7||L7|.L7L7|
.....|FJLJ|FJ|F7|.LJ
....FJL-7.||.||||...
....L---J.LJ.LJLJ...";
}