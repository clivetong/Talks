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
    actor.Tell(new Initialize(Count: 100, Actors: actors));
}

for (int i = 0; i < actors.Count * 5; i++)
{
    actors[i % actors.Count].Tell(new Distribute(Gossip:10));
    #region Take a snapshot
#if WITHSNAPSHOTS
    if (i == actors.Count * 3)
    {
        actors[i % actors.Count].Tell(new Marker());
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

Console.WriteLine($"Final Total: {Node.FinalTotal} Snapshot Total: {Node.SnapshotTotal}");

Console.ReadLine();

record class Distribute(int Gossip) { }

record class Initialize(int Count, List<IActorRef> Actors) { }

record class Increment { }

record class PrintState { }

#region Message to snapshot
#if WITHSNAPSHOTS
record class Marker { }
#endif
#endregion

class Node : ReceiveActor
{
    private int _count = 0;

    private readonly Random _random = new();
    private List<IActorRef> _actors = new();

    public static long FinalTotal = 0;
    public static long SnapshotTotal = 0;

    public Node()
    {
        Receive<Initialize>(s =>
        {
            _count = s.Count;
            _actors = s.Actors;
        });

        Receive<Increment>(_ =>
        {
            _count++;
            #region Record messages
#if WITHSNAPSHOTS
            if (_recordingMessages.Contains(Sender))
            {
                _accumulatedIncrementMessages++;
            }
#endif
            #endregion
        });

        Receive<PrintState>(_ =>
        {
            Interlocked.Add(ref FinalTotal, _count);
            Console.WriteLine($"State: {_count}");
        });

        Receive<Distribute>(d =>
        {
            for (int i = 0; i < d.Gossip; i++)
            {
                var target = _actors[_random.Next(_actors.Count)];
                if (_count > 0 && target.Path != Self.Path)
                {
                    _count--;
                    target.Tell(new Increment());
                }
            }

        });

        #region Extra Message Types
#if WITHSNAPSHOTS
        Receive<Marker>(_ =>
        {
            if (!_myState.HasValue)
            {
                _myState = _count;

                foreach (var actor in _actors)
                {
                    if (actor.Path != Self.Path)
                    {
                        _recordingMessages.Add(actor);
                        actor.Tell(new Marker());
                    }
                }
            }
            
            _recordingMessages.Remove(Sender);

            if (_recordingMessages.Count == 0)
            {
                Interlocked.Add(ref SnapshotTotal, (_myState ?? 0) + _accumulatedIncrementMessages);
                Console.WriteLine($"State: {_myState} Inflight: {_accumulatedIncrementMessages}");
            }

        });
#endif
        #endregion
    }

    #region State needed to record the snapshot
#if WITHSNAPSHOTS
    private int? _myState = null;
    private readonly HashSet<IActorRef> _recordingMessages = new();

    private int _accumulatedIncrementMessages = 0;
#endif
    #endregion

}
