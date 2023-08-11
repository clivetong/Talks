---
transition: "slide"
slideNumber: false
title: "Dynamic Profile Guided Optimization"
---

::: block
*Dynamic PGO - why's it taken so long to get here?* {style=background:red;width:500px}
::: 

---

### Why do I care about the order of code?

```
    public int FacA(int a)
    {
        if (a == 0)
          return 1;
        return a * FacA(a - 1);
    }

    public int FacB(int a)
    {
        if (a != 0)
            return a * FacB(a - 1);
        return 1;
    }
```

---


### Let's try them

- https://github.com/clivetong/Talks/blob/main/dynamicpgo/testfac/Program.cs



---



### Because the data matters



| Method |  a |       Mean |     Error |    StdDev |
|------- |--- |-----------:|----------:|----------:|
|   FacA |  0 |  0.0285 ns | 0.0175 ns | 0.0146 ns |
|   FacB |  0 |  0.2069 ns | 0.0316 ns | 0.0608 ns |
|   FacA | 10 | 14.4784 ns | 0.3166 ns | 0.2807 ns |
|   FacB | 10 | 11.0480 ns | 0.2441 ns | 0.3500 ns |


---

### Use profile Guided Optimization


- checkout CPython and build that
- use the testsuite to generate a profile
- recompile using this to guide decisions

---


### Strategy: Use profile information to guide us

In .NET for a while now (PGO)

1. Do a number of runs recording call patterns
2. Aggregate
3. Use to make decisions in the compiler

---

### But we can go further

1. Make the decisions for my run of the program (DPGO)

2. Inline lots and learn what you should not have done
    - and have a way to backout


---

### Why does inlining matter?

1. Calling conventions
    - multiple entry points in static code (not yet)

2. GC safe points
    - need to be clear what is a real pointer to a managed object at points threads can be stopped

3. The more inlining, the more further opportunities are exposed


---


### What do you need from the base platform?

1.  The ability to re-JIT code.
    - don't run the code, don't JIT it.
    - tiered compilation (tier0, tier1)

2. The ability to dynamically watch what the code is doing
    - a reflective runtime

3. On stack replacement (OSR - everywhere)
    - switch to new implementation


---



### Speculative optimization using DPGO

1. Add counters into the code

2. Re-JIT when we've seen it enough

3. Guarded compilation
   -  Trap when we didn't think we'd see it, but have  
        - most programs call the same type of instance at the same points 
        - old tech - PICs


---

### Use for

1. Devirtualization
2. Hot/cold block reordering
3. Aggressive inlining

---


### And the test

- https://github.com/clivetong/Talks/blob/main/dynamicpgo/dynamicpgo/Program.cs


---



|                  Method |          Job |          Mean |      Error |      StdDev |
|------------------------ |------------- |--------------:|-----------:|------------:|
|  ProfileDrivingInlining | Default | 3,816.0934 | 74.1844 | 209.2385 |
|  ProfileDrivingInlining | DPGO | 1,289.9320 | 25.7019 |  39.2495 |
|  HotColdBlockReordering | Default|     1.6144 |  0.0981 |   0.1310 |
|  HotColdBlockReordering | DPGO |     0.1374 |  0.0857 |   0.0760 |
| GuardedDevirtualization | Default|     6.2500 |  0.1522 |   0.2277 |
| GuardedDevirtualization | DPGO |     5.1451 |  0.1268 |   0.1303 |


---

### Env Vars

- DOTNET_TieredPGO
- DOTNET_TC_QuickJitForLoops
- DOTNET_ReadyToRun


---

### Obligatory mention

1. SELF 
    - prototype based language (old JS)
    - go extreme
    - dynamically de-optimise when debugging or too aggressive

---

### Which is better?

- PGO 
- DPGO
- attributed methods

---


### Read these

- https://gist.github.com/EgorBo/dc181796683da3d905a5295bfd3dd95b
- https://github.com/dotnet/runtime/issues/43618
- https://github.com/dotnet/runtime/pull/52708
- https://github.com/dotnet/runtime/pull/55478

---

### Reproduce the results 

- https://github.com/clivetong/Talks/tree/main/dynamicpgo
