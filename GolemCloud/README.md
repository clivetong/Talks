---
transition: "slide"
slideNumber: false
title: "WASM will disrupt your cloud via Golem"
---

::: block
*WASM will disrupt your cloud via Golem* {style=background:red;width:500px}
:::

---

### What will we be talking about?

We'll focus is on how WASM is being used in Golem cloud to make your cloud applications easier to write

---

### Some references

- Some of the material comes from [this talk at WASM I/O 2024](https://www.youtube.com/watch?v=fHPYetd3q2g) by John De Goes
- There's material on the Golem cloud blog about [Durable Computing](https://www.golem.cloud/post/what-is-durable-computing) and the [landscape](https://www.golem.cloud/post/the-emerging-landscape-of-durable-computing)
- Golem isn't the [only player in town](https://a16z.com/the-modern-transactional-stack/) - see for example [DBOS](https://www.golem.cloud/post/exploring-the-potential-of-stonebreaker-s-new-dbos)

---

### [Big Idea: much complexity stems from the fact that programs can fail half way through](https://www.youtube.com/watch?v=sDIXdVjJFN8&t=170s)

- Reliable, scalable, stateful, distributed systems need to account for (infrastructure) failure (for example, hardware failures/machine restarts/config changes/application code updates, network issues)
- This leads to complexity of application which use patterns like event sourcing and use shared databases to store state

- Instead we'd rather have a program that is guaranteed to run to completion

---

### [Durable computing in 4 phrases](https://www.golem.cloud/post/what-is-durable-computing)

- Invincible
- Failure and Recovery
- At Least Once
- Recovery latency

---

### Pushing the idea of Durable Workflows further

---

### Azure Durable Functions

- though alternatives like [Temporal.io](https://temporal.io/)

---

![Features](images/default-project.png)

---

![Features](images/start.png)

---

![Features](images/first-call.png)

---

![Features](images/into-first.png)

---

![Features](images/into-second.png)

---

![Features](images/keeps-state.png)

---

![Features](images/storage.png)

---

### Some Observations

- That looks like really strange code
- Durable state in Table Storage 
- Interactions with external systems means many possibilities for failure, so we require at least once, which leads to idempotency tokens

---

### How do we implement this?

![images](images/secret-sauce.png)

---

### What is WASM?

![image](images/wasm.png)

---

### Tell me more

- many languages compile down to it and interoperate using the component model

- [dotnet](https://devblogs.microsoft.com/dotnet/extending-web-assembly-to-the-cloud/)

---

![image](images/mural.png)

---

![image](images/how.png)

---

### Fabric of the Future

- Delete Event Sourcing
- Delete Databases
- Delete Kubernetes
- Delete Http/Json/GRpc

---

### More Links

- [Main repository](https://github.com/golemcloud/golem)
- [Examples](https://github.com/golemcloud/golem-examples)
- [Technical Details](https://learn.golem.cloud/docs/technical-details)
- [The WasmTime fork](https://github.com/golemcloud/wasmtime/tree/golem-wasmtime-17)
- [The WasmTime fork changes](https://github.com/bytecodealliance/wasmtime/compare/main...golemcloud:wasmtime:golem-wasmtime-17)
- [Watlings to understand more about the bytecode](https://github.com/EmNudge/watlings)
