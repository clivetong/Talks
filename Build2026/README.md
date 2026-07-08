# Some talks from Build 2026 (June 2-3)

---

## What are we covering?

We'll cover parts of a few of the talks from Build.

---

I'll rush through these talks from the [YouTube playlist](https://www.youtube.com/playlist?list=PLlrxD0HtieHicIn65R7Oi_1nFXQr4SbtU):

- [.NET 11 in depth: Runtime, libraries, and SDK for the AI era](https://www.youtube.com/watch?v=-zAYZ7GSjAs)
- [Build and ship faster with a developer-optimized experience on Windows](https://www.youtube.com/watch?v=V6mdr7Lw1TA)
- [What we learned shipping VS Code weekly (without breaking everything)](https://www.youtube.com/watch?v=hH4RiA7pk5Q)

---

### SDK

UX

- dotnet run for MAUI (via devices) and aware of context (LLM support)
- work well with worktrees

Performance

- NAOT performance (bundled tools)
- multi-threaded MSBuild
- NAtive AOT-ified dotnet

Acquisition

- CLI based SDK/Runtime installation and maintenance (dotnetup)
- reducing the size of SDK install

---

### Libraries

- Process API
- Unicode (Rune awareness, emojis)
- System.Text.Json (JSONL)
- Compression

---

### Runtime Async

- opt-in for 11; likely default for 12
- no source code changes, completely compatible with compiler async

```xml
  <features>runtime-async=on</features>
```

- cleaner stack traces
- performance
- augtomatically get async improvements

---

### Memory safety
