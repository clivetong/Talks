---
transition: "slide"
slideNumber: false
title: "Undefined behaviour becomes even more undefined"
---

::: block
*Tell me about Undefined Bahviour* {style=background:red;width:500px}
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

### This code

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

```
dotnet run -c Debug

dotnet run -c Release

dotnet run -c Release -p DEFINECONSTANTS="NOCOUNTER"

```

---

```
set DOTNET_JitDisasm=Program:LoopThreadStart
```

