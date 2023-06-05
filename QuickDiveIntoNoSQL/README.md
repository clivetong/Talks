---
transition: "slide"
slideNumber: false
title: "A quick dive into NoSQL"
---

::: block
*A quick dive into NoSQL* {style=background:red;width:500px}
:::

---

### UNDER CONSTRUCTION

This is the start of some slides for Redgate's Level Up conference in June

---

### What does NoSQL mean?

- referring to Sequel the implementation
- relational tables
- foreign keys
- serialization modes (locks)

- no the language, as we'll see variants of that used

---

### What does NoSQL mean?

- not only Sql
- where Sql means a relational database

---

### What makes up a SQL database?

- base this on Sql Server
- a set of data pages and a transaction log
- indexing structures (likely a BTree)
- with other secondary indexes

---

### Extended over time

- column stores
- CTEs to allow you to simulate graphs
- simulate because you have to formulate some of the harder questions into code

---

### Relational leads to impedance mismatch

- not using foreign keys
- prefer optimistic locks over pessimistic locking

- for data warehouses, typically no implementation of foreign keys and fcus on column store

---

### It's all about the amplification

- when we do a read, do we just get the data we want
- when we make a small change, how many data pages are touched
- how much memory do we need to use to keep it running efficiently

---

### the RUM conjecture

---

### It's all about the clustering

- data size grows, and we can only vertically scale so much
- read repliacs, but typcially one master node

- then you have to shard
- this requires work from the application
- horizontally - entities into different machines
- vertically - a slice through the data


