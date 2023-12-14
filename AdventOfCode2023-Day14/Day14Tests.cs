namespace AdventOfCode2023_Day14;

public class Day14Tests
{
    [Fact]
    public void Example1AfterTilt_CalculateLoad_136()
    {
        PlatForm.From(Example1AfterTilt).CalculateLoad().Should().Be(136);
    }

    [Fact]
    public void Example1_Tilted_Example1AfterTilt()
    {
        PlatForm.From(Example1).GetTiltedNorth().Should().BeEquivalentTo(PlatForm.From(Example1AfterTilt));
    }

    [Fact]
    public void Example1_GetAfterCycle_Example1()
    {
        PlatForm.From(Example1).GetAfterCycle().Should().BeEquivalentTo(PlatForm.From(AfterOneCycle));
    }

    [Fact]
    public void Example1_TurnLeftTurnRight_Example1()
    {
        PlatForm.From(Example1).TurnLeft().TurnRight().Should().BeEquivalentTo(PlatForm.From(Example1));
    }

    [Fact]
    public void Example1_4TurnLeft_Example1()
    {
        PlatForm.From(Example1).TurnLeft().TurnLeft().TurnLeft().TurnLeft().Should().BeEquivalentTo(PlatForm.From(Example1));
    }

    [Fact]
    public void Example1_2FlippedNorthSouth_Example1()
    {
        PlatForm.From(Example1).FlippedNorthSouth().FlippedNorthSouth().Should().BeEquivalentTo(PlatForm.From(Example1));
    }

    [Fact]
    public void PuzzleInput_TiltedLoad_105623()
    {
        PlatForm.From(InputLoader.LoadText()).GetTiltedNorth().CalculateLoad().Should().Be(105623);
    }

    [Fact]
    public void Example1_GetAfterMrdCycleCalculateLoad_Example1()
    {
        PlatForm.From(Example1).GetAfterCycle(1000000000).CalculateLoad().Should().Be(64);
    }

    [Fact]
    public void PuzzleInput_GetAfterMrdCycleCalculateLoad_Example1()
    {
        PlatForm.From(InputLoader.LoadText()).GetAfterCycle(1000000000).CalculateLoad().Should().Be(1);
    }

    private static string Example1 => 
@"O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....";

    private static string Example1AfterTilt => 
@"OOOO.#.O..
OO..#....#
OO..O##..O
O..#.OO...
........#.
..#....#.#
..O..#.O.O
..O.......
#....###..
#....#....";

    private static string AfterOneCycle => 
@".....#....
....#...O#
...OO##...
.OO#......
.....OOO#.
.O#...O#.#
....O#....
......OOOO
#...O###..
#..OO#....";
}