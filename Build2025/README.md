---
transition: "slide"
slideNumber: false
title: "Some talks from Build 2025 (May 19-22)"
---

::: block
*Build Recap of some sessions* {style=background:red;width:500px}
:::

---

### What are we covering?

We'll cover parts of a few of the talks from Build.

---

### Quick Impression of Build

Not as in-depth tech as in previous years, this one covered the arrival of AI into the business and developer experience.

- Agents, agents, agents
- Orchestration of Agents
- Agents talking to tools (MCP) or Agents (A2A)
- CoPilot taking on issues, producing a PR in background and then working collaboratively to solve a problem

---

### Weirdest things

[Running .NET source files `dotnet run app.cs`](https://www.youtube.com/watch?v=98MizuB7i-w)

I just don't get it. You have scripting languages and programming languages, and very few can straddle the divide.

---

### Funniest things

[Scott and Mark Learn to...LIVE](https://build.microsoft.com/en-US/sessions/KEY040)

![Robot](images/robots.png)

---

I'll rush through these talks from the [YouTube playlist](https://www.youtube.com/playlist?list=PLFPUGjQjckXH1BDmT9hZw_fUi9NZRbVJt):

- [Yet Another "Highly Technical Talk" with Hanselman and Toub](https://www.youtube.com/watch?v=J3IQBI5HVOw)
- [Python Meets .NET: Building AI Solutions with Combined Strengths](https://www.youtube.com/watch?v=fDbCqalegNU)
- [A 10x Faster TypeScript with Anders Hejlsberg](https://www.youtube.com/watch?v=UJfF3-13aFo)
- [Inside Azure innovations with Mark Russinovich](https://build.microsoft.com/en-US/sessions/BRK195?source=sessions)

---

### Yet Another "Highly Technical Talk" with Hanselman and Toub

- Hanselman and Toub have a good dynamic
- This time they implement (most of) Channels
  - like the Go feature that ripped off CSP

---

```CSharp
using System.Threading.Channels;

Channel<int> c = Channel.CreateUnbounded<int>();

_ = Task.Run(async () =>
{
    for (int i = 0; i < 10; i++)
    {
        await c.Writer.WriteAsync(i);
        await Task.Delay(TimeSpan.FromSeconds(1));
    }

    c.Writer.Complete();
});

while (true)
{
    Console.WriteLine(await c.Reader.ReadAsync());
}
```

---

![Versions](images/versions.png)

---

### Key Ideas

- Version 1 (using the real library)
- Version 2 (first implementation)
  - Use locks to preserve your invariants
  - TaskCompletionSources for someone to wait on
  - ValueTasks when you control the consumers
- Version 3 (WaitToReadAsync and TryRead)
  - MaybeNullWhen

---

```CSharp
    private readonly Queue<T> _items = [];
    private object SyncObj => _items;
    private readonly Queue<TaskCompletionSource<T>> _readers = [];
    private bool _completed;
```

- Toub's explanation of the SyncObj

---

```CSharp
    public ValueTask<T> ReadAsync()
    {
        lock (SyncObj)
        {
            if (_items.TryDequeue(out var item))
            {
                return new ValueTask<T>(item);
            }

            if (_completed)
            {
                return ValueTask.FromException<T>(new InvalidOperationException("Channel is completed"));
            }

            TaskCompletionSource<T> tcs = new();
            _readers.Enqueue(tcs);
            return new ValueTask<T>(tcs.Task);
        }
    }

```

- [ValueTask cheaper](https://devblogs.microsoft.com/dotnet/understanding-the-whys-whats-and-whens-of-valuetask/)
- The tcs Task's value will be set when writing

---

```CSharp
    public ValueTask WriteAsync(T item)
    {
        lock (SyncObj)
        {
            if (_completed)
            {
                return ValueTask.FromException(new InvalidOperationException("Channel is completed"));
            }

            if (_readers.TryDequeue(out var tcs))
            {
                tcs.SetResult(item);
            }
            else
            {
                _items.Enqueue(item);
            }
        }

        return default;
    }
```

---

```CSharp
    public void Complete()
    {
        lock (SyncObj)
        {
            _completed = true;
            while(_readers.TryDequeue(out var tcs))
            {
                tcs.SetException(new InvalidOperationException("Channel completed"));
            }
        }
    }
```

---

- Add a `TryRead`
- Add a `WaitToReadAsync`

---

```CSharp
    private TaskCompletionSource<bool>? _waitingReaders;
```

```CSharp
    public ValueTask<bool> WaitToReadAsync
    {
        get
        {
            lock (SyncObj)
            {
                if (_items.Count > 0 || _completed)
                {
                    return new ValueTask<bool>(_items.Count > 0);
                }
                _waitingReaders ??= new();
                return new ValueTask<bool>(_waitingReaders.Task);
            }
        }
    }
```

---

- Add code to writers to signal

```CSharp
            {
                _items.Enqueue(item);

                if (_waitingReaders != null)
                {
                    _waitingReaders.SetResult(true);
                    _waitingReaders = null; 
                }
            }
```

---

```CSharp
  public bool TryRead([MaybeNullWhen(false)]out T item) 
    {
        lock (SyncObj)
        {
            return _items.TryDequeue(out item);
        }
    }
```

---

### Key Ideas (continued)

- Version 4 (demo parallel stacks)
  - Reentrant locks
  - invariants and avoiding unknown code
- Version 5 (performance and the real fix)
  - `TaskCreationOptions.RunContinuationsAsynchronously`
- Version 6 (use the builtins)
  - lock free

---

```CSharp
var lockable = new object();

void ActionToDo()
{
    lock (lockable)
    {
        // Expect the invariants to hold here

        // ...

        // Expect the invariants to be restored here
    }
}

lock (lockable)
{
    // Expect the invariants to hold here

    //....
    ActionToDo();

    // Expect the invariants to be restored here
}
```

---

```CSharp
TaskCompletionSource<int> tcs = new();  
    // new(TaskCreationOptions.RunContinuationsAsynchronously);

void Foo()
{
  tcs.SetResult(42);
}
Task.Run(() =>
{
  Thread.Sleep(TimeSpan.FromSeconds(1));
  Foo();
 });

var res = await tcs.Task;
Console.WriteLine(res);
```

---

![Stacktrace](images/stacktrace.png)

---

### Python Meets .NET: Building AI Solutions with Combined Strengths

- Run the Python interpreter in .NET process
  - Python has APIs to do this
  - a question of packaging

---

### Why Python?

![C# v Python](images/CSharpvPython.png)

---

### Just use Rest

![Via Rest](images/ViaRest.png)

---

### Do it in process instead

![CSnakes](images/CSnakes.png)

---

Stephen Toub challenged the speaker to implement this.

---

### Demo Project

![Demo Project](images/DemoProject.png)

---

### Python side

![Python](images/Python.png)

---

### C# side

![C#](images/Program.png)

---

### Marshalling

![Marshalling](images/Marshalling.png)

---

### And it uses a source generator

![Source generator](images/sourcegenerator.png)

---

### Easy configuration

![Configurable](images/Options.png)

---

[Get it here](https://tonybaloney.github.io/CSnakes/)

---

### A 10x Faster TypeScript with Anders Hejlsberg

---

### The implementations

- [Old](https://github.com/microsoft/typescript)
- [New](https://github.com/microsoft/typescript-go)

---

### The challenges

- VS Code is 1.5 million lines of TS
- Microsoft have internal repos with 15 million

---

![Challenges](images/Challenges.png)

---

![Requirements](images/Requirements.png)

---

![Condenders](images/Contenders.png)

---

- Rust
  - No automatic memory management and no cyclic data structures
  - Made it hard to port to it - could rewrite but wanted to port
- C#
  - Their code is procedural and not OO
  - No mature native code generation story
- Go
  - Good support for native code, 10 years of development and highly optimized
  - Procedural with first class functions
  - Great struct data layout, great concurrency

---

- August last year started with the scanner and parser and saw 10x

![Improvements](images/improvements.png)

---

![How?](images/how.png)

---

See talk for lots of detail about the working

![Go v TS](images/GoTS.png)

---

![Concurrency](images/concurrency.png)

---

![The preview](images/preview.png)

---

### Q&A

Q: What happens to the old compiler?

A: There will be releases before 7. Once 7 is out, there will be no changes to the old code base. 

Note that they actually ported a snap of 5.7, so need to get  additions ported.

---

### Inside Azure innovations with Mark Russinovich

---

![Plan](images/innovationtalk.png)

---

### Offloading using Azure Boost (2.0)

![Azure Boost](images/azureboost.png)

[IIRC 20% coverage, Boost card had FPGA and ARM]

---

![Top of Rac](images/tor.png)

---

![Overheads](images/dma.png)

---

![RDMA](images/rdma.png)

---

### AllReduce across GPUs (GB/s)

![Azure Boost](images/eleven.png)

---

### Minimal downtime

![Host Upgrade](images/hostupgrade.png)

[18 - 5 - 2 - 4 - 3 preserving host update]

---

### Scale storage (AI workloads)

![Scale Storage](images/scalestorage.png)

---

### LinuxGuard in Azure Linux

![LinuxGuard](images/AzureLinux.png)

---

![Linux code integrity](images/codeintegrity.png)

---

### Hostile multitenancy

![Boundaries](images/hostilemultitenancy.png)

[Hypervisor, but what about edge]

---

### Very lightweight containers

![Hyperlight](images/hyperlight.png)

Used in edge scenarios like FrontDoor

---

### Azure Container Instances

The runner for serverless compute (ACI was the first in the industry)
 - [ACI](https://www.kodez.com.au/post/deciphering-azure-container-services-a-guide-to-select-between-aca-aci-and-aks) and [NGroups](https://learn.microsoft.com/en-us/azure/container-instances/container-instance-ngroups/container-instances-about-ngroups)

![ACI](images/aci.png)

---

### Azure Incubations

![Graduated](images/graduated.png)

![Coming](images/underdevelopmen.png)

---

### Radius

![Radius](images/radius.png)

---

![Radius How?](images/radiushow.png)

---

![Multiple deployments](images/multipleenvs.png)

---

### [drasi](http://github.com/drasi-project)

![Change detection](images/changedetection.png)

---

![Continuous Query](images/drasi.png)

The future of Reactive!

---

### Confidential Computing

![History of confidential on Azure](images/confidential.png)

Now a focus on confidential comouting on GPUs

---

![Analog Optical Computing](images/aoc.png)

---

![1792 virtual processors](images/taskmg1792.png)

---
