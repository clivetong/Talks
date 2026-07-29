# Some talks from Build 2026 (June 2-3)

We'll cover parts of a few of the talks from Build.

---

I'll rush through these talks from the [YouTube playlist](https://www.youtube.com/playlist?list=PLlrxD0HtieHicIn65R7Oi_1nFXQr4SbtU):

- [.NET 11 in depth: Runtime, libraries, and SDK for the AI era](https://www.youtube.com/watch?v=-zAYZ7GSjAs)
- [Build and ship faster with a developer-optimized experience on Windows](https://build.microsoft.com/en-US/sessions/BRK261?source=sessions)
- [What we learned shipping VS Code weekly (without breaking everything)](https://www.youtube.com/watch?v=hH4RiA7pk5Q)

---

## .NET 11 in depth: Runtime, libraries, and SDK for the AI era

> Join Chet and Rich for a tour of how the .NET Runtime, libraries, and tooling are improving for .NET 11. You'll learn about investments in Native AOT, support for AI agents of all kinds, foundational runtime investments around memory safety and asyncronous operations, and a large number of new APIs that make working in .NET more enjoyable and productive.
---

### UX

- dotnet run for MAUI (via devices)
- aware of context (LLM support, output formats)
- work well with worktrees

---

### Performance

- Native AOT performance (bundled tools)
- multi-threaded MSBuild
- Native AOT-ified dotnet

---

### Acquisition

- CLI based SDK/Runtime installation and maintenance (dotnetup)
- reducing the size of SDK install

---

### Simplifying .NET install using dotnetup

- [The design document](https://github.com/dotnet/designs/blob/main/accepted/2026/dotnetup/cli-acquisition-tool.md)
- [Video discussing it](https://youtu.be/eExkCpyUrrs?si=k_K6RZX4nvwDemyZ)

---

### Libraries

- Process API
  - avoid deadlocks
  - allow easy connection using anonymous pipes
- Unicode (Rune awareness, emojis)
- System.Text.Json (JSONL)
- Compression

---

[Process Api blog post](https://devblogs.microsoft.com/dotnet/process-api-improvements-in-dotnet-11/)

---

![Anonymous pipes](images/anonymouspipes.png)

---

### Runtime Async

- opt-in for 11; likely default for 12
- no source code changes
- completely compatible with compiler async

- [Design document](https://github.com/dotnet/runtime/blob/main/docs/design/specs/runtime-async.md)

---

```xml
  <features>runtime-async=on</features>
```

---

- cleaner stack traces
- performance
- automatically get async improvements
- the frontend marks the methods
- the state machine is written by the JIT
  - shared with Native AOT

---

- [.NET 11 preview 4 - runtime libraries](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview4/runtime.md#runtime-libraries-are-now-compiled-with-runtime-async)
- [.NET 11 preview 6 - runtime libraries](https://github.com/dotnet/core/blob/main/release-notes/11.0/preview/preview6/runtime.md#runtime-async-performance-improvements)

---

### Memory safety

- two release project
  - .net 11 reduce use of unsafe, and apply to CoreLib
  - .net 12 Update rest of the product
- C# 16 redesigns unsafe into a reviewable caller contract

---

### JIT improvements

- bounds check work

---

![SIMD gets faster](images/memorysafesimd.png)

---

## Build and ship faster with a developer-optimized experience on Windows

> Learn how Windows delivers a streamlined, end-to-end experience through PCs and OS experiences optimized for developers. See our new experiences in action across WSL, PowerToys and your favorite developer tools so you can code with less toil and stay in your workflows. Walk away with repeatable scenarios that you can easily integrate in your daily development workflows, helping you scale your AI projects with confidence.

---

### Powertoys/terminal/winget/wsl

- Building on windows
- Building for windows

---

![Building on windows](images/building-on-windows.png)

---

[A config for winget to get the PC into a standard configuration](https://github.com/microsoft/windowsdeveloperconfig)

---

### Insider program

- Taskbar personalization (left/right/top/bottom)
- New run dialog - takes after the powertoys command palette
- [Intelligent terminal](https://github.com/microsoft/intelligent-terminal) has AI assistant in a sub-pane

---

## WSL containers

```Powershell
C:\Users\clive.tong\Documents\git\Talks [main ≡ +2 ~1 -0 !]> wslc
Copyright (c) Microsoft Corporation. All rights reserved.
For privacy information about this product please visit https://aka.ms/privacy.

WSLC is the Windows Subsystem for Linux Container CLI tool. It enables management and interaction with WSL containers from the command line.

Usage: wslc  [<command>] [<options>]

The following commands are available:
  container  Manage containers.
  image      Manage images.
  network    Manage networks.
  registry   Manage registry credentials.
  settings   Open the settings file in the default editor.
```

---

- [Public preview](https://devblogs.microsoft.com/commandline/wsl-container-is-now-available-for-public-preview/)

- [Failed if you have a . in your username](https://github.com/microsoft/WSL/issues/40944)

---

```Powershell
C:\Users\clive.tong\Documents\git> wslc run --rm hello-world
Image 'hello-world' not found, pulling
latest: Pulling from library/hello-world
4f55086f7dd0: Pull complete
Digest: sha256:c3cbe1cc1aa588a64951ac6286e0df7b27fe2e6324b1001c619bb358770c0178
Status: Downloaded newer image for hello-world:latest

Hello from Docker!
```

---

```Powershell
C:\Users\clive.tong\Documents\git\Talks\Build2026 [main ≡ +1 ~1 -0 !]> wslc container run --rm -it ubuntu:latest bash
Image 'ubuntu:latest' not found, pulling
latest: Pulling from library/ubuntu
ed819469700f: Pull complete
a3679419df18: Pull complete
Digest: sha256:3131b4cc82a783df6c9df078f86e01819a13594b865c2cad47bd1bca2b7063bb
Status: Downloaded newer image for ubuntu:latest
root@73a51ef5eac1:/#
```

---

![Why?](images/whywslc.png)

---

- They want to make the API and the CLI work together
- Open source and will push upstream
- Opinionated
- Extended with new commands

```Powershell
wslc system session list
```

---

[See this talk: WSL improvements and the new Containers CLI and APIs | DEM346](https://www.youtube.com/watch?v=i0M13ZvL04M)

---

![Why?](images/whywslc2.png)

---

![Who for](images/whofor.png)

---

## coreutils

[70+ different tools](https://github.com/microsoft/coreutils)

```Powershell
C:\Users\clive.tong\Documents\git\Talks\Build2026 [main ≡ +1 ~1 -0 !]> ls 'C:\Program Files\coreutils\bin\' | wc -l
78
```

```Powershell
C:\Users\clive.tong\Documents\git> test -f .\counter.csv
C:\Users\clive.tong\Documents\git> $LASTEXITCODE
0
C:\Users\clive.tong\Documents\git> test -f .\counter2.csv
C:\Users\clive.tong\Documents\git> $LASTEXITCODE
1
```

---

```Powershell
:\Users\clive.tong\Documents\git\Talks\Build2026 [main ↑1 +2 ~0 -0 !]> find . -iname "README.*" -exec grep -i dotnet "{}" ";" | grep -Eo "^.{30}" | tail -5
[The green threads write up](h
[A Microsoft bug that broke th
- [Different caller and callee
- [Access to locals because th
- [GC Info and hot/cold splitt
```

---

## Building for Windows

---

![Building for windows](images/building-for-windows.png)

---

### Jump start building windows app - like publishing

- [winappcli](https://github.com/microsoft/winappcli)

- [Some skills too](https://github.com/microsoft/win-dev-skills)

---

### Sample profile guided optimization

- Discusses instrumented code for profile guided optimization and the branch in the pipeline, and the friction this causes
- SPGO uses the hardware counters (and ETL events)
- They have used this for Adobe photoshop
- C/C++

---

- Will use a stack based VM to demo the improvement, calculating Fibonacci
- Uses xperf to capture ETL traces around a run of the application

---

[See learn](https://learn.microsoft.com/en-us/cpp/build/sample-profile-guided-optimization?view=msvc-170)

---

Because SPGO profiles release bits instead of instrumented builds, it enables much more flexibility in where and how you collect data. You can gather runtime profiles from production servers, developer machines, performance labs, or any combination. The result is a binary that runs hot paths more efficiently, with a typical performance speedup of 5-15% depending on the quality of the profile data.

---

Best candidates for SPGO: Large, branch-filled C/C++ applications with tight inner loops. Gains scale with codebase size and branch complexity. The small sample in this tutorial shows around 7% improvement. Larger production codebases often see more improvement.

---

### WSL containers integrated with build

- WSL.Containers NuGet package and some in the project file
- Will now build the containers as part of the publish
- Write C# code to start a Session and run the Linux container
- Runs in its own WSL VM

---

![wslc during build](images/wslcinapp.png)

---

![isolated](images/ownvm.png)

---

[Running the container - the Herbert Stock example](https://github.com/microsoft/Build26-DEM346-whats-new-in-windows-subsystem-for-linux/blob/main/src/demos/herbert-stock-trader/Services/ContainerService.cs)

- links to a session

---

## What we learned shipping VS Code weekly (without breaking everything)

> Shipping faster sounds great until test gaps, review bottlenecks, and triage backlog scale with it. The VS Code team hit all of that going from monthly to weekly releases, and agents made it work. This session breaks down the real patterns: agent sessions before meetings, conversations that become PRs instead of specs, automated triage across one of GitHub's largest repos, and harnesses that keep quality high when 100+ commits land daily. Concrete workflows you can take back to your team.

---

### How the team evolved as they ramped up the use of AI

- They adopted Agents, now what?
- AI changes how teams work, not just how they code

---

![Code survival](images/codesurvival.png)

---

![AI increased the load](images/issues.png)

---

### After ten years, moved from monthly to weekly releases

- 3x more issues and PRs
- get smaller batches

---

### It forced changes across

- Inner loop
- AI-native engineering system
- Planning and collaboration

---

### Developer inner loop

- Restructure so you don't need to build whole product to test each PR
- GitHub Mobile App to look at PR and see the UI changes that an Agent has put into it
- AI means you can test on Apple without having a Mac
- They extracted lots of components to allow them to be tested independent of the product
- Use the Agent to build prototypes
  - makes collaboration easier
  - no longer write specification doc
  - prototyping is now very cheap
- Agent does checks for perf regressions
  - a skill triggered on certain changes for example
  - perf knowledge no longer centered on one person (common theme)

---

### Checking new models

![Optimize models](images/optimize.png)

---

- VSC-Bench to understand change of prompt on token consumption

---

### AI-native teams need an AI-native engineering system

- lots of AI tooling using Agentic workflows
- model trained to infer owner of an issue

---

- GitHub events (issues and PRs etc)
- scheduled jobs (find an fix error with Errors Agent, de-duplicate issues)
- manual events (including chrome extension)

---

### Changes to collaboration model

- No longer quarterly plan
  - now even daily plan

---

![Pre-AI](images/preai.png)

---

![Now](images/now.png)

---

- When building becomes cheaper, judgment becomes scarce

- Moved to faster alignment loops instead of heavier plans

---

![Change how work works](images/changehowwork.png)
