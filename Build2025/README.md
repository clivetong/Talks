---
transition: "slide"
slideNumber: false
title: "Some talks from Build 2025 (May 19-22)"
---

::: block
*Build Recap of some sessions* {style=background:red;width:500px}
:::

---

### What are we covering?

We'll cover parts of a few of the talks from Build.

---

### Quick Impression of Build

Not as in-depth tech as in previous years, this one covered the arrival of AI into the business and developer experience.

- Agents, agents, agents
- Orchestration of Agents
- Agents talking to tools (MCP) or Agents (A2A)
- CoPilot taking on issues and working collaboratively to solve a problem

---

### Weirdest things

[Running .NET source files `dotnet run app.cs`](https://www.youtube.com/watch?v=98MizuB7i-w)

I just don't get it. You have scripting languages and programming languages, and very few can straddle the divide.

---

These are the talks from the [playlist](https://www.youtube.com/playlist?list=PLFPUGjQjckXH1BDmT9hZw_fUi9NZRbVJt):

- [Yet Another "Highly Technical Talk" with Hanselman and Toub](https://www.youtube.com/watch?v=J3IQBI5HVOw)
- [Python Meets .NET: Building AI Solutions with Combined Strengths](https://www.youtube.com/watch?v=fDbCqalegNU)
- [A 10x Faster TypeScript with Anders Hejlsberg](https://www.youtube.com/watch?v=UJfF3-13aFo)
- [Inside Azure innovations with Mark Russinovich](https://build.microsoft.com/en-US/sessions/BRK195?source=sessions)

---

### The Highly Technical Talk

- Hanselman and Toub are really good as a presenting pair
- This time they implement (most of) Channels - like the Go feature that ripped off CSP

---

### Key Ideas

- Version 1
- Version 2
  - Use locks to preserve your invariants
  - TaskCompletionSources for someone to wait on
  - ValueTasks when you control the consumers
- Version 3 (WaitToReadAsync and TryRead)
- Version 4
  - Reentrant locks and invariants
- Version 5
  - `TaskCreationOptions.RunContinuationsAsynchronously`
- Version 6
  - lock free
