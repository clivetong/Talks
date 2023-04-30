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

---

```
; Assembly listing for method Program:LoopThreadStart()
; Emitting BLENDED_CODE for X64 CPU with AVX - Windows
; Tier-0 compilation
; MinOpts code
; rbp based frame
; fully interruptible

G_M000_IG01:                ;; offset=0000H
       55                   push     rbp
       4883EC60             sub      rsp, 96
       488D6C2460           lea      rbp, [rsp+60H]

G_M000_IG02:                ;; offset=000AH
       C745C0E8030000       mov      dword ptr [rbp-40H], 0x3E8
       EB06                 jmp      SHORT G_M000_IG04

G_M000_IG03:                ;; offset=0013H
       FF05B7C30C00         inc      dword ptr [(reloc 0x7ff8cd5fd360)]

G_M000_IG04:                ;; offset=0019H
       8B4DC0               mov      ecx, dword ptr [rbp-40H]
       FFC9                 dec      ecx
       894DC0               mov      dword ptr [rbp-40H], ecx
       837DC000             cmp      dword ptr [rbp-40H], 0
       7F0E                 jg       SHORT G_M000_IG06

G_M000_IG05:                ;; offset=0027H
       488D4DC0             lea      rcx, [rbp-40H]
       BA0E000000           mov      edx, 14
       E8DB84A45F           call     CORINFO_HELP_PATCHPOINT

G_M000_IG06:                ;; offset=0035H
       0FB60598C30C00       movzx    rax, byte  ptr [(reloc 0x7ff8cd5fd364)]
       85C0                 test     eax, eax
       74D3                 je       SHORT G_M000_IG03

G_M000_IG07:                ;; offset=0040H
       4883C460             add      rsp, 96
       5D                   pop      rbp
       C3                   ret

; Total bytes of code 70
```

---

```
; Assembly listing for method Program:LoopThreadStart()
; Emitting BLENDED_CODE for X64 CPU with AVX - Windows
; Tier-1 compilation
; OSR variant for entry point 0xe
; optimized code
; rsp based frame
; fully interruptible
; No PGO data

G_M000_IG01:                ;; offset=0000H

G_M000_IG02:                ;; offset=0000H
       0FB6055DC30C00       movzx    rax, byte  ptr [(reloc 0x7ff8cd5fd364)]
       85C0                 test     eax, eax
       750E                 jne      SHORT G_M000_IG04
       48B860D35FCDF87F0000 mov      rax, 0x7FF8CD5FD360
                            align    [0 bytes for IG03]

G_M000_IG03:                ;; offset=0015H
       FF00                 inc      dword ptr [rax]
       EBFC                 jmp      SHORT G_M000_IG03

G_M000_IG04:                ;; offset=0019H
       4883C468             add      rsp, 104
       5D                   pop      rbp
       C3                   ret

; Total bytes of code 31
```
