namespace AdventOfCode2023_Day16;

public class Day16Tests
{
    [Fact]
    public void Example1_SimulateCountPowered_46()
    {
        var lightField = new LightField(Example1.CharsBy2dPosition());

        var poweredCount = lightField.Simulate().DistinctBy(beam => beam.Position).Count();

        poweredCount.Should().Be(46);
    }

    [Fact]
    public void PuzzleInput_SimulateCountPowered_7798()
    {
        var lightField = new LightField(InputLoader.LoadText().CharsBy2dPosition());

        var poweredCount = lightField.Simulate().DistinctBy(beam => beam.Position).Count();

        poweredCount.Should().Be(7798);
    }

    [Fact]
    public void Example1_MaxPowered_51()
    {
        var lightField = new LightField(Example1.CharsBy2dPosition());

        var poweredCount = lightField.GetMaximumPowered().DistinctBy(beam => beam.Position).Count();

        poweredCount.Should().Be(51);
    }

    [Fact]
    public void PuzzleInput_MaxPowered_8026()
    {
        var lightField = new LightField(InputLoader.LoadText().CharsBy2dPosition());

        var poweredCount = lightField.GetMaximumPowered().DistinctBy(beam => beam.Position).Count();

        poweredCount.Should().Be(8026);
    }

    private static string Example1 => 
@".|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|....";
}