using System.Collections.Immutable;

namespace AdventOfCode2023_Day2;

public class Day2Tests
{

    [Fact]
    public void Games_PossibleSummedIds_2439()
    {
        var games = ToGames(InputLoader.LoadLineByLine());

        var summedIds = games.Where(PossibleGame).Sum(game => game.Id);
            
        summedIds.Should().Be(2439);
    }

    [Fact]
    public void Game1_Power_48()
    {
        var games = ToGames("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green".AsLines()).Single();

        var power = Power(games);

        power.Should().Be(48);
    }

    [Fact]
    public void Games_SumPower_63711()
    {
        var games = ToGames(InputLoader.LoadLineByLine());

        var summedPowers = games.Select(Power).Sum();

        summedPowers.Should().Be(63711);
    }

    private static int Power(Game game)
    {
        var maxRed = game.Rounds.MaxBy(round => round.Reds)!.Reds;
        var maxBlue = game.Rounds.MaxBy(round => round.Blues)!.Blues;
        var maxGreen = game.Rounds.MaxBy(round => round.Greens)!.Greens;

        return maxRed * maxGreen * maxBlue;
    }

    private static bool PossibleGame(Game game)
    {
        var impossible = game.Rounds.Any(round => round.Reds > 12) ||
            game.Rounds.Any(round => round.Greens > 13) ||
            game.Rounds.Any(round => round.Blues > 14);

        return !impossible;
    }

    private sealed record Cube(Colors Color);

    private sealed record Round(IImmutableList<Cube> Cubes)
    {
        public int Blues => Cubes.Count(c => c.Color == Colors.Blue);
        public int Reds => Cubes.Count(c => c.Color == Colors.Red);
        public int Greens => Cubes.Count(c => c.Color == Colors.Green);
    };

    private sealed record Game(int Id, IImmutableList<Round> Rounds);

    private static IImmutableList<Game> ToGames(IImmutableList<string> lines) => 
        lines.Select(ParseGame).ToImmutableList();

    private static Game ParseGame(string line)
    {
        var split = line.Split(":");
        var gameId = int.Parse(split[0].Replace("Game ", ""));
        var rounds = split[1]
            .Split(';')
            .Select(ParseRound)
            .ToImmutableList();

        return new Game(gameId, rounds);
    }

    private static Round ParseRound(string text)
    {
        var cubes = text
            .Split(',')
            .Select(s => s.Trim())
            .SelectMany(ParseCube)
            .ToImmutableList();

        return new Round(cubes);
    }

    private static IImmutableList<Cube> ParseCube(string cubeText)
    {
        var split = cubeText.Split(' ');
        var amount = int.Parse(split[0]);
        var color = ToColor(split[1]);
        return Enumerable.Repeat(new Cube(color), amount).ToImmutableList();
    }

    private static Colors ToColor(string colorString) => colorString switch
    {
        "blue" => Colors.Blue,
        "red" => Colors.Red,
        "green" => Colors.Green,
        _ => throw new NotImplementedException()
    };
}