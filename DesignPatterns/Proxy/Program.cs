// https://refactoring.guru/design-patterns/proxy

// Substitute for another object, perhaps controlling access (my favourite use),
// or doing something before or after calling the original (perhaps even transferring the call
// across the network)

// Lazy initialization
// Access control [Capability based protection mechanisms]
// Remoting
// Logging
// Caching 
// Reference counting (smart proxy in front of a heavyweight object)

var realHeatingController = new MyHeatingController();
var heatingController = new HeatingControllerProxy(realHeatingController);

heatingController.TurnOn();

interface IHeatingController
{
    void TurnOn();
    void TurnOff();
}

class MyHeatingController : IHeatingController
{
    public void TurnOn()
    {
        Console.WriteLine("Heating on");
    }

    public void TurnOff()
    {
        Console.WriteLine("Heating off");
    }
}

class HeatingControllerProxy(IHeatingController proxy) : IHeatingController
{
    public void TurnOn()
    {
        var user = Environment.GetEnvironmentVariable("USERNAME");
        if (user == "clive.tong")
        {
            proxy.TurnOn();
        }
        else
        {
            throw new Exception("Only clive can turn the heating on!!!");
        }
    }

    public void TurnOff() => proxy.TurnOff();
}