using System.Collections.Immutable;

namespace AdventOfCode_Day5;

public class Day5Tests
{
    [Fact]
    public void SmallExample_GetLocationsBySeed_SmallestIs35()
    {
        var locationBySeed = GetLocationsBySeed(SmallExample, SeedsByIndependentValues);

        var lowestLocation = locationBySeed.Values.Min(rs => rs.Min(r => r.Lower));

        lowestLocation.Should().Be(35);
    }

    [Fact]
    public void PuzzleInput_GetLocationsBySeed_SmallestIs35()
    {
        var locationBySeed = GetLocationsBySeed(InputLoader.LoadText(), SeedsByIndependentValues);

        var lowestLocation = locationBySeed.Values.Min(rs => rs.Min(r => r.Lower));

        lowestLocation.Should().Be(196167384);
    }

    [Fact]
    public void SmallExample_GetLocationsBySeedInPairs_SmallestIs46()
    {
        var locationBySeed = GetLocationsBySeed(SmallExample, SeedsByPairs);

        var lowestLocation = locationBySeed.Values.Min(rs => rs.Min(r => r.Lower));

        lowestLocation.Should().Be(46);
    }

    [Fact]
    public void PuzzleInput_GetLocationsBySeedInPairs_SmallestIs35()
    {
        var locationBySeed = GetLocationsBySeed(InputLoader.LoadText(), SeedsByPairs);

        var lowestLocation = locationBySeed.Values.Min(rs => rs.Min(r => r.Lower));

        lowestLocation.Should().Be(125742456);
    }

    private static ImmutableDictionary<VRange, IImmutableList<VRange>> GetLocationsBySeed(string almanac, Func<IEnumerable<ulong>, IImmutableList<VRange>> seedFactory)
    {
        var chunks = almanac.Replace("\r\n\r\n", "$").Split('$');
        var seedString = chunks[0];
        var lookupChunks = chunks[1..];

        var lookups = lookupChunks.Select(ChuckToLookup).ToImmutableList();
        var seeds = seedFactory(seedString["seeds: ".Length..].Split(' ').Select(ulong.Parse));

        var locationBySeed = seeds.ToImmutableDictionary(
            s => s,
            s => lookups.Aggregate(ImmutableList.Create(s).As<IImmutableList<VRange>>(), (r, lookup) => lookup.Map(lookup.ChunkToFit(r))));
        return locationBySeed;
    }

    private static IImmutableList<VRange> SeedsByIndependentValues(IEnumerable<ulong> values) => 
        values.Select(v => new VRange(v, v)).ToImmutableList();

    private static IImmutableList<VRange> SeedsByPairs(IEnumerable<ulong> values) => 
        values.Chunk(2).Select(c => VRange.FromCount(c[0], c.Last())).ToImmutableList();

    private static RangeLookup ChuckToLookup(string chunk) => 
        new(chunk.AsLines().Skip(1).Select(MappingLineToRule).Append(IdLookup).ToImmutableList());

    private static RangeRule IdLookup => new(new VRange(0, ulong.MaxValue), new VRange(0, ulong.MaxValue));

    private static RangeRule MappingLineToRule(string line)
    {
        var intValues = line.Split(' ').Select(ulong.Parse).ToArray();
        var destinationRangeStart = intValues[0];
        var sourceRangeStart = intValues[1];
        var rageLength = intValues[2];
        return new RangeRule(
            VRange.FromCount(sourceRangeStart, rageLength),
            VRange.FromCount(destinationRangeStart, rageLength)
        );
    }

    private static string SmallExample => 
@"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4";
}