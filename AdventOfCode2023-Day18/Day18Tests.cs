using System.Collections.Immutable;

namespace AdventOfCode2023_Day18;

public class Day18Tests
{
    [Fact]
    public void Example1_LavaPitCount_62()
    {
        var instructions = Example1.AsLines().Select(DigInstruction.From).ToImmutableList();

        var pit = LavaPit.From(instructions);

        pit.Pit.Should().HaveCount(62);
    }

    [Fact]
    public void Example1_BigLavaPitCount_62()
    {
        var instructions = Example1.AsLines().Select(DigInstruction.From).ToImmutableList();

        var size = BigPitSizer.GetSizeBy(instructions);

        size.Should().Be(62);
    }

    [Fact]
    public void PuzzleInput_BigLavaPitCount_31171()
    {
        var instructions = InputLoader.LoadLineByLine().Select(DigInstruction.From).ToImmutableList();

        var size = BigPitSizer.GetSizeBy(instructions);

        size.Should().Be(31171);
    }

    [Fact]
    public void PuzzleInput_LavaPitCount_31171()
    {
        var instructions = InputLoader.LoadLineByLine().Select(DigInstruction.From).ToImmutableList();

        var pit = LavaPit.From(instructions);

        pit.Pit.Should().HaveCount(31171);
    }

    [Fact]
    public void Instruction_FromSwapped_R461937()
    {
        var instruction = DigInstruction.FromSwapped("R 6 (#70c710)");

        instruction.Direction.Should().Be(new Point2D(461937, 0));
    }

    [Fact]
    public void Example1_BigLavaFromSwappedPitCount_952408144115()
    {
        var instructions = Example1.AsLines().Select(DigInstruction.FromSwapped).ToImmutableList();

        var size = BigPitSizer.GetSizeBy(instructions);

        size.Should().Be(952408144115);
    }

    [Fact]
    public void PuzzleInput_BigLavaFromSwappedPitCount_131431655002266()
    {
        var instructions = InputLoader.LoadLineByLine().Select(DigInstruction.FromSwapped).ToImmutableList();

        var size = BigPitSizer.GetSizeBy(instructions);

        size.Should().Be(131431655002266L);
    }

    private const string Example1 = 
        """
        R 6 (#70c710)
        D 5 (#0dc571)
        L 2 (#5713f0)
        D 2 (#d2c081)
        R 2 (#59c680)
        D 2 (#411b91)
        L 5 (#8ceee2)
        U 2 (#caa173)
        L 1 (#1b58a2)
        U 2 (#caa171)
        R 2 (#7807d2)
        U 3 (#a77fa3)
        L 2 (#015232)
        U 2 (#7a21e3)
        """;
}