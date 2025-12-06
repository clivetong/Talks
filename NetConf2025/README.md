---
transition: "slide"
slideNumber: false
title: "Some talks from .NET Conf 2025 (November 11-13)"
---

::: block
*.NET Conf 2025 Recap* {style=background:red;width:500px}
:::

---

### What's are we covering?

We're going to look at some of the runtime, libraries and languages talks from .NET Conf 2025, and talk through some of my highlights.

---

These are the talks from the [playlist](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oXtIlvq1tuORUtZqVG-HdCt):

- [Performance Improvements in .NET 10](https://www.youtube.com/watch?v=snnULnTWcNM)
- [Aspire Unplugged with David and Maddy](https://www.youtube.com/watch?v=dJdXdRiIfDw)
- [A Year in .NET Security (2024–2025)](https://www.youtube.com/watch?v=8lFm4wI1bPo)

---

### Performance Improvements

---

[Deabstraction issue](https://github.com/dotnet/runtime/issues/108913)

[Some design notes](https://github.com/dotnet/runtime/blob/main/docs/design/coreclr/jit/DeabstractionAndConditionalEscapeAnalysis.md)

[Support for devirtualizing array interface methods](https://github.com/dotnet/runtime/pull/108153)

---

### De-abstraction

- Bring together previous work and extend it!
- Show the array example and the front end work (sharplab.io)

---

### Here's an example

- Show the test3 example

```Csharp
  dotnet run -c Release -f net48
  dotnet run -c Release -f net9.0
  dotnet run -c Release -f net10.0
```

---

- Stack allocation via better escape analysis
- GDV (guarded devirtualization)
- PGO improvements
- Inlining has improved

---

### Stack allocation

```CSharp
record class Number(int x);

void Test()
{
    var inst = new Number(20);
    return inst.x;
}
```

- Mention Go and it's reverse use of escape analysis

---

### GDV

```CSharp
interface IFoo { int Call(); }

class A : IFoo { int Call() => 42; }

int Test(IFoo target) => target.Call();

int Test(IFoo target) =>
  if (typeof(target) == typeof(A))
  {
    return 42;
  }
  else
  {
    target.Call();
  }
```

---

### And we used heuristics to decide to do it

```CSharp
int Test(IFoo target) 
{
  // Record count of the instance types
  // When we have enough data, trigger a recompliation
  target.Call();
}
```

---

### And see that in practice

```CSharp
$env:DOTNET_JitDisasm="<<Main>$>g__Test3|0_2"
```

---

### Lots of the benefits were inlining

- Function calls have an overhead
- We can see through to what is actually happening

---

### What does it mean for me?

- It is hard to correctly grasp the cost model
- Decisions are heuristics and may change over time

---

### Work over lots of the collection types

- Stack<>
- Queue<>
- ConcurrentDictionary<,>

- Improved the enumerators but also lots more

---

### Call out to CollectionsMarshal

- to get to the underlying data structure

---

### LINQ - what is the cost model?

[Ad hoc improvements](https://github.com/dotnet/runtime/issues/100378)

---

```CSharp
Enumerable.Range(0,100).OrderBy(x => -x).First()
```

- We don;t really need to sort
- From .NET Core 3 they've passed information between the query operators
- ... not really implemented as a pipeline

---

### Extended these optimizations

```CSharp
Enumerable.Range(0,100).OrderBy(x => -x).Contains(42)
```

- and Reverse (so it doesn't take a full copy)

---

### Regular expressions engine

- Toub uses the source code that the Regexp source generator outputs (added in .NET 7)
- Greedy loops and backtracking - better recognising when backtracking won't help (atomic)
- Remove unnecessary work

---

### Aspire Unplugged with David and Maddy

---

## How did it all start?

- Xmas break project for five people.
- What is the hardest part of developing and deploying cloud apps?
- Decided "Human writes and maintains a lot of scripts" - model in something with more structure to enable tooling
- The human orchestrates the yaml for cloud, and docker compose file for local dev and lots of other scripts

---

## Project Tye

- [Project Tye announcement](https://devblogs.microsoft.com/dotnet/introducing-project-tye/)
- [Project Tye repository](https://github.com/dotnet/tye)

---

## Aspire

- Ideas from Tye, Kubernetes, Cloud Native, ...
- Didn't know how good it would be for general things? 
- How good it wuld be for mobile development didn't occur to them at the time
- Brilliant for onboarding - no instructions, just F5 the code

---

### Window Forms and IIS. Why not a target for Aspire?

---

## IIS

- Focus on bigger apps that not just database and frontend
- Thinking eShop - multi-repo, multi-database
- IIS not in that space
- Not a never, but a not now.
- Currently focus on container based applications
- The model would allow someone in the community to build it.

---

- Build experiences or buid the tooling to build the experiences
- Focus on one vertical with ACA
- Polygot came around naturally because of JavaScript frontends
- No longer ~.NET~ Aspire
- Again picking one language, Python, helped flesh out what is needed for any language
- Small team, hence need to prioritize and build verticals

---

- Python release in 13 took ages to figure out the problems
- JavaScript could be done in the two weeks prior to release

---

## When is the dashboard getting persistence?

- Coming soon!

---

## How do I get things to keep running when I shut the dashboard?

- Containers can be made persistent.
- Persistent execitables will come.
- They'd like the dashboard to be persistent.

---

## What has been the most challenging thing?

- the Resource model - AddDatabase, AddXXX
- hit publish and convert to a manifest
- then waitfors and dependencies complicated it all
- They would like to get hot reload; currently a one pass apphost
- will eventually get to reconciliation loops
- how do they evolve in place?

---

- on the fourth version of deployment

---

## Why no apphost referencing another apphost?

- That would handle multi-repo
- And will happen eventually
- They have changed to Aspire as tool more than an application framework (no longer service defaults as a central thing)
- More often than not, you are retro fitting Aspire to an existing project
- Aspire should be non-intrusive

---

## They adverised using the VS Code extension

---

## Dashboard initially advertised as development only. Now we see it in production. What for the future?

- Year one the dashboard caught everyone's eye
- People always have a "is it working dashboard"
- ie did it deploy correctly.
- For big appliaction telemetry need real APM tooling
- But for quick feedback it is great - "JIT debugging"
- It isn't Honeycomb
- It will stay optimised for local development

---

### A year in .NET security

---

## Terms

[CVE](https://www.cve.org/)
[CVSS](http://nvd.nist.gov/vuln-metrics/cvss)
[CWE](http://cwe.mitre.org/)  Common Weaknesses

---

## This year

- 84 reports to MSRC
- 12 CVEs

---

![Vulnerabilities By Type](images/ByType.png)

---

- Dorrans then goes through them all, linking out to the fix on GitHub

---

## One I suspect we have on our code

- [An API that worked on Windows but not on Linux](https://github.com/dotnet/msbuild/pull/12688)

![Bad permissions](images/dos.png)

---

- Must admit I've seen AI generated code that uses the wrong API.

---

## And a 9.9

![Smuggling](images/smuggling.png)
