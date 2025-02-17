// https://refactoring.guru/design-patterns/state

// It appears as if the object changes its class

// Related to a FSM. The outer object delegates through to the current context state

// See applicability cases on the link above

class Gun : IAction
{
    private State _context = new Unloaded();

    public void PullTrigger()
    {
        _context.PullTrigger();
    }

    public void LoadGun()
    {
        _context = new Loaded();
    }
}
interface IAction
{
    void PullTrigger();
}

abstract class State
{
    public abstract void PullTrigger();
}

class Unloaded : State
{
    public override void PullTrigger()
    {
        Console.WriteLine("Click!!");
    }
}

class Loaded : State
{
    public override void PullTrigger()
    {
        Console.WriteLine("Bang!!");
    }
}
