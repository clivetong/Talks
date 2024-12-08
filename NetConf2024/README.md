---
transition: "slide"
slideNumber: false
title: "Some talks from .NET Conf 2024 (November 12-14)"
---

::: block
*.NET Conf 2024 Recap* {style=background:red;width:500px}
:::

---

### What's the talk about?

We're going to look at some of the runtime, libraries and languages talks from .NET Conf 2024, and talk through some of my highlights.

---

These are the talks from the [playlist](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oXeSG8BgCVru3zQtw_K4ANY):

- [What's new in the .NET Runtime, Libraries, & SDK](https://www.youtube.com/watch?v=4iEqqPZKDC0)
- [Performance Improvements in .NET 9](https://www.youtube.com/watch?v=aLQpnpSxosg)
- [C#'s Best features you might not be using](https://www.youtube.com/watch?v=yuXw7oj0Bg0)
- [New Features in the .NET 9 JIT](https://www.youtube.com/watch?v=1bsTnaLchi4)

---

### TALK: What's new in the .NET Runtime, Libraries, & SDK

---

### Runtime

---

### Server GC less aggressive in taking memory

In practical terms, .NET 8 has a bias to starting off big and .NET 9 is the opposite.

![Picture](images/gc.png)

- there is more bookkeeping
- can configure Server GC to still use old algorithm

---

![config](images/gc.png)

---

![Datas](images/datas.png)

---

![Conserve memory](images/loh-compact.png)

---

### RyuJIT and Profile Guided Optimization

- DPGO ("The .NET re-compiler") has fast path and common path
- focus this time on casts and loops

---

### The Host ("myapp.exe")

Enabled control-flow enforcement technology by default on windows.

- Hardware protection againt ROP
- Small cost but recommended

---

### .NET Install Search Behaviour

Previously deploy self-contained apps, but not can have multiple apps sharing a framework install

![Search](images/search.png)

---

### Libraries

---

- Lots of "spanification" (with .AsSpan())
- Avoids allocations of things like strings which can make a perf difference
- But how do we use spans for lookups

---

### Alternate Lookup

![Alternate lookup](images/alternate.png)

---

![Alternate demo](images/demoalternate.png)

---

### Debug.Assert tells you what failed

Using a combination of CallerArgumentExpression and OverloadResolutionPriority

![image](images/debug-with-message.png)

![debug8](images/debug8.png)

---

### Linq Index expression for ForEach

```CSharp
foreach(var (index,persion) in people.Index())
```

---

### Iterate through Tasks as they finish 

![WhenEach](images/wheneach.png)

---

### `JSONSchemaExporter` from C# to JSON Schema

TransformSchemaNode to adjust types.

No runtime support to check the compliance yet

![schema](images/type-for-schema.png)

---

### BinaryFormatter is gone!

An Unsupported Compat package to bring back the functionaliy

---

### SDK

---

### Package vulnerability auditing now does transitive dependencies

So expect more warnings when restoring

Some System.* packages have false positives

---

### Terminal Logger has pretty colours and pretty summaries

What can I say.

---

### Roll forward tools to new versions of the framework

Tools are a special kind of NuGet package and are currently bound to a .NET version

```csharp
dotnet tool install --allow-roll-forward ...

dotnet tool run --allow-roll-forward ...
```

---

### dotnet publish can now publish to insecure (http) registries

![Insecure](images/insecure.png)

---

### Talk: Performance Improvements in .NET 9

---

[The yearly Toub post on the performance improvements](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-9/)

---

### Initial focus on GetAlternateLookup

Required changes to language, libraries and runtime

---

### Demo

Parse the words of Pride and Prejudice and calculate the words and their counts.

Use a generated RegExp parser to parse the words

![regexp sg](images/regexp-sg.png)

---

![version 1](images/version1.png)

---

### But this is allocating, so use Spans

"The" occurs 4000 times, but we need to only generate a string for it once

---

![get alternate](images/getalternate.png)

---

![and use](images/and-use.png)

[Same speed but no allocation]

---

![ialternate](images/ialternate.png)

- Equals
- GetHashCode
- Create

---

### You've used String.Split but it is allocation heavy.

![enumerate splits](images/enumerate-splits.png)

![convert range to string](images/convert-range-using-indexer.png)

![range-to-span](images/range-to-span.png)

---

### And we can avoid the double lookup

![avoid double lookup](images/avoid-double-lookup.png)

Memory safe but beware dictionary mutation

---

### Toub's two favourite .NET 8 features

- Turning on DPGO

- The SearchValues type

---

### SearchValues

- lets the system precompute for the type you ask for. 
- for bytes and char in .NET 8 (used in HTTP header parsing). 
- in .NET 9, `SearchValues<string>`

---

![without searchvalues](images/without-searchvalues.png)

---

![with searchvalues](images/with-searchvalues.png)

[A 200x speed improvement]

---

### Regexps can use this functionality

(and note the use of partial property)

![in regexp](images/in-regexp.png)

![regexp searchvalues](images/regexp-searchvalues.png)

---

### `TensorPrimitives.<T>`

- Full set of math operations

- And as generic methods and not just floating point

[All works on top of Span and ReadOnlySpan]

[Vectorized when possible]

---

Shows Hamming distance where hand rolled loop is 80ms and vector extensions using TensorPrimitve is 2ms (on 256 AV machine)

---

### Talk: C#'s Best features you might not be using

---

High bar for backwards compatibility

Some things for the last few releases

- Pattern matching
- File scoped namespaces
- Init and required
- String literals
- Records
- Collection Expressions

---

### Pattern Matching

Express your intent

Switch expressions give warning if all values not matched. Also if a pattern is redundant.

![Patterns](images/patterns.png)

Bug in the second method - subsumed warning soon

---

### File scoped namespaces

Remove a complete layer of indent

Turn off whitespace diff when comparing

---

### init and required

Makes the intent clearer, and avoids the nullable warning

![required and init](images/required-init.png)

---

### Strings

- quoted
- verbatim
- interpolated
- raw

---

![raw string](images/raw-string.png)

- 3 or more quotes needed
- whitespace ignored on first line and up to indent of the closing

---

![interpolated](images/interpolated.png)

---

![interpolated json](images/interpolated-json.png)

- Note double brackets to avid clash with JSON bracket (and the double $)

---

### Records

A single approach for data oriented types

- Correct and fast equality
- Withers - non-destructive mutation
- Readable consistent ToString() output
- Concise declaration
- Both struct and class records
- Synthesises several members for correctness and performance

---

[See a Sharplab decompilation](https://sharplab.io/#v2:D4Jwpgxg9iAmAEBhArgZwC5QLZhACgAEBGABngDsBDHASgG4g===)

---

### Primary contructors

Parameters converted to public fields

---

### Collection Expressions

![before collection](images/before-collection.png)

---

![after collection](images/after-collection.png)

---

### Talk: New Features in the .NET 9 JIT

---

![jit agenda](images/jit-agenda.png)

---

![command line](images/command-line.png)

---

### Object stack allocation

If escape analysis proves that the lifetime is constrained by the lifetime of the stack frame, then allocate on the stack

- possible field promotion
- aggressive optimization

(see example in talk of allocating a rectangle, and everyting is optimized away)

---

### Cobalt-100 on Azure

See the [article](https://learn.microsoft.com/en-us/azure/virtual-machines/sizes/cobalt-overview)

ARM based machine for your workloads

Offers SVE extension and the JIT can use them in some loops for vector processing.

![sve](images/sve.png)

---

### Jitted code

Fewer instructions, and use of fewer registers

---

### DPGO

- .NET 8 DPGO was about virtual calls and interface calls

- .NET 9 extend to casting

[Tier 0 watches for the common types in the cast, and then optimze for this case]

---

### Inlining

Inlining for shared generics

![inline shared generics](images/inline-shared-generics.png)

---

~1100 changes to the codegen between .NET 8 and .NET 9

All of the improvements are yours automatically

[Stack allocation enhancements](https://github.com/dotnet/runtime/issues/104936)

[JIT Focus areas for .NET 10](https://github.com/dotnet/runtime/issues/108988)
