#define WITHSNAPSHOTS
using Akka.Actor;

ActorSystem system = ActorSystem.Create("MySystem");

List<IActorRef> actors = new();
for (int i = 0; i < 10; i++)
{
    IActorRef fooActor = system.ActorOf<Foo>($"Actor-{i}");
    actors.Add(fooActor);
}

foreach (var actor in actors)
{
    actor.Tell(new SetState { Actors = actors }); ;

}

for (int i = 0; i < actors.Count * 5; i++)
{
    actors[i % actors.Count].Tell(new Distribute());
    #region Take a snapshot
#if WITHSNAPSHOTS
    if (i == actors.Count)
    {
        actors[i % actors.Count].Tell(new Snapshot());
    }
#endif
    #endregion
}

Console.ReadLine();

foreach (var actor in actors)
{
    actor.Tell(new PrintState());
}

Console.ReadLine();

class Distribute
{
    public int Gossip { get; } = 2;
}

class SetState
{
    public int Count { get; init; } = 100;
    public List<IActorRef> Actors { get; init; } = new();
}

class Increment
{

}

class PrintState
{

}

#region Message to snapshot
#if WITHSNAPSHOTS
class Snapshot
{

}

class Marker
{

}
#endif
#endregion

class Foo : ReceiveActor
{
    Random random = new();
    int state = 0;
    List<IActorRef> actors = new();
    public Foo()
    {
        Receive<SetState>(s =>
        {
            state = s.Count;
            actors = s.Actors;
        });

        Receive<Increment>(_ =>
        {
            state++;
            #region Record messages
#if WITHSNAPSHOTS
            if (recordingMessages.Contains(Sender))
            {
                accumulatedIncrementMessages++;
            }
#endif
            #endregion
        });

        Receive<PrintState>(_ =>
        {
            Console.WriteLine(state);
        });

        Receive<Distribute>(_ =>
        {
            for (int i = 0; i < 5; i++)
            {
                var target = actors[random.Next(actors.Count)];
                if (state > 0 && target != Self)
                {
                    state--;
                    target.Tell(new Increment());
                }
            }

        });

        #region Extra Message Types
#if WITHSNAPSHOTS
        Receive<Snapshot>(_ =>
        {
            myState = state;
            foreach(var actor in actors)
            {
                if(actor != Self)
                {
                    recordingMessages.Add(actor);
                    actor.Tell(new Marker());
                }
            }

        });

        Receive<Marker>(_ =>
        {
            if (!myState.HasValue)
            {
                myState = state;

                foreach (var actor in actors)
                {
                    if (actor != Self)
                    {
                        recordingMessages.Add(actor);
                        actor.Tell(new Marker());
                    }
                }
            }
            
            recordingMessages.Remove(Sender);

            if (recordingMessages.Count == 0)
            {
                Console.WriteLine($"State: {myState} Inflight: {accumulatedIncrementMessages}");
            }

        });
#endif
        #endregion
    }

    #region State needed to record the snapshot
#if WITHSNAPSHOTS
    HashSet<IActorRef> recordingMessages = new();
    int accumulatedIncrementMessages = 0;
    int? myState = null;
#endif
    #endregion

}
