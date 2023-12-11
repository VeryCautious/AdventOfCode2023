using System.Collections.Immutable;

namespace AdventOfCode2023_Day9;

public class Day9Tests
{
    [Fact]
    public void Measurement_PredictNext_28()
    {
        var measurements = "1   3   6  10  15  21".IntegersAsList();

        var next = PredictNext(measurements);
            
        next.Should().Be(28);
    }

    [Fact]
    public void Measurements_SummedPredictNext_1731106378()
    {
        var predictedSum = InputLoader.LoadLineByLine()
            .Select(line => line.IntegersAsList())
            .Select(PredictNext)
            .Sum();

        predictedSum.Should().Be(1731106378);
    }

    [Fact]
    public void Measurements_SummedPredictLast_1087()
    {
        var predictedSum = InputLoader.LoadLineByLine()
            .Select(line => line.IntegersAsList())
            .Select(PredictLast)
            .Sum();

        predictedSum.Should().Be(1087);
    }

    [Fact]
    public void Measurement_PredictLast_5()
    {
        var measurements = "10  13  16  21  30  45".IntegersAsList();

        var next = PredictLast(measurements);
            
        next.Should().Be(5);
    }

    private static int PredictLast(IImmutableList<int> measurements) 
        => Predict(measurements, m => m[0], (x, y) => x - y);

    private static int PredictNext(IImmutableList<int> measurements) => 
        Predict(measurements, m => m[^1], (x, y) => x + y);

    private static int Predict(IImmutableList<int> measurements, Func<IImmutableList<int>,int> referenceSelector, Func<int, int, int> predictionCalculation)
    {
        if (measurements.All(m => m == 0))
        {
            return 0;
        }

        var deltas = measurements
            .SlidingWindow()
            .Select(t => t.Item2 - t.Item1)
            .ToImmutableList();

        var newDelta = Predict(deltas, referenceSelector, predictionCalculation);
        return predictionCalculation(referenceSelector(measurements), newDelta);
    }
}