// Define an abstract syntax for a language that is more domain specific

// https://en.wikipedia.org/wiki/Interpreter_pattern

// Maybe applies to an embedded SQL query

var query = 
    new OrQuery(
    new AndQuery(new BaseTerm("Linkin Park"), new BaseTerm("Emily Armstrong")),
    new BaseTerm("Scream Silence"));

interface DoSearch
{
    string[] Query(QueryExpression query);
}

abstract class QueryExpression;
class AndQuery(QueryExpression left, QueryExpression right) : QueryExpression;

class OrQuery(QueryExpression left, QueryExpression right) : QueryExpression;

class BaseTerm(string term) : QueryExpression;

