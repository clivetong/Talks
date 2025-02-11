// This is good, https://www.youtube.com/watch?v=JEk7B_GUErc, starting at "Simple Factory", moving to Factory and then to Abstract Factory

// See slides at
//   https://youtu.be/JEk7B_GUErc?si=ZNxClz6vAjKOzgA1&t=169
//   https://youtu.be/JEk7B_GUErc?si=iIUMPT2kXY1iIZEr&t=638
//   https://youtu.be/JEk7B_GUErc?si=MSAGDPYsiPghqZaK&t=909
//   https://youtu.be/JEk7B_GUErc?si=bFJ7gGdoxS3HjewX&t=1109  (abstract factory)

// https://refactoring.guru/design-patterns/factory-method

// ... difference with Abstract Factory 
//      https://softwareengineering.stackexchange.com/questions/81838/what-is-the-difference-between-the-factory-pattern-and-abstract-factory

// Step 1 is moving all calls to new - new is glue, to a central place ie give responsibility to something else

// This is the days before DI containers, which might now be a good place


IThing makeThing(string typeToMake) => typeToMake switch
{
    "A" => new TypeA(),
    "B" => new TypeB("Complicated arguments")
};

// stop passing a string placeholder

IThing makeThing2(IFactory factory) => factory.Make();


class TypeBFactory : IFactory
{
    public IThing Make() => new TypeB("Complicated arguments");
}

interface IFactory
{
    IThing Make();
}

interface IThing
{
}

class TypeA : IThing
{
}

class TypeB(string complicatedArguments) : IThing
{
}

// The Factory Method pattern
//    Use abstract classes to let the specific factory override the creation, but still return the common interface

abstract class Logistics
{
    public abstract ITransport CreateLogistics();
}

interface ITransport
{

}

class ShipLogistics : Logistics
{
    public override ITransport CreateLogistics() => new Ship();
}

class Ship : ITransport
{
}

class VehicleLogistics : Logistics
{
    public override ITransport CreateLogistics() => new Vehicle();
}

class Vehicle : ITransport
{
}
