using System.Security.Cryptography;

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