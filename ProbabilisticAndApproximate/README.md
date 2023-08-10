---
transition: "slide"
slideNumber: false
title: "What! Quantum Computers Can't Do That?"
---

::: block
*That might be the right answer* {style=background:red;width:500px}
::: 

---

### What are we going to talk about?

- No longer just using deterministic algorithms for getting a right answer
- Too much data means we might prefer an approximate answer
- Some algorithms are too complicated to implement, and it's easier to get it almost right
- And in the future, with Quantum you'll get an answer and have to check it (and then try again) 

---

### Randomization for the good

- Deterministic hashing can lead to a DOS
- Add a random number, chosen per run, into the hash algorithm
- ANTS memory profiler, made a bug hard to reproduce (until you know about it)

---

### Order of Magnitude Estimators

- [Morris](https://en.wikipedia.org/wiki/Approximate_counting_algorithm)
- Don't count every time, but instead count to the nearest power of two
- Suppose the count is 4 = 2^2
- When we increment, throw two coins and only increment if we get two Heads

---

### [Skip Lists](https://en.wikipedia.org/wiki/Skip_list)

- 

---

### [Count-min sketch](https://en.wikipedia.org/wiki/Count%E2%80%93min_sketch)

---

### [Bloom filter](https://en.wikipedia.org/wiki/Bloom_filter)


---

