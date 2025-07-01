---
transition: "slide"
slideNumber: false
title: "Some talks from Build 2025 (May 19-22)"
---

::: block
*The C# Memory Model* {style=background:red;width:500px}
:::

---

### Assembled material

- Some slides from [W08-a: SMP, Multicore, Memory Ordering and Locking](https://www.youtube.com/watch?v=luyj4biSAeM)
- [The C# memory model](https://github.com/dotnet/runtime/blob/main/docs/design/specs/Memory-model.md#net-memory-model)
- [The issue to define a better memory model](https://github.com/dotnet/runtime/issues/79764)
- [Questions about whether the JIT obeys them](https://github.com/dotnet/runtime/pull/75790#issuecomment-1354408347) and [here](https://github.com/dotnet/runtime/issues/6280)
- [MSDN article on the old ECMA model](https://learn.microsoft.com/en-us/archive/msdn-magazine/2012/december/csharp-the-csharp-memory-model-in-theory-and-practice) and [part two](https://learn.microsoft.com/en-us/archive/msdn-magazine/2013/january/csharp-the-csharp-memory-model-in-theory-and-practice-part-2)
- [The best book on this topic, for Rust](https://marabos.nl/atomics/memory-ordering.html)
- [The OCaml memory model](https://ocaml.org/manual/5.3/memorymodel.html#sec92)
- [A talk on the ARM model](https://www.youtube.com/watch?v=2I8OHacills)

---

### What are we covering?

How you can be sure that your multi-threaded code has no races

- race-free is really hard to demonstrate

---

### What is a memory model?

A contract between the language platform and the writer of code that tells them what guarantees they have

---

### Why is it interesting (to me)?

It lies at the intersection of hardware and software, requiring guarantees from:

---

- the hardware
  - processor
    - pipelines
    - store buffers
    - invalidation queues
    - bus snooping and directory protocols across interconnects (cache coherency)
    - speculative execution
    - micro-ops (Intel ie assembly is just the instructions of another virtual machine)
    - superscalar
    - [register renaming](https://en.wikipedia.org/wiki/Tomasulo%27s_algorithm)

---

- the hardware (cont)
  - MMU
  - DMA
- the operating system
- the compiler and JIT
- the runtime libraries

---


### [Why doesn't the hardware just get things right?](https://www.youtube.com/watch?v=luyj4biSAeM)

- So Intel does go some way to doing this, but it costs space on the die and performance

![Intel](images/intel.png)

---

### So what's the state of play?

- Newer architectures instead optimize for single threaded code where the CPU guarantees that you observe things in program order
- [Amdahl's law](https://en.wikipedia.org/wiki/Amdahl%27s_law)

- when you need to guarantee the observations cross cores then you are likely to synchronize using program level locks

- if you don't want to lock, you'd better understand the memory model

---

### Sequential consistency

[It is the property that "... the result of any execution is the same as if the operations of all the processors were executed in some sequential order, and the operations of each individual processor appear in this sequence in the order specified by its program."](https://en.wikipedia.org/wiki/Sequential_consistency)

---

### Cache coherency

![Coherence](images/coherency.png)

---

### Write-through caches

![Write-through](images/write-through.png)

---

But slows things down to the speed of main memory!

---

![Numa](images/numa.png)

---

### MESI

![MESI](images/mesi.png)

---

I find it fascinating that we write

- message passing systems
- on top of a procedural language
- that runs on hardware that passes cache lines around

---

![Protocol](images/protocol.png)

---

### TSO and the moving instructions

---

### So this is all about ARM?

- Maybe.

- .NET Framework gave a very weak memory model (not many guarantees)
  - ECMA-334 (and Duffy's book [Concurrent Programming on Windows](https://learning.oreilly.com/library/view/concurrent-programming-on/9780321434821/) 2008)
  - volatile had a meaning very similar to old C, with device memory ideas

- .NET Core designed for modern platforms (ARM)
  - but can't expect people to get the code right without help, so make [the memory model much stronger](https://github.com/dotnet/runtime/blob/main/docs/design/specs/Memory-model.md)!
  - Don't make it too strong
    - so no sequential consistency (not necessary an interleaving of the code of the threads that run it)

---

### [ECMA 335 vs. .NET memory models](https://github.com/dotnet/runtime/blob/main/docs/design/specs/Memory-model.md#ecma-335-vs-net-memory-models)

```Text
ECMA 335 standard defines a very weak memory model. After two decades the desire to have a flexible model did not result in considerable benefits due to hardware being more strict. On the other hand programming against ECMA model requires extra complexity to handle scenarios that are hard to comprehend and not possible to test.

In the course of multiple releases .NET runtime implementations settled around a memory model that is a practical compromise between what can be implemented efficiently on the current hardware, while staying reasonably approachable by the developers. This document rationalizes the invariants provided and expected by the .NET runtimes in their current implementation with expectation of that being carried to future releases.
```

---

### What do the following programs do?

---

```csharp
Task.Run(() => { while (true) { counter++; } });

while (true)
{
  Console.WriteLine(counter);
}
```

Do I only see the values going up?

---

Yes, though the Console.WriteLine could move across cores at the whim of the observations

---

```csharp
static bool flag;

static void Main(string[] args)
{
  Task.Run(() => flag = true);

  while (!flag)
  {
  }

  System.Console.WriteLine("done");
```

---

```csharp
  while (!Volatile.Read(ref flag))
```

---

```csharp
void ThreadFunc1() { while (true) { obj = new MyClass(); } }

void ThreadFunc2() { while (true) { obj = null; } }

void ThreadFunc3()
{
    MyClass localObj = obj;
    if (localObj != null)
    {
        System.Console.WriteLine(localObj.ToString());
    }
}
```

---

```csharp
// accessing members of the local object is safe because
// - reads cannot be introduced, thus localObj cannot be re-read and become null
// - publishing assignment to obj will not become visible earlier than write operations in the MyClass constructor
// - indirect accesses via an instance are data-dependent reads, thus we will see results of constructor's writes
```

---

### Key remarks from the new memory model

---

### Do I need to care?

- Probably not - you should be using higher level constructs rather than caring about the observability of field writes from other threads
- `lock` is really important as it lets you assert an invariant at start and end, and stops others working when the invariant isn't holding
  - recursive locks on windows are easy to get wrong 
  - lock levelling to avoid deadlocks
- use in-built concurrency primitives like `lazy` - those are reasoned about by experts and have massive soak tests to check them
- [structured concurrency is the future](https://en.wikipedia.org/wiki/Structured_concurrency)
