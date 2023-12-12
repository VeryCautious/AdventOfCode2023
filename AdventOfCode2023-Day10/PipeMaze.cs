using System.Collections.Immutable;
using System.Diagnostics;

namespace AdventOfCode2023_Day10;

public sealed record PipeMaze(IImmutableDictionary<Pipe, IImmutableList<Pipe>> Connections, IImmutableDictionary<(int X, int Y), IImmutableList<(int X, int Y)>> GroundConnections, IImmutableList<Pipe> Circle, int Width, int Height)
{

    public Pipe StartPipe => Connections.Keys.Single(p => p.Type == PipeType.Start);

    public static PipeMaze From(string mazeString)
    {
        var finalConnections = GetPipeConnections(mazeString);
        var circle = GetCircle(finalConnections).ToImmutableList();
        var groundConnections = GetGroundConnections(mazeString, finalConnections, circle);
        var width = mazeString.AsLines()[0].Length;
        var height = mazeString.AsLines().Count;
        var maze = new PipeMaze(finalConnections, groundConnections, circle, width, height);
        var s = maze.PrintAsString(p => groundConnections.ContainsKey(p) ? '.' : 'X');
        return maze;
    }

    public int Traverse((int x, int y) startPoint)
    {
        var visited = new HashSet<(int, int)>();
        var fringeList = new Queue<(int, int)>();
        fringeList.Enqueue((startPoint.x * 2, startPoint.y * 2));

        while (fringeList.Count > 0)
        {
            var ground = fringeList.Dequeue();

            if (!GroundConnections.TryGetValue(ground, out var newGrounds))
            {
                continue;
            }

            foreach (var newGround in newGrounds.Where(p => !visited.Contains(p)))
            {
                visited.Add(newGround);
                fringeList.Enqueue(newGround);
            }
        }

        //var s = PrintAsString(p => p == startPoint ? '%' : visited.Contains(p) ? 'O' : GroundConnections.ContainsKey(p) ? '.' : 'X');

        return visited.Count(p => IsOnPoint(p) && GroundConnections.ContainsKey(p));
    }

    private static IImmutableList<Pipe> GetCircle(IImmutableDictionary<Pipe, IImmutableList<Pipe>> connections)
    {
        var startPipe = connections.Keys.Single(p => p.Type == PipeType.Start);
        var firstPipe = connections[startPipe][0];
        var visitedByPrev = new Dictionary<Pipe, Pipe>() { {firstPipe, startPipe}};
        var fringeList = new Queue<Pipe>();
        fringeList.Enqueue(firstPipe);

        while (fringeList.Count > 0)
        {
            var pipe = fringeList.Dequeue();
            var newPipesToExplore = connections[pipe];

            if (visitedByPrev.Count <= 2)
            {
                newPipesToExplore = newPipesToExplore.Remove(startPipe);
            }

            foreach (var newPipe in newPipesToExplore.Where(p => !visitedByPrev.ContainsKey(p)))
            {
                visitedByPrev.Add(newPipe, pipe);
                fringeList.Enqueue(newPipe);
            }

            if (newPipesToExplore.Contains(startPipe))
            {
                return CollectByPath(visitedByPrev.ToImmutableDictionary(), startPipe).ToImmutableList();
            }
        }

        throw new Exception("No circle found.");
    }

    private string PrintAsString(Func<(int x, int y), char> presenter)
    {
        var chars = Enumerable.Range(0, 2 * Width - 1)
            .Cartesian(Enumerable.Range(0, 2 * Height - 1)).OrderByDescending(p => p.Item2).ThenBy(p => p.Item1)
            .ToArray();

        return string.Join("\n", chars.Select(presenter).Chunk(2*Width-1).Select(cs => new string(cs)));
    }

    private static IEnumerable<Pipe> CollectByPath(IImmutableDictionary<Pipe, Pipe> pathDict, Pipe startPipe)
    {
        yield return startPipe;

        var pipe = pathDict[startPipe];
        do
        {
            yield return pipe;

            pipe = pathDict[pipe];

        } while (pipe != startPipe);

        yield return startPipe;
    }

