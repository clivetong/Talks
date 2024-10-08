﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(Tests).Assembly).Run(args);

[MemoryDiagnoser]
public class Tests
{
    [Benchmark]
    public void CallWithParams()
    {
        var result = Called(1, 2, 3, 4, 5, 6);
        if (result != 3)
        {
            throw new Exception("Something is wrong.");
        }
    }

    int Called(params int[] args) => args[2];

    // int Called(params ReadOnlySpan<int> args) => args[2];
}