using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2023_Day17;

public class PathFinder
{
    private readonly ImmutableDictionary<Point2D, PathNode> _nodes;
    private readonly Point2D _endPoint;

    public PathFinder(IImmutableDictionary<Point2D, char> fields)
    {
        _nodes = fields
            .Select(t => new PathNode(t.Key, (int)char.GetNumericValue(t.Value)))
            .ToImmutableDictionary(t => t.Position);
        _endPoint = fields.Select(t => t.Key).MaxBy(p => p.X + p.Y)!;
    }

    private sealed record Passed(PathNode Node, Point2D Direction, int Count, int Cooled);
    
    [SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Local")]
    private sealed record Location(PathNode Node, Point2D Direction, int Count);

    public int GetShortestPath() => GetShortestPath(NextPossibleAfter);
    public int GetShortestPathForUltra() => GetShortestPath(NextPossibleAfterWithUltra);

    private int GetShortestPath(Func<Passed, IEnumerable<Passed>> nextPossibleGenerator)
    {
        var start = new Passed(_nodes[new Point2D(0, 0)], new Point2D(0, 0), 0, 0);
        var fringe = new PriorityQueue<Passed, int>();
        fringe.Enqueue(start, start.Cooled);
        var visited = new HashSet<Location>();

        while (fringe.Count > 0)
        {
            var current = fringe.Dequeue();

            var nextPossible = nextPossibleGenerator(current);
            foreach (var next in nextPossible)
            {
                var location = new Location(next.Node, next.Direction, next.Count);
                if (location.Node.Position == _endPoint)
                {
                    return next.Cooled;
                }
                if (!visited.Add(location))
                {
                    continue;
                }
                fringe.Enqueue(next, next.Cooled);
            }
        }

        return int.MaxValue;
    }

    private IEnumerable<Passed> NextPossibleAfter(Passed current)
    {
        foreach (var neighbourPosition in current.Node.Position.GetDirectNeighbours)
        {
            if (MovingBackwards(current, neighbourPosition))
            {
                continue;
            }

            if (ToManyInSameDirection(current, neighbourPosition))
            {
                continue;
            }

            if (NotOnField(neighbourPosition, out var neighbourNode))
            {
                continue;
            }

            yield return GetPassedBy(current, neighbourNode!);
        }
    }

    private IEnumerable<Passed> NextPossibleAfterWithUltra(Passed current)
    {
        foreach (var neighbourPosition in current.Node.Position.GetDirectNeighbours)
        {
            if (MovingBackwards(current, neighbourPosition))
            {
                continue;
            }

            var forwards = MovingForwards(current, neighbourPosition);
            if (forwards && current.Count < 10 && !NotOnField(neighbourPosition, out var neighbourNode))
            {
                yield return GetPassedBy(current, neighbourNode!);
                continue;
            }

            if (MovingForwards(current, neighbourPosition))
            {
                continue;
            }

            var newDirection = neighbourPosition.Subtract(current.Node.Position);
            var newPosition = current.Node.Position.Add(newDirection.Times(4));

            if (!NotOnField(newPosition, out var node))
            {
                yield return GetPassedBy(current, node!);
            }
        }
    }

    private bool NotOnField(Point2D neighbourPosition, out PathNode? neighbourNode) => 
        !_nodes.TryGetValue(neighbourPosition, out neighbourNode);

    private static bool ToManyInSameDirection(Passed current, Point2D neighbourPosition) => 
        current.Count == 3 && neighbourPosition == current.Node.Position.Add(current.Direction);

    private static bool MovingBackwards(Passed current, Point2D neighbourPosition) => 
        neighbourPosition == current.Node.Position.Subtract(current.Direction);

    private static bool MovingForwards(Passed current, Point2D neighbourPosition) => 
        neighbourPosition == current.Direction.Add(current.Node.Position);

    private Passed GetPassedBy(Passed current, PathNode neighbourNode)
    {
        var direction = neighbourNode.Position.Subtract(current.Node.Position).Normalized();
        Debug.Assert(Math.Abs(direction.Length - 1) < 0.01);
        var nodeBetween = GetNodesBetween(current.Node.Position, neighbourNode.Position).ToArray();
        var newCount = direction == current.Direction ? current.Count + nodeBetween.Length : nodeBetween.Length;
        var cooling = nodeBetween.Sum(n => n.Cooling);
        return new Passed(neighbourNode, direction, newCount, current.Cooled + cooling);
    }

    private IEnumerable<PathNode> GetNodesBetween(Point2D start, Point2D end)
    {
        if (start.X == end.X)
        {
            return Enumerable
                .Range(Math.Min(start.Y, end.Y), Math.Abs(start.Y - end.Y) + 1)
                .Select(y => _nodes[start with { Y = y }]).Except(new []{_nodes[start]});
        }
        else if(start.Y == end.Y)
        {
            return Enumerable
                .Range(Math.Min(start.X, end.X), Math.Abs(start.X - end.X) + 1)
                .Select(x => _nodes[start with { X = x }]).Except(new []{_nodes[start]});
        }
        throw new ArgumentException();
    }

}