    private static IImmutableDictionary<(int X, int Y), IImmutableList<(int X, int Y)>> GetGroundConnections(string mazeString, IImmutableDictionary<Pipe, IImmutableList<Pipe>> pipeConnections, IImmutableList<Pipe> circle)
    {
        var pipes = mazeString
            .AsLines()
            .Reverse()
            .SelectMany((line, y) => line.Select((symbol, x) => Pipe.From(x, y, symbol))).Where(p => p is not null)
            .Cast<Pipe>()
            .ToArray();

        var pipesByPosition = pipes.ToDictionary(p => (p.X, p.Y)).ToImmutableDictionary();

        var width = mazeString.AsLines()[0].Length;
        var height = mazeString.AsLines().Count;

        var grid = Enumerable.Range(0, 2*width-1)
            .Cartesian(Enumerable.Range(0, 2*height-1))
            .Where(p => (IsOnPoint(p) && !BlockedByPipe(p, pipesByPosition, circle)) || (!IsOnPoint(p) && !BlockedByPipeConnection(p, pipesByPosition, pipeConnections, circle)))
            .ToHashSet();

        var delta = Enumerable.Range(-1, 3).CartesianSelf().Where(p => Math.Abs(p.Item1) + Math.Abs(p.Item2) == 1).ToArray();

        var groundConnections = grid.ToImmutableDictionary(
            p => p,
            p => (IImmutableList<(int X, int Y)>)delta.Select(d => (p.Item1 + d.Item1, p.Item2 + d.Item2)).Where(grid.Contains).ToImmutableList()
        );

        return groundConnections;
    }

    private static bool BlockedByPipe((int x, int y) p, IImmutableDictionary<(int x, int y), Pipe> pipes, IImmutableList<Pipe> circle)
    {
        if(pipes.TryGetValue((p.Item1/2, p.Item2/2), out var pipe)){
            return circle.Contains(pipe);
        }

        return false;
    }

    private static bool BlockedByPipeConnection((int x, int y) p, IImmutableDictionary<(int x, int y), Pipe> pipes, IImmutableDictionary<Pipe, IImmutableList<Pipe>> pipeConnections, IImmutableList<Pipe> circle)
    {
        if (p.x % 2 == 1 && p.y % 2 == 1) return false;

        if (p.x % 2 == 1)
        {
            if (pipes.TryGetValue(((p.x - 1) / 2, p.y / 2), out var pipe))
            {
                return circle.Contains(pipe) && pipeConnections[pipe].Any(p2 => p2.X == (p.x + 1) / 2 && p2.Y == p.y / 2);
            }
        }

        if (p.y % 2 == 1)
        {
            if (pipes.TryGetValue((p.x / 2, (p.y - 1) / 2), out var pipe))
            {
                return circle.Contains(pipe) && pipeConnections[pipe].Any(p2 => p2.X == p.x / 2 && p2.Y == (p.y + 1) / 2);
            }
        }

        return false;
    }

    private static bool IsOnPoint((int x, int y) p) => p.x % 2 == 0 && p.y % 2 == 0;

    private static IImmutableDictionary<Pipe, IImmutableList<Pipe>> GetPipeConnections(string mazeString)
    {
        var pipes = mazeString
            .AsLines()
            .Reverse()
            .SelectMany((line, y) => line.Select((symbol, x) => Pipe.From(x, y, symbol))).Where(p => p is not null)
            .Cast<Pipe>()
            .ToArray();

        var pipesByPosition = pipes.ToDictionary(p => (p.X, p.Y));

        var connections = pipes.ToDictionary(
            p => p,
            p => p.ConnectedPoints.Where(point => pipesByPosition.ContainsKey(point)).Select(point => pipesByPosition[point])
        );

        FillInStartPipeConnections(pipes, connections, pipesByPosition);

        var finalConnections = connections.ToImmutableDictionary(
            kv => kv.Key,
            kv => (IImmutableList<Pipe>)kv.Value.ToImmutableList()
        );

        return finalConnections;
    }

    private static void FillInStartPipeConnections(Pipe[] pipes, Dictionary<Pipe, IEnumerable<Pipe>> connections, Dictionary<(int X, int Y), Pipe> pipesByPosition)
    {
        var startPipe = pipes.Single(p => p.Type == PipeType.Start);
        var startPipeConnections = pipes.Where(p => p.ConnectedPoints.Contains(startPipe.Position)).ToArray();
        Debug.Assert(startPipeConnections.Length == 2);

        connections[startPipe] = startPipeConnections
            .Select(p => p.Position)
            .Select(pos => pipesByPosition[pos])
            .ToImmutableList();

        connections[startPipeConnections[0]] =
            connections[startPipeConnections[0]].Append(startPipe).Distinct();

        connections[startPipeConnections[^1]] =
            connections[startPipeConnections[^1]].Append(startPipe).Distinct();
    }
}