#if FALSE
var c = new MyUnboundedChannel<int>();

_ = Task.Run(async () =>
{
    for (int i = 0; i < 10; i++)
    {
        await c.WriteAsync(i);
        await Task.Delay(TimeSpan.FromSeconds(1));
    }

    c.Complete();
});

while (true)
{
    Console.WriteLine(await c.ReadAsync());
}

class MyUnboundedChannel<T>
{
    private readonly Queue<T> _items = [];
    private object SyncObj => _items;
    private readonly Queue<TaskCompletionSource<T>> _readers = [];
    private bool _completed;

    public ValueTask WriteAsync(T item)
    {
        lock (SyncObj)
        {
            if (_completed)
            {
                return ValueTask.FromException(new InvalidOperationException("Channel is completed"));
            }

            if (_readers.TryDequeue(out var tcs))
            {
                tcs.SetResult(item);
            }
            else
            {
                _items.Enqueue(item);
            }
        }

        return default;
    }

    public ValueTask<T> ReadAsync()
    {
        lock (SyncObj)
        {
            if (_items.TryDequeue(out var item))
            {
                return new ValueTask<T>(item);
            }

            if (_completed)
            {
                return ValueTask.FromException<T>(new InvalidOperationException("Channel is completed"));
            }

            TaskCompletionSource<T> tcs = new();
            _readers.Enqueue(tcs);
            return new ValueTask<T>(tcs.Task);
        }
    }

    public void Complete()
    {
        lock (SyncObj)
        {
            _completed = true;
            while(_readers.TryDequeue(out var tcs))
            {
                tcs.SetException(new InvalidOperationException("Channel completed"));
            }
        }
    }

}
#endif
