// https://refactoring.guru/design-patterns/template-method

// You've got some steps in a some algorithm
// Let subclasses implement those steps as they need to

var humanWalker = new HumanWalking();
humanWalker.Motion();

public abstract class StartWalking
{
    public void Motion()
    {
        Prepare();
        StartRhythmForLeftLeg();
        StartRhythmForRightLeg();
        StepLeftLeg();
        StepRightLeg();
    }

    protected virtual void Prepare() {}
    protected abstract void StartRhythmForLeftLeg();

    protected abstract void StartRhythmForRightLeg();

    protected virtual void StepLeftLeg() 
    { 
        // Step left
    }

    protected virtual void StepRightLeg()
    {
        // Step right
    }

}

public class HumanWalking : StartWalking
{
    protected override void Prepare()
    {
        // Lean forwards
    }

    protected override void StartRhythmForLeftLeg()
    {
    }

    protected override void StartRhythmForRightLeg()
    {
    }
}