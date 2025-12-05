using System.Diagnostics;
using System.Runtime.CompilerServices;

const int Iters = 100_000;

Stopwatch sw = Stopwatch.StartNew();

while (true)
{
    long mem = GC.GetAllocatedBytesForCurrentThread();
    sw.Restart();

    for(int i =0; i < Iters; i++)
    {
        Test();
    }

    sw.Stop();
    mem = GC.GetAllocatedBytesForCurrentThread() - mem;
    Console.WriteLine($"{sw.Elapsed.TotalSeconds * 1_000_000_000 / Iters:N0} ns, {mem/Iters:N0} bytes" );
}

[MethodImpl(MethodImplOptions.NoInlining)]
static void Test()
{
    
}