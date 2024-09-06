---
transition: "slide"
slideNumber: false
title: "Higher Kinded C#"
---

::: block
*Higher Kinds* {style=background:red;width:500px}
:::

---

### What's the talk about?

Category theory is a beautiful set of abstractions that cover lots of areas of maths and computer science.
Haskell's types were heavily influenced by these ideas.
How far can we go with the ideas in C# (*)

Based on [some posts by Paul Louth](https://paullouth.com/higher-kinds-in-c-with-language-ext/) who re-implemented [his language-ext repo](https://github.com/louthy/language-ext) to use `static abstract` in interfaces.

(*) there will be no mention of the m-word

---

### But really

There's a cool encoding of higher kinds into C# (with low overhead, and with a syntax that isn't too bad).

---

### Quick disclaimer

Haskell is a great language, but I think they overplay the "implements mathematical theories". To me, Haskell has based it's libraries in some of the types of category theory, but more as suitable abstract datatypes.

[The romance of Haskell and Category Theory](https://www.reddit.com/r/haskell/comments/qqs2ur/the_romance_of_haskell_and_category_theory/)

[Is Hask even a category?](https://stackoverflow.com/questions/48485660/is-hask-even-a-category) and [Hask is not category](https://math.andrej.com/2016/08/06/hask-is-not-a-category/)
