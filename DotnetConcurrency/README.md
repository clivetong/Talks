---
transition: "slide"
slideNumber: false
title: "Tell me about .NET conurrency!"
---

::: block
*Tell me about .NET concurency* {style=background:red;width:500px}
:::

---

### UNDER CONSTRUCTION

I intended to make this into a full talk, but haven't had time yet. It was an attempt use material from [the book](https://www.amazon.co.uk/Concurrent-Programming-Windows-Architecture-Development/dp/032143482X) and add some more notes on the changes since the book was written.

---

### What and when

  data parallelism
  tasks
  actors - Orleans, Pony

---

### Moore's law

Functional programming and immutable data

---

### Windows concurrency primiives

---

### Threads

And how slow they are to spawn

---

### And we need more context

Task - the modern abstraction

Execution context

---

### Inline tasks and nesting

Conpletion (and we are goingto need that later)

---

### That got us going but what's the overhead

ie why we need a thread pool

---

### And who's going to wait for you?

---

### Unmanaged threadpools

---

### And managed threadpools and their history

Chunks of work
timers
I/O completion

---

### But don't dirty the threads

What actually gets cleaned up when you return to the pool
Throttling

---

### How it has changed over time

The new managed pool
How timers are really done

---

### So where does async fit in to this

---

### The APM (OMG)

---

### async and what it means

- The async transformation 
- why lowering?

---

### Synchronous completion

Midori

---

### Some of the more modern primitives

---

### Memory models and lock-free programming

---

### PLINQ

---
