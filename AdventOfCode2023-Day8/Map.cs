using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace AdventOfCode2023_Day8;

public sealed record Map(IImmutableDictionary<Node, IImmutableList<Node>> Connections)
{
    private readonly Node _startNode = new("AAA");
    private readonly Node _endNode = new("ZZZ");

    private Node Move(Node startNode, Direction direction) => Connections[startNode][direction.Value];

    public static Map FromLines(IEnumerable<string> lines)
    {
        var regex = new Regex(@"^(...).*?=.*?\((...), (...)\)$");

        var dict = lines.ToImmutableDictionary(
            l => new Node(regex.Match(l).Groups[1].Value),
            l => (IImmutableList<Node>)regex.Match(l).Groups.Values.Skip(2).Select(g => new Node(g.Value)).ToImmutableList());

        return new Map(dict);
    }

    public IImmutableList<Node> GhostStartNodes => 
        Connections.Keys.Where(n => n.Id.EndsWith('A')).ToImmutableList();

    public (Node LoopNode, int LoopSize, int StepsToLoop) StepsUntilRepeatingFrom(IImmutableList<Direction> directions, Node startNode)
    {
        var directionQueue = new Queue<Direction>(directions);
        var step = 0;
        var visited = new Dictionary<string, int>();
        var currentNode = startNode;
        var key = $"{currentNode.Id}-{directions.Count - directionQueue.Count}";

        while (!visited.ContainsKey(key))
        {
            visited.Add(key, step);
            if (directionQueue.Count == 0)
            {
                directionQueue = new Queue<Direction>(directions);
            }

            currentNode = Move(currentNode, directionQueue.Dequeue());
            key = $"{currentNode.Id}-{directions.Count - directionQueue.Count}";
            step++;
        }

        var loopSize = step - visited[key];

        return (currentNode, loopSize, visited[key]);
    }

    public sealed record LoopInfo(
        Node StarNode,
        int StepsInLoopToEndPoint,
        int StepsToLoop,
        int LoopSize
    )
    {
        public long StepsAfterLoops(long loopAmount) => StepsToLoop + StepsInLoopToEndPoint + LoopSize * loopAmount;

        public bool CanEqual(long steps, out long loopCount)
        {
            var potLoopCount = (steps - (StepsToLoop + StepsInLoopToEndPoint)) / (double)LoopSize;
            loopCount = 0;
            if (Math.Abs(Math.Round(potLoopCount) - potLoopCount) < 0.000001)
            {
                loopCount = (long)potLoopCount;
                return true;
            }
            return false;
        }
    };

    public IImmutableList<LoopInfo> GetLoopInfoForGhostWalk(IImmutableList<Direction> directions)
    {
        var loopsByNode = GhostStartNodes.ToDictionary(n => n, n => StepsUntilRepeatingFrom(directions, n));
        var loopInfos = new List<LoopInfo>();

        foreach (var (startNode, loop) in loopsByNode)
        {
            var (_, loopSize, stepsToLoop) = loop;

            var loopNodesByStepsInLoop = Walk(directions, startNode, stepsToLoop + loopSize).Skip(stepsToLoop).ZipWithIndex().ToArray();

            var stepsInLoopToEndPoint = loopNodesByStepsInLoop.Where(t => t.Value.Id.EndsWith('Z')).Select(t => t.Index).Single();
            
            loopInfos.Add(new(startNode, stepsInLoopToEndPoint, stepsToLoop, loopSize));
        }

        return loopInfos.ToImmutableList();
    }

    public IEnumerable<Node> Walk(IImmutableList<Direction> directions) => Walk(directions, _startNode, _endNode);

    public IEnumerable<Node> Walk(IImmutableList<Direction> directions, Node startNode, int steps)
    {
        var directionQueue = new Queue<Direction>(directions);
        var currentNode = startNode;

        foreach(var _ in Enumerable.Range(0, steps + 1))
        {
            yield return currentNode;

            if (directionQueue.Count == 0)
            {
                directionQueue = new Queue<Direction>(directions);
            }

            currentNode = Move(currentNode, directionQueue.Dequeue());
        }
    }

    private IEnumerable<Node> Walk(IImmutableList<Direction> directions, Node startNode, Node endNode)
    {
        var directionQueue = new Queue<Direction>(directions);
        var currentNode = startNode;
        do
        {
            yield return currentNode;

            if (directionQueue.Count == 0)
            {
                directionQueue = new Queue<Direction>(directions);
            }

            currentNode = Move(currentNode, directionQueue.Dequeue());
        } while (currentNode != endNode);
    }

    public long WalkGhost(IImmutableList<Direction> directions)
    {
        var infos = GetLoopInfoForGhostWalk(directions);

        var weights = Enumerable.Repeat(1L, infos.Count).ToArray();

        var x = 1;
        while(x < weights.Length)
        {
            if (infos[x].CanEqual(infos[0].StepsAfterLoops(weights[0]), out var loopCount))
            {
                weights[x] = loopCount;
                x++;

                if (x >= weights.Length)
                {
                    break;
                }
            }
            else
            {
                x = 1;
                weights[0]++;
            }
        }

        var stepValues = infos.Select((info, i) => info.StepsAfterLoops(weights[i])).ZipWithIndex().ToArray();
        return stepValues.First().Value;
    }
}

public sealed record Node(string Id);

public sealed record Direction(int Value)
{
    public static Direction FromChar(char c) => new(c switch
    {
        'L' => 0,
        'R' => 1,
        _  => throw new ArgumentOutOfRangeException()
    });

};