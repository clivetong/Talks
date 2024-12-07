---
transition: "slide"
slideNumber: false
title: "Some talks from .NET Conf 2024"
---

::: block
*.NET Conf 2024 Recap* {style=background:red;width:500px}
:::

---

### What's the talk about?

We're going to look at some of the runtime, libraries and languages talks from .NET Comnf 2024, and talk through some of the highlights.

---

These are the talks from the [playlist](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oXeSG8BgCVru3zQtw_K4ANY):

- [What's new in the .NET Runtime, Libraries, & SDK](https://www.youtube.com/watch?v=4iEqqPZKDC0)
- [Performance Improvements in .NET 9](https://www.youtube.com/watch?v=aLQpnpSxosg)
- [C#'s Best features you might not be using](https://www.youtube.com/watch?v=yuXw7oj0Bg0)
- [New Features in the .NET 9 JIT](https://www.youtube.com/watch?v=1bsTnaLchi4)

---

### What's new in the .NET Runtime, Libraries, & SDK

---

### Server GC less aggressive in taking memory

In practical terms, .NET 8 has a bias to starting off big and .NET 9 is the opposite.

![Picture](images/gc.png)

- there is more bookkeeping
- can configure Server GC to still use old algorithm

---

### RyuJIT and Profile Guided Optimization

"The .NET re-compiler"

- DPGO has fast path and common path
- focus this time on casts and loops (strength reduction, induction variable widening)

---

### The Host

"myapp.exe"

Enabled control-flow enforcement technology by default on windows.

- Hardware protection againt ROP (Return Oriented Programming)
- small cost but recommended

---

### .NET Install Search Behaviour

Previously deploy self-contained apps, but not can have multiple apps sharing a framework install

![Search](images/search.png)

###

- Lots of "spanification" (with .AsSpan())
- Avoids allocations of things like strings which can make a perf difference
- But how do we use spans for lookups

---

### Alternate Lookup

![Alternate lookup](images/alternate.png)

---

Debug.Assert tells you what failed

Using a combination of CallerArgumentExpression and OverloadResolutionPriority

---

Linq Index expression for ForEach

```CSharp
foreach(var (index,persion) in people.Index())
```
---

![WhenEach](images/wheneach.png)

---

JSONSchemaExporter from C# to JSON Schema `JSONSchemaExporter`

TransformSchemaNode to adjust types.
No runtime support to check the compliance yet

![schema](images/type-for-schema.png)

---

BinaryFormatter is gone!

unsupported Compat package is bring back the functionaliy

---

Package vulnerability auditing now does transitive dependencies

So expect more warnings when restoring

Some System.* packages have false positives

---

Terminal Logger has pretty colours and pretty summaries

---

dotnet tool install --allow-roll-forward ...

dotnet tool run --allow-roll-forward ...

Tools a special kind of NuGet package

Currently bound to a .NET version

---

dotnet publish can now publish to insecure (http) registries

![Insecure](images/insecure.png)

---

### Performance Improvements in .NET 9

---

### C#'s Best features you might not be using

---

### New Features in the .NET 9 JIT

---
