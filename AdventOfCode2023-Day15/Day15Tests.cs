namespace AdventOfCode2023_Day15;

public class Day15Tests
{
    [Fact]
    public void Text_Hash_Value()
    {
        HashMap.ComputeHash("rn=1").Should().Be(30);
        HashMap.ComputeHash("cm-").Should().Be(253);
    }

    [Fact]
    public void Example1_HashSum_1320()
    {
        Example1.Split(',').Select(HashMap.ComputeHash).Sum().Should().Be(1320);
    }

    [Fact]
    public void PuzzleInput_HashSum_514394()
    {
        InputLoader.LoadText().Split(',').Select(HashMap.ComputeHash).Sum().Should().Be(514394);
    }

    [Fact]
    public void Example1HashMap_ApplyAll_Changed()
    {
        var operations = Example1.Split(',').Select(HashMapOperation.From);
        var hm = new HashMap();

        hm.ApplyAll(operations);
        
        hm.ToString().Should().Be("Box 0: [rn 1] [cm 2]\nBox 3: [ot 7] [ab 5] [pc 6]");
    }

    [Fact]
    public void Example1HashMap_ApplyAll_FocusingPower145()
    {
        var operations = Example1.Split(',').Select(HashMapOperation.From);
        var hm = new HashMap();

        hm.ApplyAll(operations);
        
        hm.FocusingPower.Should().Be(145);
    }

    [Fact]
    public void PuzzleInputHashMap_ApplyAll_FocusingPower236358()
    {
        var operations = InputLoader.LoadText().Split(',').Select(HashMapOperation.From);
        var hm = new HashMap();

        hm.ApplyAll(operations);
        
        hm.FocusingPower.Should().Be(236358);
    }

    private static string Example1 => 
        "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";
}