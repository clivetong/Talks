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

### Where are we going?

- What does NoSQL mean?
- Why not always Sql Server?

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

---

### Sharding has it's own issues

- Facebook TAO
- RAMP

---

### The CAP theorem

---

### The alternatives

- file system (Map-Reduce)
- KV - Redis/Memcached/etcd (etc distributed)
- document - MongoDB
- column-family stores - Cassandra
- graph databases - Neo4J

---

### And also

- NewSQL (Spanner)
- spatial (PostGIS)
- time series InfluxDB/TimescaleDB

---

### File system

- The original system for processing lots of big data. 
- Map phase/Reduce phase with data shuffling happening in the middle
- Large clusters
- Design influenced by the need to restart jobs

---

### Key Value stores

---

### Redis

``` 
docker run --name my-redis -p 6379:6379 -d redis
docker exec -it my-redis sh
```

---

### Redis datatypes

- strings
- lists
- sets, sorted sets
- hashes
- streams
- bitmaps
- hyperloglog

---

```
set name Monica
get name
del name

rpush name Clive
rpush name Andrew 
rpush name Tong
lrange name 0 10

```

---

- single-threaded
- emphasise in-memory for speed

---

### MongoDB

```
docker run -d --name test-mongo mongo:latest
docker exec -it test-mongo bash
mongosh
```

---


---

### Azure Cosmos DB

- multi-model
