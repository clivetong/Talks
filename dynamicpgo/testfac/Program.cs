using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<CheckFib>();

public class CheckFib
{

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    public int FacA(int a)
    {
        if (a == 0)
            return 1;
        return a * FacA(a - 1);
    }

    [Benchmark]
    [ArgumentsSource(nameof(Values))]
    public int FacB(int a)
    {
        if (a != 0)
            return a * FacB(a - 1);
        return 1;
    }

    public IEnumerable<int> Values()
    {
        yield return 10;
        yield return 0;
    }
}
