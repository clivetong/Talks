// But there may be other ways this could manifest

// The synchronous call to the continuation is wrong so change that

#if FALSE
using System.Diagnostics.CodeAnalysis;

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

while (await c.WaitToReadAsync)
{
    if (c.TryRead(out var item))
    {
        Console.WriteLine(item);
    }
}

class MyUnboundedChannel<T>
{
    private readonly Queue<T> _items = [];
    private object SyncObj => _items;
    private readonly Queue<TaskCompletionSource<T>> _readers = [];
    private TaskCompletionSource<bool>? _waitingReaders;
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

                var waitingReaders = _waitingReaders;
                _waitingReaders = null;
                waitingReaders?.SetResult(true);
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

            TaskCompletionSource<T> tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
            _readers.Enqueue(tcs);
            return new ValueTask<T>(tcs.Task);
        }
    }

    public bool TryRead([MaybeNullWhen(false)] out T item) // Generics and Nullability, caller can check
    {
        lock (SyncObj)
        {
            return _items.TryDequeue(out item);
        }
    }

    public ValueTask<bool> WaitToReadAsync
    {
        get
        {
            lock (SyncObj)
            {
                if (_items.Count > 0 || _completed)
                {
                    return new ValueTask<bool>(_items.Count > 0);
                }
                _waitingReaders ??= new(TaskCreationOptions.RunContinuationsAsynchronously);
                return new ValueTask<bool>(_waitingReaders.Task);
            }
        }
    }

    public void Complete()
    {
        lock (SyncObj)
        {
            _completed = true;
            while (_readers.TryDequeue(out var tcs))
            {
                tcs.SetException(new InvalidOperationException("Channel completed"));
            }

            var waitingReaders = _waitingReaders;
            _waitingReaders = null;
            waitingReaders?.SetResult(false);
        }
    }

}
#endif
