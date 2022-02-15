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
    [Arguments(4)]
    public int FibA(int a)
    {
        if (a == 0)
            return 1;
        return a * FibA(a - 1);
    }

    [Benchmark]
    [Arguments(4)]
    public int FibB(int a)
    {
        if (a != 0)
            return a * FibB(a - 1);
        return 1;
    }
}
