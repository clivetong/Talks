using System.Diagnostics;
using System.Runtime.CompilerServices;

/*
  dotnet run -c Release -f net48
  dotnet run -c Release -f net9.0
  dotnet run -c Release -f net10.0

  To see DPGO in progress
  $env:DOTNET_JitDisasm="<<Main>$>g__Test3|0_2"
*/
const int Iters = 10_000;

int[] valuesArray = Enumerable.Range(0, Iters).ToArray();
IEnumerable<int> valuesList = Enumerable.Range(0, Iters).ToList();

Stopwatch sw = Stopwatch.StartNew();

while (true)
{
    long mem = GC.GetAllocatedBytesForCurrentThread();
    sw.Restart();

    for(int i =0; i < Iters; i++)
    {
        //Test(valuesArray);
        //Test2(valuesArray);
        Test3(valuesList);
    }

    sw.Stop();
    mem = GC.GetAllocatedBytesForCurrentThread() - mem;
    Console.WriteLine($"{sw.Elapsed.TotalSeconds * 1_000_000_000 / Iters:N0} ns, {mem/Iters:N0} bytes" );
}

[MethodImpl(MethodImplOptions.NoInlining)]
static int Test(int[] values)
{
    int sum = 0;
    foreach(var x in values)
    {
        sum += x;
    }
    return sum;
}

[MethodImpl(MethodImplOptions.NoInlining)]
static int Test2(int[] values)
{
    int sum = 0;
    for(var i =0; i < values.Length; i++)
    {
        sum += values[i];
    }
    return sum;
}

[MethodImpl(MethodImplOptions.NoInlining)]
static int Test3(IEnumerable<int> values)
{
    int sum = 0;
    foreach(var x in values)
    {
        sum += x;
    }
    return sum;
}
