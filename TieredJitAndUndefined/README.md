---
transition: "slide"
slideNumber: false
title: "Undefined behaviour becomes even more undefined"
---

::: block
*Tell me about Undefined Behaviour* {style=background:red;width:500px}
:::

---

### What is undefined behaviour?

It's something that the language specification either doesn't specify or something that it explicitly says it isn't going to define

---

### And by a coincidence

[This post by Laurence Tratt](https://tratt.net/laurie/blog/2023/why_arent_programming_language_specifications_comprehensive.html?utm_source=programmingdigest&utm_medium&utm_campaign=1525)

---

### Where he mentions the categories

- deliberate flexibility
- semi-inevitable flexibility
- undesired flexibility
- unknown flexibility

---

### And the one that interests me is the memory model

This was left undefined in a number of languages when the multi-core revolution happened. Java was the first language to seriously consider it, but it is complicated.
The way caches work means you end up with a happens-before relationship.

---

### I write 1,2,3,4,5 to a memory location

Another thread reading might see
- 1,2,3,4,5
- 2,4,5
- 2,5

But will definitely  not see
- 5,4

---

### This code

[This code](https://github.com/clivetong/Talks/blob/master/TieredJitAndUndefined/Program.cs)

```
class Program
{
    private static bool _cancelLoop = false;
    private static int _counter = 0;

    private static void LoopThreadStart()
    {
        while (!_cancelLoop)
        {
#if !NOCOUNTER
            _counter++;
#endif
        }
    }

    static void Main()
    {
        var loopThread = Task.Run(LoopThreadStart);

        Thread.Sleep(5000);
        _cancelLoop = true;

        loopThread.Wait();
    }
}
```

---

### dotnet run -c Debug

```
C:\...\TieredJitAndUndefined > dotnet run -c Debug
C:\...\TieredJitAndUndefined >
```

---

### dotnet run -c Release

```
C:\...\TieredJitAndUndefined > dotnet run -c Release
.....
```


---

### dotnet run -c Release -p DEFINECONSTANTS="NOCOUNTER"

```
C:\...\TieredJitAndUndefined > dotnet run -c Release -p DEFINECONSTANTS="NOCOUNTER"
... compilation warning ...
C:\...\TieredJitAndUndefined >
```

---

### So where did the undefined behaviour get used?

- hoped that it was in the hardware)
- it's not even in the compiler
- but it is in the JIT
- ... and the behaviour changes over time

---

### Let's have a look

```
$env:DOTNET_JitDisasm="Program:LoopThreadStart"
```

---

### And the results

- the first checks the location
- the second starts checking, then recompiles to not check it
- the third one notices no side effects

---

### So the behaviour of code depends when we call it

```
dotnet build -c Release
windbg 
  sxe ld clrjit
  !bpmd ConsoleApp4 Program.LoopThreadStart
```
