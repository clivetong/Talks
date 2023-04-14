using Akka.Actor;
using System.Reactive.Subjects;

Task.Run(delegate
{
    for (int i = 0; i < 1000; i++)
    {
        Console.WriteLine("Foo");
    }
});

Task.Run(delegate
{
    for (int i = 0; i < 1000; i++)
    {
        Console.WriteLine("Bar");
    }
});

Console.ReadLine();
Console.Clear();




// -------------------------------------------------------------------------------------


Console.WriteLine("Barrier");


var barrier = new Barrier(2);

Task.Run(delegate
{
    for (int i = 0; i < 1000; i++)
    {
        barrier.SignalAndWait();
        Console.WriteLine("Foo");
        barrier.SignalAndWait();
    }
});

Task.Run(delegate
{
    for (int i = 0; i < 1000; i++)
    {
        barrier.SignalAndWait();
        barrier.SignalAndWait();
        Console.WriteLine("Bar");
    }
});

Console.ReadLine();
Console.Clear();



// -------------------------------------------------------------------------------------

Console.WriteLine("Semaphore");


var fooer = new Semaphore(0,1);
var barer = new Semaphore(0, 1);

fooer.Release();
Task.Run(delegate
{
    for (int i = 0; i < 1000; i++)
    {
        fooer.WaitOne();
        Console.WriteLine("Foo");
        barer.Release();
    }
});

Task.Run(delegate
{
    for (int i = 0; i < 1000; i++)
    {
        barer.WaitOne();
        Console.WriteLine("BAr");
        fooer.Release();
    }
});


Console.ReadLine();
Console.Clear();



// -------------------------------------------------------------------------------------

Console.WriteLine("Condition variables");

var lockable = new object();
var foo = true;

Task.Run(delegate
{
    lock (lockable)
    {
        for (int i = 0; i < 1000; i++)
        {
            while (!foo)
            {
                Monitor.Wait(lockable);
            }
            Console.WriteLine("Foo");
            foo = !foo;
            Monitor.Pulse(lockable);
        }
    }
});

Task.Run(delegate
{
    lock (lockable)
    {
        for (int i = 0; i < 1000; i++)
        {
            while (foo)
            {
                Monitor.Wait(lockable);
            }
            Console.WriteLine("BAr");
            foo = !foo;
            Monitor.Pulse(lockable);
        }
    }
});

Console.ReadLine();
Console.Clear();



// -------------------------------------------------------------------------------------

Console.WriteLine("Atomic read");

var turn = 0L;

Task.Run(delegate
{
    for (int i = 0; i < 1000; i++)
    {
        while (Interlocked.Read(ref turn) != 2 * i)
        {
            Thread.Sleep(0);
        }
        Console.WriteLine("Foo");
        Interlocked.Exchange(ref turn, 2 * i + 1);
    }
});

Task.Run(delegate
{
    for (int i = 0; i < 1000; i++)
    {
        while (Interlocked.Read(ref turn) != 2 * i + 1)
        {
            Thread.Sleep(0);
        }
        Console.WriteLine("BAr");
        Interlocked.Exchange(ref turn, 2 * i + 2);
    }
});

Console.ReadLine();
Console.Clear();

// -------------------------------------------------------------------------------------

//  <PackageReference Include="System.Reactive" Version="5.0.0" />

Console.WriteLine("Reactive");


using var fooStream = new Subject<string>();
using var barStream  = new Subject<string>();

using var sub1 = fooStream.Subscribe(x =>
{
    barStream.OnNext(x);
    barStream.OnNext("Bar");
}); 

using var sub2 = barStream.Subscribe(Console.WriteLine);

Task.Run(delegate
{
    for (int i = 0; i < 1000; i++)
    {
        fooStream.OnNext("Foo");
    }
    fooStream.OnCompleted();
});

Console.ReadLine();

// -------------------------------------------------------------------------------------

//  <PackageReference Include="Akka" Version="1.5.2" />

Console.WriteLine("Akka");

ActorSystem system = ActorSystem.Create("MySystem");
IActorRef fooActor = system.ActorOf<Foo>("foo");
IActorRef barActor = system.ActorOf<Bar>("bar");

fooActor.Tell(barActor);

Console.ReadLine();

class Step
{
}

class Foo : ReceiveActor
{
    private int _count = 0;
    private IActorRef _bar;

    public Foo()
    {
        Receive<IActorRef>(bar =>
        {
            _bar = bar;
            Self.Tell(new Step());
        });

        Receive<Step>(step =>
        {
            if (_count < 1000)
            {
                Console.WriteLine("Foo");
                _count++;
                _bar.Tell(step);
            }
        });
    }
}

class Bar : ReceiveActor
{
    public Bar()
    {
        Receive<Step>(step =>
        {
            Console.WriteLine("Bar");
            Sender.Tell(step);
        });
    }
}
