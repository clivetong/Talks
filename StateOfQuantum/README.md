---
transition: "slide"
slideNumber: false
title: "What! Quantum Computers Can't Do That?"
---

::: block
*What! Quantum Computers Can't Do That?* {style=background:red;width:500px}
::: 

---

### Good books

![The book](https://github.com/clivetong/Play/raw/master/StateOfQuantum/images/quantum-computing.jpg)


---

### Have a standard fixed design

![Qubit counts](https://github.com/clivetong/Play/raw/master/StateOfQuantum/images/qubits.jpg)

[From this YouTube video](https://www.youtube.com/watch?v=gcbMKt079l8)

---


### Anything big


![Qubit counts](https://github.com/clivetong/Play/raw/master/StateOfQuantum/images/errors.jpg)

[From this YouTube video](https://www.youtube.com/watch?v=-UlxHPIEVqA)

---

### Don't just use the Qubit count

- IBM have a notion of Quantum Volume that measures many features

---

### Sit on my desktop

- will be somewhere else in a special environment
  - but will need a standard computer to help with the computation


---

### Allow you to write imperative programs

- reversible computations until measurement time
  - so it helps if you've studied linear algebra
  - matrices over complex numbers
  - logical model of a Qubit as a bloch sphere
- the no copy theorem means you can't just
  - ```x = y ```
- use superposition
- use entanglement
- phase kickback


---


### Example program (Real Randomness)

```
namespace Qrng {
    open Microsoft.Quantum.Convert;
    open Microsoft.Quantum.Math;
    open Microsoft.Quantum.Measurement;
    open Microsoft.Quantum.Canon;
    open Microsoft.Quantum.Intrinsic;
    
    operation SampleQuantumRandomNumberGenerator() : Result {
        // Allocate a qubit        
        use q = Qubit();  
        // Put the qubit to superposition
        // It now has a 50% chance of being measured 0 or 1  
        H(q);      
        // Measure the qubit value            
        return M(q); 
    }
}```


---


### Allow you to definitely get the right answer 

- many algorithms are probabilistic
  - amplitude amplification ([Grover's algorithm](https://docs.microsoft.com/en-us/azure/quantum/tutorial-qdk-grovers-search?tabs=tabid-visualstudio))
- programs mix quantum and conventional, so that the quantum part can be run multiple times

---

### Represent more data than a classical computer with that number of bits

- it takes 2^n bits to simulate a Quantum computer with n Qubits 
   (because of superposition)
- [Holevo's theorem](https://en.wikipedia.org/wiki/Holevo%27s_theorem#:~:text=Holevo's%20theorem%20is%20an%20important,quantum%20state%20(accessible%20information))

---



### Out-perform a classical computer on small problems

- likely that quantum computers scale better int he size of the data
