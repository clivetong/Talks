using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(Tests).Assembly).Run(args);

[MemoryDiagnoser]
public class Tests
{
    [Benchmark]
    public void CallWithParams()
    {
        Called(1, 2, 3, 4, 5, 6);
    }

    public void Called(params int[] args)
    {

    }
    //public void Called(params ReadOnlySpan<int> args)
    //{

    //}
}