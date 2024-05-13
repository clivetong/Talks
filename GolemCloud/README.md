---
transition: "slide"
slideNumber: false
title: "WASM will disrupt your cloud"
---

::: block
*WASM will disrupt your cloud* {style=background:red;width:500px}
:::

---

### What will we be talking about?

The main focus is on how WASM is being used in Golem cloud to make your cloud applications easier to write

---

### Some references

- Some of the material comes from [this talk at WASM I/O 2024](https://www.youtube.com/watch?v=fHPYetd3q2g) by John De Goes
- There's material on the Golem cloud blog about [Durable Computing](https://www.golem.cloud/post/what-is-durable-computing) and the [landscape](https://www.golem.cloud/post/the-emerging-landscape-of-durable-computing)
- Golem isn't the [only player in town](https://a16z.com/the-modern-transactional-stack/) - see for example [DBOS](https://www.golem.cloud/post/exploring-the-potential-of-stonebreaker-s-new-dbos)

---

### [Durable computing in 5 phrases](https://www.golem.cloud/post/what-is-durable-computing)

- Invincible
- Failure and Recovery
- At Least Once
- Durable State

---

### Azure Durable Functions

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

### Any observations

- That looks like really strange code
- Durable state in the database
- Requires token for Idempotency

---
