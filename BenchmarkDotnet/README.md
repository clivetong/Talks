---
transition: "slide"
slideNumber: false
title: "Benchmark.Net"
---

::: block
*Benchmark.Net* {style=background:red;width:500px}
:::

---

### What's the talk about?

How Stephen Toub used [Benchmark.net](https://github.com/dotnet/BenchmarkDotNet) in [his Microsoft Build session](https://youtube.com/watch?v=TRFfTdzpk-M&si=f_qi44B92f6hxnrt) to micro-optimize some code

---

### Interstitial

[Toub and Hanselman talk about performance and implementation on the dotnet YouTube channel](https://www.youtube.com/results?search_query=toub)

---

![Test the scenario](images/truncator.png)

---

### An ages old way to understand your application

- Occasionally break into the application and look at the call stack (but beware that it tells you where the application is going and not where it has been)

- Run a profiler to understand the higher level flow and where the time goes (sampling or instrumenting)

- Isolate down to the level of methods (but its hard to isolate enough)


---

```powershell
PS Test\bin\Release\net9.0> pushd ../../..
PS Test> dotnet build -c Release
PS Test> popd
PS Test\bin\Release\net9.0> .\ConsoleApp1.exe --filter *Test*
```

---

![old way](images/old-way.png)

---

![old way](images/new-way.png)

---

