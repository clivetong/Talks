// https://refactoring.guru/design-patterns/visitor

// Separates the object representatino and the algorithms that work on them

// Don't need to keep changign the classes for each new piece of functionality
//  Export XML
//  Export to Pdf

// Double dispatching - use virtual methods to discriminate to find the right method, instead of picking the overload statically

var things = new List<Thing>();
things.Add(new BigThing());
things.Add(new SmallThing());

var processor = new Processor();

foreach(var thing in things)
{
    if (thing is BigThing x)
    {
        processor.Process(x);
    }
    else if (thing is SmallThing y)
    {
        processor.Process(y);
    }
    else
    {
        throw new Exception("Failed to match");
    }
}

//foreach (var thing in things)
//{
//    thing.Accept(processor);
//}

// And just for the record, you can use dynamic in C# to do dynamic dispatch (ie method overload resolution on the runtime type)
//foreach (dynamic thing in things)
//{
//    processor.Process(thing);
//}


abstract class Thing
{
    //public abstract void Accept(Processor processor);
}

class BigThing : Thing
{
    //public override void Accept(Processor process)
    //{
    //    process.Process(this);
    //}
}

class SmallThing : Thing
{
    //public override void Accept(Processor process)
    //{
    //    process.Process(this);
    //}
}


class Processor
{
    public void Process(BigThing x) {}
    public void Process(SmallThing x) { }
}



// See the Expression Problem: https://en.wikipedia.org/wiki/Expression_problem
// and The Expression Problem and the Visitor Pattern: https://prog2.de/book/sec-java-expr-problem.html

