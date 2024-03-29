---
transition: "slide"
slideNumber: false
title: "Zod and the TS type system"
---

::: block
*Lessons from a few lines of code* {style=background:red;width:500px}
:::

---

### Why's TypeScript's Type System Different?

- [See the Hejlsberg talk at ICFP](https://www.youtube.com/live/d0zFruedB-w?si=BaKI1LX3F3KKXZBD&t=763)
- This is a type system for the sake of tooling 
  - not runtime safety or compilers
- Arbitrary limits to keep it feeling fast

---

![Features](images/features.png) 

---

![Types](images/types.png) 

---

### And much more

- [And the rather cool infer](https://blog.logrocket.com/understanding-infer-typescript/)

- [And it is Turing Complete](https://itnext.io/implementing-arithmetic-within-typescripts-type-system-a1ef140a6f6f)


---

### But we are just going to look at Zod

- some parts implemented as in Zod, some aren't
  - `npm run start`
  - simple-zod.ts

---

### References

- [A partial "how to build Zod" talk](https://youtube.com/watch?v=6zojOpZGrtg&si=1IlO3qiuvJlHW7dd)
- [TypeLevel TypeScript](https://github.com/gvergnaud/type-level-typescript-workshop)
- [Reconstructing TypeScript](https://jaked.org/blog/2021-09-15-Reconstructing-TypeScript-part-1)
