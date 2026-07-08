# Mechanical Sympathy

---

## Some material

- [The original blog post](https://mechanical-sympathy.blogspot.com/)
- [The most amazing achievement of the computer software industry is its continuing cancellation of the steady and staggering gains made by the computer hardware industry.](https://news.ycombinator.com/item?id=37538556)
- [Mechanical Sympathy in Cooperative Multitasking by Kenny Chamberlin](https://www.youtube.com/watch?v=1dLYhc2ho4Y)
- [Programmers, Now More Than Ever, Need Mechanical Sympathy](https://www.youtube.com/watch?v=OMSTT4SLwus)
- [Mechanical Sympathy approach - does it still hold up?](https://www.reddit.com/r/devops/comments/1damjk6/mechanical_sympathy_approach_does_it_still_hold_up/)

---

The other day, I was pondering the change in CPU speed since my first job in 1985.

We work though many layers of abstraction these days (which makes writing things a lot easier), but was it useful being closer to the machine

---

CPU speed since 1985: ~1000x-100,000x

- Clock speed alone: ~8-16MHz (286-era) → ~4-5GHz is only ~300-600x
- But with pipelining, superscalar/out-of-order execution, SIMD, and multiple cores, effective instruction throughput is easily 10,000-100,000x higher

---

Though latency (time to fetch a random address): only ~10-15x better

- 1985: DRAM access time ~150-250ns (e.g., 150ns DIP DRAM chips of the era)
- Today: DRAM row access is still ~40-60ns; full random access with controller overhead often lands around 70-100ns
- Physics (capacitor charge/discharge in the DRAM array) hasn't scaled the way transistor switching has, so latency is the laggard.

---

Bandwidth: ~10,000x+ better

- 1985: a memory chip/bus delivered on the order of 1 MB/s
- Today: a single DDR5 channel delivers 40-50+ GB/s, and multi-channel servers reach hundreds of GB/s to TB/s (HBM even higher)
- Wider buses, higher clocks, burst/prefetch modes, multiple channels — all attack bandwidth, not latency.

---

## The takeaway (the actual "memory wall")

CPUs got roughly 1,000-100,000x faster while DRAM latency only got ~10-15x faster. In 1985 a memory access cost a handful of CPU cycles; today it can cost 200-400+ cycles. That's why cache hierarchies, prefetching, and cache-conscious data layout (classic mechanical sympathy material) matter so much more now than they did then — the CPU is starved waiting on memory far more often.

---

## What is Mechanical Sympathy

Mechanical sympathy = understanding how the hardware actually works so you can write software that works with it instead of against it. Martin Thompson borrowed the phrase from F1 driver Jackie Stewart, who said the best drivers have a feel for how the machine works. Same idea for engineers and CPUs/memory/OS.

---

## More specifically

The core gap: software abstraction vs. hardware reality

Most developers reason about code in terms of "steps executed," not what the hardware is doing underneath. High-level languages and abstractions provided by the runtime (garbage collection, virtual memory, thread schedulers) hide costs that can dominate performance.

---

## Some hardware features matter, sometimes

- Branch prediction — predictable branches are nearly free, unpredictable ones stall the pipeline
- Memory latency vs. CPU speed — the growing gap 
- Context switches & syscalls — why cooperative/async models avoid kernel-thread overhead

---

## An example

[Branch prediction benchmark (sorted vs unsorted array, C#)](https://github.com/clivetong/Talks/blob/main/MechanicalSympathy/benchmarks/branch-prediction/Program.cs)

---

### To verify that it isn't just better code optimization, we'll track the recompilations

```Powershell
$env:DOTNET_JitDisasm="Program:<<Main>$>g__SumAboveThreshold|0_2"
dotnet run -c Release 
```

---

### Where it shows up in real systems

LMAX Disruptor (Thompson's own project) is the canonical case study — lock-free ring buffer designed entirely around cache lines and mechanical sympathy.

---

### But does it still hold?

Cloud/managed runtimes, JIT compilers, and abstraction layers arguably reduce how much this matters day-to-day. The honest answer is it matters less for CRUD APIs, more for anything latency-sensitive, high-throughput, or cost-at-scale.

---

### In summary

Not "know assembly" — more like: profile before you assume (and check in the large too), understand your data layout, know what your language runtime is doing behind the scenes, and know when this level of thinking is worth the cost vs. when it's premature optimization.
