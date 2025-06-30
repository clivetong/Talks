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

- https://github.com/dotnet/runtime/blob/main/docs/design/specs/Memory-model.md#net-memory-model
- https://github.com/dotnet/runtime/issues/79764
- https://github.com/dotnet/runtime/pull/75790#issuecomment-1354408347
- https://github.com/dotnet/runtime/issues/6280
- https://learn.microsoft.com/en-us/archive/msdn-magazine/2012/december/csharp-the-csharp-memory-model-in-theory-and-practice
- https://learn.microsoft.com/en-us/archive/msdn-magazine/2013/january/csharp-the-csharp-memory-model-in-theory-and-practice-part-2
- https://marabos.nl/atomics/memory-ordering.html
- https://ocaml.org/manual/5.3/memorymodel.html#sec92
- https://www.youtube.com/watch?v=luyj4biSAeM&list=PLtoQeavghzr3nlXyJEXaTLU9Ca0DXWMnt&index=13
- https://www.youtube.com/watch?v=2I8OHacills

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

- the hardware
  - processor
    - store buffers
    - invalidation queues
    - bus snooping and directory protocols across interconnects (cache coherency)
    - speculative execution
    - micro-ops (Intel ie assembly is just the instructions of another virtual machine)
  - MMU
  - DMA
- the operating system
- the compiler and jit
- the runtime libraries

---

I find it fascinating that we write

- message passing systems
- on top of a procedural language
- that runs on hardware that passes cache lines around

---

### [Why doesn't the hardware just get things right?](https://www.youtube.com/watch?v=luyj4biSAeM)

- So Intel does go some way to doing this, but it costs space on the die

![Intel](images/intel.png)

---

- Newer architectures instead optimize for single threaded code by way of
  - cache levels to keep the processor running with accessing main memory
  - [register renaming](https://en.wikipedia.org/wiki/Tomasulo%27s_algorithm)
  - speculation

- [Amdahl's law](https://en.wikipedia.org/wiki/Amdahl%27s_law)

---

### So this is all about Arm?

- Maybe.

- .NET Framework gave a very weak memory model (not many guarantees)
  - ECMA-334
  - volatile had a meaning very similar to old C, with device memory ideas

- .NET Core designed for modern platforms (Arm)
  - but can't expect people to get the code right without help, so make the memory model much stronger!
  - but don't make it too strong
    - so no sequential consistency (not necessary an interleaving of the code of the threads that run it)

---

### What do the following programs do?

