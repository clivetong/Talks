using System.Diagnostics;

// Classic branch-prediction demo (the "why is it faster to process a sorted
// array" benchmark). Same data, same loop, same instructions — the only
// difference is whether the branch inside the loop is predictable.
//
// Loop body: `if (data[i] >= threshold) sum += data[i];`
// Unsorted data -> the CPU's branch predictor guesses wrong ~50% of the time,
// causing a pipeline flush on every misprediction.
// Sorted data -> the branch becomes almost perfectly predictable (long runs
// of taken / not-taken), so the pipeline stays full.

const int ArraySize = 32768;
const int Threshold = 128;
const int Iterations = 100_000; // repeat the pass over the array many times so the effect dominates over loop overhead
const int WarmupIterations = 5;

var random = new Random(42);
int[] data = new int[ArraySize];
for (int i = 0; i < ArraySize; i++)
{
    data[i] = random.Next(0, 256);
}

Console.WriteLine($"Array size: {ArraySize:N0}, passes: {Iterations:N0}, threshold: {Threshold}");
Console.WriteLine();

// --- Unsorted ---
WarmUp(data, WarmupIterations);
var (unsortedMs, unsortedSum) = Time(data, Iterations);
Console.WriteLine($"Unsorted: {unsortedMs,8:N0} ms   (sum={unsortedSum}, sanity check)");

// --- Sorted ---
int[] sorted = (int[])data.Clone();
Array.Sort(sorted);

WarmUp(sorted, WarmupIterations);
var (sortedMs, sortedSum) = Time(sorted, Iterations);
Console.WriteLine($"Sorted:   {sortedMs,8:N0} ms   (sum={sortedSum}, sanity check)");

Console.WriteLine();
Console.WriteLine($"Speedup from sorting: {unsortedMs / (double)sortedMs:N2}x");

static (long ElapsedMs, long Sum) Time(int[] data, int iterations)
{
    var sw = Stopwatch.StartNew();
    long sum = SumAboveThreshold(data, iterations);
    sw.Stop();
    return (sw.ElapsedMilliseconds, sum);
}

static void WarmUp(int[] data, int iterations) => SumAboveThreshold(data, iterations);

static long SumAboveThreshold(int[] data, int iterations)
{
    long sum = 0;
    for (int pass = 0; pass < iterations; pass++)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] >= Threshold)
            {
                sum += data[i];
            }
        }
    }
    return sum;
}
