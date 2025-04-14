
// https://refactoring.guru/design-patterns/strategy

// Feels a lot like state, which can be regarded as an extension of Strategy
//   - we talked there about dynamic switching of the implementation at runtime (with composition over inheritance)

var context = new Context();

var result1 = context.Sorted([2, 1, 3, 4, 5, 4]);

context.SetStrategy(new InplaceSorter());

var result2 = context.Sorted([2, 1, 3, 4, 5, 4]);

Console.ReadLine();

interface ISorter
{
    int[] Sort(int[] items);
}

class Context
{
    private ISorter sorter = new DefaultSorter();

    public void SetStrategy(ISorter sorter)
    {
        this.sorter = sorter;
    }

    internal class DefaultSorter : ISorter
    {
        public int[] Sort(int[] items) => items.Order().ToArray();
    }

    public int[] Sorted(int[] items) => sorter.Sort(items);
}

class InplaceSorter : ISorter
{
    public int[] Sort(int[] items)
    {
        var result = new List<int>(items);
        result.Sort();
        return result.ToArray();
    }
}

