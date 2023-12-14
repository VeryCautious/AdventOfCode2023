using System.Collections.Immutable;

namespace AdventOfCode2023_Day13;

public class Day13Tests
{
    [Fact]
    public void Example1_GetScore_405()
    {
        var blocks = ToNoteBlocks(Example1.AsLines());
            
         var scoreSum = blocks.Select(nb => nb.GetScore()).Sum();

         scoreSum.Should().Be(405);
    }

    [Fact]
    public void PuzzleInput_GetScore_36041()
    {
        var blocks = ToNoteBlocks(InputLoader.LoadLineByLine());

        var scoreSum = blocks.Select(nb => nb.GetScore()).Sum();

        scoreSum.Should().Be(36041);
    }

    [Fact]
    public void Example1_GetSmudgedScore_400()
    {
        var blocks = ToNoteBlocks(Example1.AsLines());
            
        var scoreSum = blocks.Select(nb => nb.GetSmudgedScore()).Sum();

        scoreSum.Should().Be(400);
    }

    [Fact]
    public void PuzzleInput_GetSmudgedScore_35915()
    {
        var blocks = ToNoteBlocks(InputLoader.LoadLineByLine());

        var scoreSum = blocks.Select(nb => nb.GetSmudgedScore()).Sum();

        scoreSum.Should().Be(35915);
    }

    [Fact]
    public void P_MirrorVerticallyPRef()
    {
        NoteBlock.MirrorVertically(new Point2D(2, 0), 5).Should().BeEquivalentTo(new Point2D(9, 0));
        NoteBlock.MirrorVertically(new Point2D(3, 1), 5).Should().BeEquivalentTo(new Point2D(8, 1));
    }

    private static IImmutableList<NoteBlock> ToNoteBlocks(IImmutableList<string> allLines) 
        => allLines
            .SplitBy("")
            .Select(lines => lines.CollectToString("\n"))
            .Select(NoteBlock.From)
            .ToImmutableList();

    private static string Example1 => 
@"#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.

#...##..#
#....#..#
..##..###
#####.##.
#####.##.
..##..###
#....#..#";
}