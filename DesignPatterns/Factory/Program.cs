// https://refactoring.guru/design-patterns/factory-method

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
