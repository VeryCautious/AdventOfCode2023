using System;
using System.Collections.Immutable;
using System.Text;

namespace AdventOfCode_Day5;

public class Day5Tests
{
    [Fact]
    public void SmallExample_GetLocationsBySeed_SmallestIs35()
    {
        var locationBySeed = GetLocationsBySeed(SmallExample);

        var lowestLocation = locationBySeed.Values.Min();

        lowestLocation.Should().Be(35);
    }

    [Fact]
    public void PuzzleInput_GetLocationsBySeed_SmallestIs35()
    {
        var locationBySeed = GetLocationsBySeed(InputLoader.LoadText());

        var lowestLocation = locationBySeed.Values.Min();

        lowestLocation.Should().Be(196167384);
    }

    private static ImmutableDictionary<ulong, ulong> GetLocationsBySeed(string almanac)
    {
        var chunks = almanac.Replace("\r\n\r\n", "$").Split('$');
        var seedString = chunks[0];
        var lookupChunks = chunks[1..];

        var lookups = lookupChunks.Select(ChuckToLookup).ToImmutableList();
        var seeds = seedString["seeds: ".Length..].Split(' ').Select(ulong.Parse).ToArray();

        var locationBySeed = seeds.ToImmutableDictionary(s => s, s => lookups.Aggregate(s, (v, lookup) => lookup.Map(v)));
        return locationBySeed;
    }

    private static Lookup<ulong, ulong> ChuckToLookup(string chunk) => 
        new(chunk.AsLines().Skip(1).Select(MappingLineToRule).Append(IdLookup).ToImmutableList());

    private static LookupRule<ulong, ulong> IdLookup => new(_ => true, i => i);

    private static LookupRule<ulong, ulong> MappingLineToRule(string line)
    {
        var intValues = line.Split(' ').Select(ulong.Parse).ToArray();
        var destinationRangeStart = intValues[0];
        var sourceRangeStart = intValues[1];
        var rageLength = intValues[2];
        return new LookupRule<ulong, ulong>(
            i => i >= sourceRangeStart && i < sourceRangeStart + rageLength,
            i => i + destinationRangeStart - sourceRangeStart
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