namespace AdventOfCode_Day5;

public class RangeLookupTests
{
    [Fact]
    public void SourceContainingKnife_SplitBy_3Chunks()
    {
        var chunks = RangeLookup.SplitBy(new VRange(10, 100), new VRange(20, 90)).OrderBy(r => r.Lower).ToArray();

        chunks.Should().BeEquivalentTo(new VRange[] { new(10, 19), new(20, 90), new(91, 100) });
    }

    [Fact]
    public void SourceOverlapsKnife_SplitBy_2Chunks()
    {
        var chunks = RangeLookup.SplitBy(new VRange(30, 100), new VRange(20, 90)).OrderBy(r => r.Lower).ToArray();

        chunks.Should().BeEquivalentTo(new VRange[] { new(30, 90), new(91, 100) });
    }

    [Fact]
    public void SourceSameAsKnife_SplitBy_1Chunks()
    {
        var chunks = RangeLookup.SplitBy(new VRange(20, 90), new VRange(20, 90)).OrderBy(r => r.Lower).ToArray();

        chunks.Should().BeEquivalentTo(new VRange[] { new(20, 90) });
    }

    [Fact]
    public void SourceAtEdgeOfKnife_SplitBy_2Chunks()
    {
        var chunks = RangeLookup.SplitBy(new VRange(19, 20), new VRange(20, 90)).OrderBy(r => r.Lower).ToArray();

        chunks.Should().BeEquivalentTo(new VRange[] { new(19, 19), new(20, 20) });
    }

    [Fact]
    public void SourceAtUpperEdgeOfKnife_SplitBy_2Chunks()
    {
        var chunks = RangeLookup.SplitBy(new VRange(19, 20), new VRange(10, 19)).OrderBy(r => r.Lower).ToArray();

        chunks.Should().BeEquivalentTo(new VRange[] { new(19, 19), new(20, 20) });
    }
}