// There's old style Builder,
// and a more fluent builder

// https://refactoring.guru/design-patterns/builder

// Easy to avoid constructing something with complex constructor

var h1 = new MyHouse(1, 4, false);

var h2 = new MyHouse("1 The Street", numWalls: 5);

var h3 = new MyHouse { NumWalls = 6 };

var h4 = FluentHouseBuilder
    .Empty()
    .WithDoors(5)
    .Build();

// People do all sorts of rules for adding things fluently
class MyHouse
{
    public MyHouse(int numDoors, int numWalls, bool hasPathway)
    {
        NumDoors = numWalls;
        NumWalls = numWalls;
        HasPathway = hasPathway;
    }

    // Though model approaches allow defaults
    public MyHouse(string name, int numDoors = 2, int numWalls = 4, bool hasPathway= false)
    {
        NumDoors = numWalls;
        NumWalls = numWalls;
        HasPathway = hasPathway;
    }

    public int NumDoors { get; init; }
    public int NumWalls { get; init; }
    public bool HasPathway { get; init; }

    public MyHouse()
    {
    }

}

// Builders can let us set the defaults easily

class HouseBuilder
{
    private int _numDoors = 2;
    private int _numWalls = 4;
    private bool _hasPathway = true;

    public void SetDoors(int doors) => _numDoors = doors;
    public void SetWalls(int walls) => _numWalls = walls;
    public void SetPathway(bool pathway) => _hasPathway = pathway;

    public MyHouse Build() => new MyHouse(_numDoors, _numWalls, _hasPathway);
}



class FluentHouseBuilder
{
    private int _numDoors = 2;
    private int _numWalls = 4;
    private bool _hasPathway = true;

    private FluentHouseBuilder()
    {
    }

    // Maybe copy after state change
    public static FluentHouseBuilder Empty() => new();

    public FluentHouseBuilder WithDoors(int doors)
    {
        _numDoors = doors;
        return this;
    }

    public FluentHouseBuilder SetWalls(int walls)
    {
        _numWalls = walls;
        return this;
    }

    public FluentHouseBuilder SetPathway(bool pathway)
    {
        _hasPathway = pathway;
        return this;
    }

    public MyHouse Build() => new MyHouse(_numDoors, _numWalls, _hasPathway);
}
