﻿using Autofac;
using System.Diagnostics;

// https://refactoring.guru/design-patterns/singleton


// All superseded by dependency injection in modern times (you tell the container that it should always return the same instance)

var cb = new ContainerBuilder();

cb.RegisterType<NormalClass>().SingleInstance();

var container = cb.Build();

var i1 = container.Resolve<NormalClass>();
var i2 = container.Resolve<NormalClass>();

Debug.Assert(i1 == i2);


// Singletons often require multi-thread support
var a = Singleton.Instance();
var b = Singleton.Instance();

Debug.Assert(a == b);


var a2 = DoubleCheck.Instance();
var b2 = DoubleCheck.Instance();

Debug.Assert(a2 == b2);




var l1 = UsingLazy.Instance();
var l2 = UsingLazy.Instance();

Debug.Assert(l1 == l2);





InStaticConstructor a3 = null;
InStaticConstructor b3 = null;

var barrier = new Barrier(3);

Task.Run(() =>
{
     a3 = InStaticConstructor.Instance;
     Console.WriteLine($"Finished {DateTime.Now}");
     barrier.SignalAndWait();
});

Task.Run(() =>
{
    b3 = InStaticConstructor.Instance;
    Console.WriteLine($"Finished {DateTime.Now}");
    barrier.SignalAndWait();
});

barrier.SignalAndWait();

Debug.Assert(a3 == b3);


try
{
    var a4 = OneChance.Instance;
}
catch (Exception)
{

}

var a5 = OneChance.Instance;

class NormalClass
{

}

// Basic implementation
// - use the new Lock in .NET 9
class Singleton
{
    private static volatile Singleton? s_Instance = null;
    private static readonly Lock _lock = new Lock();

    public static Singleton Instance()
    {
        lock (_lock)
        {
            if (s_Instance == null)
            {
                return (s_Instance = new Singleton());
            }
        }

        return s_Instance;
    }
}

// Double check to avoid the locking in the common case
class DoubleCheck
{
    private static volatile DoubleCheck? s_Instance = null;
    private static readonly Lock _lock = new Lock();

    public static DoubleCheck Instance()
    {
        if (s_Instance == null)
        {
            lock (_lock)
            {
                if (s_Instance == null)
                {
                    return (s_Instance = new DoubleCheck());
                }
            }
        }

        return s_Instance;
    }
}

class UsingLazy
{
    // Note the options on Lazy - it can cache the exception or try again
    private static Lazy<UsingLazy> s_Lazy = new Lazy<UsingLazy>(() => new UsingLazy());

    public static UsingLazy Instance() => s_Lazy.Value;

    private UsingLazy()
    {
    }
}

// Use the single threaded nature of the static constructor
class InStaticConstructor
{
    public static InStaticConstructor Instance { get; } = new InStaticConstructor();

    private InStaticConstructor()
    {
        Console.WriteLine($"Initializing {DateTime.Now}");
        Thread.Sleep(TimeSpan.FromSeconds(5));
    }
}

//   --- but if it errors the class is dead - you can't fail the static constructor
class OneChance
{
    public static OneChance Instance { get; } = new OneChance();

    private static int s_Count = 0;

    private OneChance()
    {
        s_Count++;
        if (s_Count == 1)
        {
            throw new Exception("Not this time");
        }

    }
}
