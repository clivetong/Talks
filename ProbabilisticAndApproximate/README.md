---
transition: "slide"
slideNumber: false
title: "That might be the right answer"
---

::: block
*That might be the right answer* {style=background:red;width:500px}
::: 

---

### What are we going to talk about?

- Using randomness to make choices in algorithms
- No longer just using deterministic algorithms for getting a right answer

---

### Why would you do that?

- Save resources for order of magnitude estimates (Morris)
- Some algorithms are too complicated to implement, and it's easier to get it right most of the time (Skip Lists)
- Too much data means we might prefer an approximate answer (Count-min sketch, Bloom filters)
- And in the future, with Quantum you'll get an answer and have to check it (and then try again) 

---

### Randomization for the good

- Deterministic hashing can lead to a DOS
- Add a random number, chosen per run, into the hash algorithm
- ANTS memory profiler, made a bug hard to reproduce (until you knew about it)

---

### Order of Magnitude Estimators

- [Morris](https://en.wikipedia.org/wiki/Approximate_counting_algorithm)
- Don't count every time, but instead count to the nearest power of two
- Suppose the count is 4 = 2^2
- When we increment, throw two coins and only increment if we get two Heads

---

### See C# implementation

---

### [Skip Lists](https://en.wikipedia.org/wiki/Skip_list)

- Remember those 2-3 trees or red-black or AVL trees from University
- Really hard/fiddly to implement the rotate operation
- Maybe you can get it right most of the time
- Parallel friendly

---

![Skip list](images/skiplist.png)

---

![](images/skiplist-complexity.png)

![](images/2-3-complexity.png)

---

### And now for something different

- Two algorithms that can't be certain the answer is right

- Based on multiple hash functions to find a group of locations
- Then use the locations to get the answer
- Bloom - binary values and do "OR"
- Count-min sketch - integer values and take the min
- It's the combining operation that makes it approximate

---

### [Bloom filter](https://en.wikipedia.org/wiki/Bloom_filter)

![](images/bloom.png)

- Possibly in the set
- Definitely not in the set

---

### [Count-min sketch](https://en.wikipedia.org/wiki/Count%E2%80%93min_sketch)

- Use integer values
- Count by incrementing all the locations
- Fetch count by taking the minimum of the locations

---

### And that's it

- Sometimes the right answer isn't 100% correct
- Sometimes that means something is easier to implement or is more parallel friendly

- But doing the maths to check its correctness can be realy hard
