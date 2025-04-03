
// https://refactoring.guru/design-patterns/bridge

//      An abstraction and its implementation should be defined and extended independently from each other.
//      A compile-time binding between an abstraction and its implementation should be avoided so that an implementation can be selected at run-time.

// Pimpl idiom in C++
//    https://en.wikipedia.org/wiki/Bridge_pattern#:~:text=The%20bridge%20pattern%20can%20also,in%20the%20C%2B%2B%20world.

// Emphasis on object composition instead of inheritance
//    Can avoid a lot of subclasses to fix all the flavours, and the delegation target can be chosen at runtime
//      See coloured shape in the refactoring guru piece

var ab = new AbstractBridge(new Bridge1());

ab.CallMethod1();

// Maybe you'd just program against the interface these days anyway? (And use depenency injection to select the right implementation)

public interface IBridge
{
    void Function1();
    void Function2();
}

public class Bridge1 : IBridge
{
    public void Function1()
    {
        Console.WriteLine("Bridge1.Function1");
    }

    public void Function2()
    {
        Console.WriteLine("Bridge1.Function2");
    }
}

public class Bridge2 : IBridge
{
    public void Function1()
    {
        Console.WriteLine("Bridge2.Function1");
    }

    public void Function2()
    {
        Console.WriteLine("Bridge2.Function2");
    }
}

public interface IAbstractBridge
{
    void CallMethod1();
    void CallMethod2();
}

public class AbstractBridge : IAbstractBridge
{
    public IBridge bridge;

    public AbstractBridge(IBridge bridge)
    {
        this.bridge = bridge;
    }

    public void CallMethod1()
    {
        this.bridge.Function1();
    }

    public void CallMethod2()
    {
        this.bridge.Function2();
    }
}