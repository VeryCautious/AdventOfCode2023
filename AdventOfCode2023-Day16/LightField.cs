using System.Collections.Immutable;

namespace AdventOfCode2023_Day16;

public class LightField(IImmutableDictionary<Point2D,char> tiles)
{

    public IEnumerable<BeamState> GetMaximumPowered()
    {
        var width = tiles.Max(t => t.Key.X + 1);
        var height = tiles.Max(t => t.Key.Y + 1);

        var down = Enumerable.Range(0, width).Select(i => new BeamState(Direction.Down, new Point2D(i, 0)));
        var up = Enumerable.Range(0, width).Select(i => new BeamState(Direction.Up, new Point2D(i, height-1)));
        var right = Enumerable.Range(0, height).Select(i => new BeamState(Direction.Right, new Point2D(0, i)));
        var left = Enumerable.Range(0, height).Select(i => new BeamState(Direction.Left, new Point2D(width-1, i)));

        return up.Union(down).Union(left).Union(right).Select(Simulate)
            .MaxBy(beamStates => beamStates.DistinctBy(beam => beam.Position).Count())!;
    }

    public IEnumerable<BeamState> Simulate() => 
        Simulate(new BeamState(Direction.Right, new Point2D(0, 0)));

    private IEnumerable<BeamState> Simulate(BeamState startState)
    {
        var beamHeads = new Queue<BeamState>([startState]);
        var beamStates = new HashSet<BeamState>([startState]);

        while (beamHeads.Count > 0)
        {
            var current = beamHeads.Dequeue();
            var newStates = current.InteractWith(tiles[current.Position]);

            foreach (var newState in newStates)
            {
                if (tiles.ContainsKey(newState.Position) && beamStates.Add(newState))
                {
                    beamHeads.Enqueue(newState);
                }
            }
        }

        return beamStates;
    }

}