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

 <!-- .slide: data-background="#ff0000" -->

This is the start of some slides for Redgate's Level Up conference in June

---

### Where are we going?

- What does NoSQL mean?
- Why not always relational?
- The challenges - RUM/CAP
- The famous four (+1)
- And the others
- And one database to rule them all 

---

### What does NoSQL mean?

- referring to "not the language" but its model
- relational tables
- foreign keys
- serialization modes (locks)

---

### What does NoSQL mean?

- "NOT ONLY SQL"

---

### What makes up a SQL database?

- base this on Sql Server
- a transaction log (WAL)
- a set of data pages 
- indexing structures (likely a BTree)
- with other secondary indexes
- lock manager

---

### See this book

![](images/prosqlserverinternals.jpg)

---

### [8KB pages](https://learning.oreilly.com/library/view/pro-sql-server/9781484219645/A313962_2_En_1_Chapter.html#Fig6)

![](images/page.png)

---

### [one special index that controls how the data is laid out](https://learning.oreilly.com/library/view/pro-sql-server/9781484219645/A313962_2_En_2_Chapter.html#Fig12)

![](images/indexseek.png)

---

### Extended over time

- column stores
- CTEs to allow you to simulate graphs
- simulate because you have to formulate some of the harder questions into code
- Contrast with PostgreSQL and its many extensions

---

### Relational leads to impedance mismatch

- not using foreign keys
- prefer optimistic locks over pessimistic locking

- for data warehouses, typically no implementation of foreign keys and focus on column store
- for data warehouses, store in parquet (or other encoding) on the disk and bring to life later

---

### It's all about the amplification

- when we do a read, do we just get the data we want
- when we make a small update, how many data pages are touched
- how much memory do we need to use to keep it running efficiently

---

### [The RUM conjecture](https://stratos.seas.harvard.edu/files/stratos/files/rum.pdf)

![](images/rum.png)

---

### It's all about the clustering

- data size grows, and we can only vertically scale so much
- read repliacs, but typically one master node

- then you have to shard
- this requires work from the application
- horizontally - entities into different machines
- vertically - a slice through the data

---

### Sharding has it's own issues

- Facebook TAO
- RAMP - https://people.eecs.berkeley.edu/~alig/papers/ramp.pdf
- RAMP TAO - https://www.vldb.org/pvldb/vol14/p3014-cheng.pdf
- implemented as a clien tlibrary, it's nicer if it is automatic

---

### The CAP theorem

https://en.wikipedia.org/wiki/CAP_theorem

---

### Relational clustering tends to be master-slave

- writes to a master and then push out to secondaries (from HA failover)

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

### Redis example

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

### Some quick points

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

### MongoDB example

```
show databases
db.user.insertOne({name: "Ada Lovelace", age: 205})
show databases
db.user.find({ age: { "$gt": 200 }})

```

---

### Indexing and sharding

- you can define indexes
- you can get the system to shard large collections
- you can do transactions

---

### Cassandra

- written by Facebook to handle write heavy
- SSTables and LSTM
- handle the write and do the work later

- highly configurable for acks on write

---

### That sounds a lot like MongoDB

- compare with Mongo https://www.mongodb.com/compare/cassandra-vs-mongodb

---

### Example

```
docker pull cassandra:latest
docker network create cassandra
docker run --rm -d --name cassandra --hostname cassandra --network cassandra cassandra
docker run --rm -it --network cassandra nuvo/docker-cqlsh cqlsh cassandra 9042 --cqlversion='3.4.6'
```

<pre>
CREATE KEYSPACE IF NOT EXISTS store WITH REPLICATION = { 'class' : 'SimpleStrategy', 'replication_factor' : '1' };
CREATE TABLE IF NOT EXISTS store.shopping_cart (userid text PRIMARY KEY, item_count int, last_update_timestamp timestamp);
INSERT INTO store.shopping_cart(userid, item_count, last_update_timestamp) VALUES ('9876', 2, toTimeStamp(now()));
INSERT INTO store.shopping_cart(userid, item_count, last_update_timestamp) VALUES ('1234', 5, toTimeStamp(now()));
SELECT * FROM store.shopping_cart;
</pre>

---

### Graph databases

- Neo4j
- lots of things are better represented as a graph

---

### Example

```
docker run --name testneo4j --env NEO4J_AUTH=neo4j/password neo4j:latest
docker exec -it testneo4j bash
cypher-shell -u neo4j -p password
```

---

### (Define and query a graph)[https://neo4j.com/docs/cypher-manual/current/clauses/match/]

```
CREATE
  (charlie:Person {name: 'Charlie Sheen'}),
  (martin:Person {name: 'Martin Sheen'}),
  (michael:Person {name: 'Michael Douglas'}),
  (oliver:Person {name: 'Oliver Stone'}),
  (rob:Person {name: 'Rob Reiner'}),
  (wallStreet:Movie {title: 'Wall Street'}),
  (charlie)-[:ACTED_IN {role: 'Bud Fox'}]->(wallStreet),
  (martin)-[:ACTED_IN {role: 'Carl Fox'}]->(wallStreet),
  (michael)-[:ACTED_IN {role: 'Gordon Gekko'}]->(wallStreet),
  (oliver)-[:DIRECTED]->(wallStreet),
  (thePresident:Movie {title: 'The American President'}),
  (martin)-[:ACTED_IN {role: 'A.J. MacInerney'}]->(thePresident),
  (michael)-[:ACTED_IN {role: 'President Andrew Shepherd'}]->(thePresident),
  (rob)-[:DIRECTED]->(thePresident),
  (martin)-[:FATHER_OF]->(charlie)
```

```
MATCH (director {name: 'Oliver Stone'})--(movie)
             RETURN director.name, movie.title;
```

---

### Time series Databases

TimescaleDB

https://docs.timescale.com/getting-started/latest/

- Timescale extends PostgreSQL for time-series and analytics, so you can build faster, scale further, and stay under budget.
- Querying https://docs.timescale.com/getting-started/latest/query-data/
- Compression and automatic compression - https://docs.timescale.com/getting-started/latest/compress-data/

---

### But you don't have to use a different storage engine

- Azure Cosmos DB supports multipe models by converting to Document DB format

---

### Azure Cosmos DB

- multi-model
- native Document DB
- Azure Table
- Mongo DB
- Gremlin graph - https://docs.janusgraph.org/getting-started/gremlin/ https://kelvinlawrence.net/book/Gremlin-Graph-Guide.html#gremlinandsql
---

### And there are more controls

- https://rajneeshprakash.medium.com/cosmos-db-under-the-hood-2d4ce920bb7e
- automatic indexing
- choice of consistency levels - see the beautiful https://learn.microsoft.com/en-us/azure/cosmos-db/consistency-levels

