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

- Mention Go and it's reverse

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

Regular expressions engine

---

### Aspire Unplugged

---

- Xmas time project for five people. What is the hardest part of developing and deploying cloud apps
- Human writes and maintains a lot of scripts - model in somethign with more structure to enable tooling

[Project Tye announcement](https://devblogs.microsoft.com/dotnet/introducing-project-tye/)
[Project Tye repository](https://github.com/dotnet/tye)

---

### A year in .NET security

---
