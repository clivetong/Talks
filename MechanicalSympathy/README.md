# Mechanical Sympathy

:warning: Under construction!

---

[Mechanical Sympathy in Cooperative Multitasking by Kenny Chamberlin](https://www.youtube.com/watch?v=1dLYhc2ho4Y)
[Go class: Mechanical sympthy](https://www.youtube.com/watch?v=7QLoOd9HinY)
[Programmers, Now More Than Ever, Need Mechanical Sympathy](https://www.youtube.com/watch?v=OMSTT4SLwus)
[Quote on waste](https://news.ycombinator.com/item?id=37538556)
[Mechanical Sympathy approach- does it still hold up?](https://www.reddit.com/r/devops/comments/1damjk6/mechanical_sympathy_approach_does_it_still_hold_up/)
[The original blog post](https://mechanical-sympathy.blogspot.com/)
[Branch prediction benchmark (sorted vs unsorted array, C#)](benchmarks/branch-prediction)

A standard consumer computer today is roughly hundreds of thousands to millions of times faster than a top-tier computer from 1985.

A single core in your current computer is roughly 1,000 to 5,000 times faster at raw instruction processing than a typical PC from 1985. When taking modern architectural efficiency and the ability to process 64-bit chunks into account, some complex tasks can run 10,000 times faster.

Claude commented: 

This is genuinely a repo where you're prepping this exact talk, so I'll answer this as a real recommendation rather than generic trivia — and it lines up with the links you've already gathered.

Here's how I'd structure it, in the spirit of Martin Thompson's original framing (the term comes from his blog, which you've already linked):

1. Define the concept early, with the racing-driver analogy
Mechanical sympathy = understanding how the hardware actually works so you can write software that works with it instead of against it. Martin Thompson borrowed the phrase from F1 driver Jackie Stewart, who said the best drivers have a feel for how the machine works. Same idea for engineers and CPUs/memory/OS.

2. The core gap: software abstraction vs. hardware reality
Most developers reason about code in terms of "steps executed," not what the hardware is doing underneath. Set up the tension: high-level languages and abstractions (garbage collection, virtual memory, thread schedulers) hide costs that can dominate performance.

3. Concrete hardware truths that matter
This is the meat — pick 3-4 to go deep on rather than skimming ten:
- Cache lines & locality — sequential access vs. pointer-chasing; false sharing across cores
- Branch prediction — predictable branches are nearly free, unpredictable ones stall the pipeline
- Memory latency vs. CPU speed — the growing gap (your README's "1000-5000x faster" stat is a great hook here — but pair it with the fact memory latency hasn't scaled nearly as much, which is why mechanical sympathy matters more, not less, today)
- Context switches & syscalls — why cooperative/async models (ties to your Kenny Chamberlin link) avoid kernel-thread overhead

4. Show, don't tell — a benchmark
A live or slide-based example (array-of-structs vs struct-of-arrays, or linked list vs array traversal) showing a 10-50x difference from cache-friendly code alone. This is usually the moment that lands the whole talk.

5. Where it shows up in real systems
LMAX Disruptor (Thompson's own project) is the canonical case study — lock-free ring buffer designed entirely around cache lines and mechanical sympathy. Worth a slide even if brief.

6. Address the pushback
Your Reddit link ("does it still hold up?") suggests you want to engage the counterargument: cloud/managed runtimes, JIT compilers, and abstraction layers arguably reduce how much this matters day-to-day. Good talks address this rather than ignore it — the honest answer is it matters less for CRUD APIs, more for anything latency-sensitive, high-throughput, or cost-at-scale.

7. Close with actionable takeaways
Not "know assembly" — more like: profile before you assume, understand your data layout, know what your language runtime is doing behind the scenes, and know when this level of thinking is worth the cost vs. when it's premature optimization.