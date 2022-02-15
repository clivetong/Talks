using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

// Run the benchmarks
BenchmarkRunner.Run<CheckFib>();

public class CheckFib
{

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    public int FibA(int a)
    {
        if (a == 0)
            return 1;
        return a * FibA(a - 1);
    }

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    public int FibB(int a)
    {
        if (a != 0)
            return a * FibB(a - 1);
        return 1;
    }

    public IEnumerable<int> Values()
    {
        yield return 10;
        yield return 0;
    }
}
