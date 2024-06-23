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

![Test the scenario](images/truncator.png)

---

```powershell
PS C:\Users\clive.tong\Documents\git\Talks\BenchmarkDotnet\Test\bin\Release\net9.0> pushd ../../..
PS C:\Users\clive.tong\Documents\git\Talks\BenchmarkDotnet\Test> dotnet build -c Release
PS C:\Users\clive.tong\Documents\git\Talks\BenchmarkDotnet\Test> popd
PS C:\Users\clive.tong\Documents\git\Talks\BenchmarkDotnet\Test\bin\Release\net9.0> .\ConsoleApp1.exe --filter *Test*
```

---

![old way](images/old-way.png)

---

![old way](images/new-way.png)
