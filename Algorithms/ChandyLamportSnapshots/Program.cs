#define WITHSNAPSHOTS
using Akka.Actor;

ActorSystem system = ActorSystem.Create("MySystem");

List<IActorRef> actors = new();
for (int i = 0; i < 10; i++)
{
    IActorRef nodeActor = system.ActorOf<Node>($"Actor-{i}");
    actors.Add(nodeActor);
}

foreach (var actor in actors)
{
    actor.Tell(new Initialize(Count: 100, Actors: actors)); ;

}

for (int i = 0; i < actors.Count * 5; i++)
{
    actors[i % actors.Count].Tell(new Distribute(Gossip:5));
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

record class Distribute(int Gossip) { }

record class Initialize(int Count, List<IActorRef> Actors) { }

class Increment {}

class PrintState { }

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

class Node : ReceiveActor
{
    readonly Random _random = new();
    int _state = 0;
    List<IActorRef> _actors = new();

    public Node()
    {
        Receive<Initialize>(s =>
        {
            _state = s.Count;
            _actors = s.Actors;
        });

        Receive<Increment>(_ =>
        {
            _state++;
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
            Console.WriteLine(_state);
        });

        Receive<Distribute>(d =>
        {
            for (int i = 0; i < d.Gossip; i++)
            {
                var target = _actors[_random.Next(_actors.Count)];
                if (_state > 0 && target != Self)
                {
                    _state--;
                    target.Tell(new Increment());
                }
            }

        });

        #region Extra Message Types
#if WITHSNAPSHOTS
        Receive<Snapshot>(_ =>
        {
            myState = _state;
            foreach(var actor in _actors)
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
                myState = _state;

                foreach (var actor in _actors)
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
