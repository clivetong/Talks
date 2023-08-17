
var counter = new Counter();

for (int i = 1; true; i++)
{
    var count = counter.Increment();
    Console.WriteLine($"Count: {i} Counter: {count} Error: {Math.Abs(i - count) * 100.0 / i}");

}

class Counter
{
    readonly Random _random = new();
    public int Count { get; private set; }

    public int Increment()
    {
        for (int i = 0; i < Count; i++)
        {
            if (_random.Next(2) != 0)
            {
                return 1 << Count;
            }
        }
        Count++;
        return 1 << Count;
    }
}

/*
var r = new Random();
int L = 32;

var items = 1000000;
var max = 1000000;

var selection = Enumerable.Range(0, items)
    .Select(_ => r.Next(max))
    .ToList();

var distinct = new HashSet<int>(selection).Count;

foreach(var hashfun in new[] { Hash1, Hash2})
{
    var bv = 0;
    foreach (var x in selection)
    {
        var index = p(hashfun(x));
        bv |= (1 << index);
    }

    for (int R = 0; R < L; R++)
    {
        if ((bv & (1 << R)) == 0)
        {
            Console.WriteLine($"Distinct {distinct} Estimate {Math.Pow(2, R) / 0.77351}");
            break;
        }
    }
}

int Hash1(int x) => x.GetHashCode();

int Hash2(int x)
{
    x = ((x >> 16) ^ x) * 0x45d9f3b;
    x = ((x >> 16) ^ x) * 0x45d9f3b;
    x = (x >> 16) ^ x;
    return x;
};

int p(int x)
{
    if (x == 0)
    {
        return L;
    }

    for (int i = 0; i < L; i++)
    {
        if ((x & (1 << i)) != 0)
        {
            return i;
        }
    }

    return L;
}

*/