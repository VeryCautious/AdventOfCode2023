using System.Collections.Immutable;

namespace AdventOfCode2023_Day12;

public class Day12Tests
{
    [Fact]
    public void SmallExample1_ArrangementCountSummed_21()
    {
        var springRows = SmallExample1.AsLines().Select(SpringRow.From).ToImmutableList();

        var arrangementSum = springRows.Select(r => r.ArrangementCount()).Sum();

        arrangementSum.Should().Be(21);
    }

    [Fact]
    public void PuzzleInput_ArrangementCountSummed_7251()
    {
        var springRows = InputLoader.LoadLineByLine().Select(SpringRow.From).ToImmutableList();

        var arrangementSum = springRows.Select(r => r.ArrangementCount()).Sum();

        arrangementSum.Should().Be(7251);
    }

    [Fact]
    public void Line_ArrangementCount_10()
    {
        var springRow = SpringRow.From("?###???????? 3,2,1");

        var arrangementCount = springRow.ArrangementCount();

        arrangementCount.Should().Be(10);
    }


    private static string SmallExample1 => 
@"???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1";
}