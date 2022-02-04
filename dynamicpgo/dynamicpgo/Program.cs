using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

// Run the benchmarks
BenchmarkRunner.Run<PgoBenchmarks>();

// Example from the gist https://gist.github.com/EgorBo/dc181796683da3d905a5295bfd3dd95b

// https://github.com/dotnet/runtime/pull/52708
// https://github.com/dotnet/runtime/pull/55478

[Config(typeof(MyEnvVars))]
public class PgoBenchmarks
{
    // Custom config to define "Default vs PGO"
    class MyEnvVars : ManualConfig
    {
        public MyEnvVars()
        {
            // Use .NET 6.0 default mode:
            AddJob(Job.Default.WithId("Default mode"));

            // Use Dynamic PGO mode:
            AddJob(Job.Default.WithId("Dynamic PGO")
                .WithEnvironmentVariables(
                    new EnvironmentVariable("DOTNET_TieredPGO", "1"),
                    new EnvironmentVariable("DOTNET_TC_QuickJitForLoops", "1"),
                    new EnvironmentVariable("DOTNET_ReadyToRun", "0")));
        }
    }


    //
    // Benchmark 1: Devirtualize unknown virtual calls:
    //

    public IEnumerable<object> TestData()
    {
        // Test data for 'GuardedDevirtualization(ICollection<int>)'
        yield return new List<int>();
    }

    [Benchmark]
    [ArgumentsSource(nameof(TestData))]
    public void GuardedDevirtualization(ICollection<int> collection)
    {
        // a chain of unknown virtual calls...
        collection.Clear();
        collection.Add(1);
        collection.Add(2);
        collection.Add(3);
    }


    //
    // Benchmark 2: Allow inliner to be way more aggressive than usual
    //              for profiled call-sites:
    //

    [Benchmark]
    public StringBuilder ProfileDrivingInlining()
    {
        StringBuilder sb = new();
        for (int i = 0; i < 1000; i++)
            sb.Append("hi"); // see https://twitter.com/EgorBo/status/1451149444183990273
        return sb;
    }


    //
    // Benchmark 3: Reorder hot-cold blocks for better performance
    //

    [Benchmark]
    [Arguments(42)]
    public string HotColdBlockReordering(int a)
    {
        if (a == 1)
            return "a is 1";
        if (a == 2)
            return "a is 2";
        if (a == 3)
            return "a is 3";
        if (a == 4)
            return "a is 4";
        if (a == 5)
            return "a is 5";
        return "a is too big"; // this branch is always taken in this benchmark (a is 42)
    }
}
