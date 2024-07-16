---
transition: "slide"
slideNumber: false
title: "Little's Law and Virtual Threads"
---

::: block
*Virtual Threads* {style=background:red;width:500px}
:::

---

### What's the talk about?

Virtual threads (aka green threads), the attempts at implementing them in the two big managed platforms, and how they make your programs look.

---

### Why?

[Little's law](https://en.wikipedia.org/wiki/Little%27s_law)

In mathematical queueing theory, Little's law is a theorem by John Little which states that the long-term average number L of customers in a stationary system is equal to the long-term average effective arrival rate λ multiplied by the average time W that a customer spends in the system. Expressed algebraically the law is 𝐿=𝜆𝑊.

---

The relationship is not influenced by the arrival process distribution, the service distribution, the service order, or practically anything else. In most queuing systems, service time is the bottleneck that creates the queue.

---

[At massive scale, in a request-per-thread, model we need to serve as many requests as possible (and operating system threads only go so far)](https://openjdk.org/jeps/425)

---

### Clarification

Process- an isolated virtual address space (Pico process)

Thread - something that exists in the context of a process, and brings computation

---

### Why now?

[Project Loom](https://openjdk.org/projects/loom/) - Fibers, Continuations and Tail-Calls for the JVM

The goal of this Project is to explore and incubate Java VM features and APIs built on top of them for the implementation of lightweight user-mode threads (fibers), delimited continuations (of some form), and related features, such as explicit tail-call.

---

### Java

Generally available in Java 21 but with a few issues remaining

[Java's virtual threads - Next steps](https://www.youtube.com/watch?v=KBW4LbCoo6c)

---

### More details

[Continuations - Under the Covers #JVMLS](https://www.youtube.com/watch?v=6nRS6UiN7X0)

- Virtual threads and carrier threads
- Parking and Unparking

---

![Continuation interface](images/continuations.png)

---

![Examples](images/examples.png)

---

### VM mechanics

When parking hapens, the continuation is moved off the thread, and another continuation is executed there.

---

### So what?

You write Java code that looks like normal Java code, and the VM can virtualize the thread for you (most of the time).

Compared to the .NET way of async where you end up with normal code and Task based code, and a compiler trick to make the latter like the former.

[Red/Green function colouring](https://journal.stuffwithstuff.com/2015/02/01/what-color-is-your-function/).

---

### .NET

[The green threads experiment](https://github.com/dotnet/runtimelab/issues/2057)

[The green threads experiment ends](https://github.com/dotnet/runtimelab/issues/2398)

[The green threads write up](https://github.com/dotnet/runtimelab/blob/feature/green-threads/docs/design/features/greenthreads.md)

---

### Why so hard?

- VMs in the 1990s did Green Threads
- hard to retrofit into the implementation?

---

### Things to read

- [Is Reactive Programming Dead?](https://www.youtube.com/watch?v=eAjy7E_FQN0)
- [Async wrappers for synchronous](https://devblogs.microsoft.com/pfxteam/should-i-expose-asynchronous-wrappers-for-synchronous-methods/)
- [Synchronous wrappers for async](https://devblogs.microsoft.com/pfxteam/should-i-expose-synchronous-wrappers-for-asynchronous-methods/)
