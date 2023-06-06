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

https://stratos.seas.harvard.edu/files/stratos/files/rum.pdf

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
- RAMP - https://people.eecs.berkeley.edu/~alig/papers/ramp.pdf
- RAMP TAO - https://www.vldb.org/pvldb/vol14/p3014-cheng.pdf
- it's nicer if it is automatic

---

### The CAP theorem

https://en.wikipedia.org/wiki/CAP_theorem

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

- good for write heavy workloads
- offer locking of keys
- offer auto-deleteion after time

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

```
show databases
db.user.insertOne({name: "Ada Lovelace", age: 205})
show databases
db.user.find({ age: { "$gt": 200 }})

```

### Indexing and sharding

- you can define indexes
- you can get the system to shard large collections
- you can do transactions

---

### Cassandra

- written by Facebook to handle write heavy
- SSTables and LSTM
- handle the write and do the work later

- compare with Mongo https://www.mongodb.com/compare/cassandra-vs-mongodb

- highly configurable for acks on write

---

### Example

```
docker pull cassandra:latest
docker network create cassandra
docker run --rm -d --name cassandra --hostname cassandra --network cassandra cassandra
docker run --rm -it --network cassandra nuvo/docker-cqlsh cqlsh cassandra 9042 --cqlversion='3.4.6'
```

```
CREATE KEYSPACE IF NOT EXISTS store WITH REPLICATION = { 'class' : 'SimpleStrategy', 'replication_factor' : '1' };
CREATE TABLE IF NOT EXISTS store.shopping_cart (userid text PRIMARY KEY, item_count int, last_update_timestamp timestamp);
INSERT INTO store.shopping_cart(userid, item_count, last_update_timestamp) VALUES ('9876', 2, toTimeStamp(now()));
INSERT INTO store.shopping_cart(userid, item_count, last_update_timestamp) VALUES ('1234', 5, toTimeStamp(now()));
SELECT * FROM store.shopping_cart;
```

---

### Graph databases

- Neo4j

---

### Example

```
docker run --name testneo4j --env NEO4J_AUTH=neo4j/password neo4j:latest
docker exec -it testneo4j bash
cypher-shell -u neo4j -p password
match (n) return count(n);

```

---


### Azure Cosmos DB

- multi-model
- native Document DB
- Azure Table
- Mongo DB
- Gremlin graph

- automatic indexing

