using System.Collections.Immutable;

namespace AdventOfCode2023_Day8;

public class Day8Tests
{
    [Fact]
    public void SmallExample_WalkCount_2()
    {
        var (directions, map) = Parse(SmallExample.AsLines());

        var stepsToEnd = map.Walk(directions).Count();

        stepsToEnd.Should().Be(2);
    }

    [Fact]
    public void SmallExample2_WalkCount_6()
    {
        var (directions, map) = Parse(SmallExample2.AsLines());

        var stepsToEnd = map.Walk(directions).Count();

        stepsToEnd.Should().Be(6);
    }

    [Fact]
    public void PuzzleInput_WalkCount_20777()
    {
        var (directions, map) = Parse(InputLoader.LoadLineByLine());

        var stepsToEnd = map.Walk(directions).Count();

        stepsToEnd.Should().Be(20777);
    }

    [Fact(Skip = "Takes very long")]
    public void PuzzleInput_WalkGhostCount_13289612809129()
    {
        var (directions, map) = Parse(InputLoader.LoadLineByLine());

        var stepsToEnd = map.WalkGhost(directions);

        stepsToEnd.Should().Be(13289612809129);
    }

    [Fact]
    public void PuzzleInput_WalkGhostLoopInfo_Loops()
    {
        var (directions, map) = Parse(InputLoader.LoadLineByLine());

        var loopInfo = map.GetLoopInfoForGhostWalk(directions).First();

        map.Walk(directions, loopInfo.StarNode, loopInfo.StepsToLoop + loopInfo.StepsInLoopToEndPoint).TakeLast(10).Last().Should().Match<Node>(n => n.Id.EndsWith('Z'));

        map.Walk(directions, loopInfo.StarNode, loopInfo.StepsToLoop + loopInfo.StepsInLoopToEndPoint + loopInfo.LoopSize).Last().Should().Match<Node>(n => n.Id.EndsWith('Z'));

        map.Walk(directions, loopInfo.StarNode, loopInfo.StepsToLoop + loopInfo.StepsInLoopToEndPoint + 2*loopInfo.LoopSize).Last().Should().Match<Node>(n => n.Id.EndsWith('Z'));
    }

    [Fact]
    public void Map_StepsUntilRepeatingFrom_Loops()
    {
        var (directions, map) = Parse(InputLoader.LoadLineByLine());
        var startNode = map.GhostStartNodes[0];

        var (loopNode, loopSize, stepsToLoop) = map.StepsUntilRepeatingFrom(directions, startNode);

        map.Walk(directions, startNode, stepsToLoop).Last().Should().Be(loopNode);

        map.Walk(directions, startNode, stepsToLoop + loopSize).Last().Should().Be(loopNode);

        map.Walk(directions, startNode, stepsToLoop + 2 * loopSize).Last().Should().Be(loopNode);
    }

    private static (IImmutableList<Direction>, Map) Parse(IEnumerable<string> lines)
    {
        var array = lines as string[] ?? lines.ToArray();
        var directions = array.First().Select(Direction.FromChar).ToImmutableList();
        var map = Map.FromLines(array.Skip(2));
        return (directions, map);
    }

    private static string SmallExample => 
@"RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)";

    private static string SmallExample2 => 
@"LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)";

}