using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace AsyncTransform;


// Why the details about the history?
//   To explain why things are named this way
//   To show it isn't just magic
//   And we can explain why the await points changed over time

// Why the lowering and not add support to the CLR?

[TestFixture]
public class MethodWithAwaits
{
    public async Task<int> TransformThis(int argument)
    {
        var i = argument;

        await Task.Delay(TimeSpan.FromSeconds(1));

        i++;

        Console.WriteLine(i);

        await Task.Delay(TimeSpan.FromSeconds(1));

        i++;

        Console.WriteLine(i);

        return i;
    }

    [Test]
    public async Task CheckAsync()
    {
        var result = await TransformThis(10);
        Assert.That(result, Is.EqualTo(12));
    }

    // Remind you of enumeration? (Why do I talk about that?)
    //    C# 2.0
    //    CCR 
    
    class Unit
    {
        public static Unit Instance { get; } = new Unit();
    }

    IEnumerable<Unit> DoSteps(int argument)
    {
        var i = argument;

        yield return Unit.Instance;

        i++;

        Console.WriteLine(i);

        yield return Unit.Instance;

        i++;

        Console.WriteLine(i);

        // return i;
    }

    [Test]
    public void CheckTheContinuations0()
    {
        var res = DoSteps(10);
        var result = res.ToList();

        Assert.That(result.Count, Is.EqualTo(2));
    }

    // Under the covers - let's drive it by hand

    [Test]
    public void CheckTheContinuations()
    {
        var res = DoSteps(10);

        var enumerator = res.GetEnumerator();

        Assert.IsTrue(enumerator.MoveNext());
        Assert.IsTrue(enumerator.MoveNext());
        Assert.IsTrue(enumerator.MoveNext());

        Assert.IsFalse(enumerator.MoveNext());
    }

    // So how does the compiler transform that?
    //    There's a protocol - GetEnumerator
    //    There's a display class (and explain the naming)
    //    And there's a state machine

    class MethodReplacement
    {
        private readonly int _argument;
        public MethodReplacement(int argument)
        {
            _argument = argument;
        }

        public IEnumerator<Unit> GetEnumerator()
        {
            return new MethodEnumerator(_argument);
        }
    }

    class MethodEnumerator : IEnumerator<Unit>
    {
        private readonly int _argument;
        public MethodEnumerator(int argument)
        {
            _argument = argument;
        }

        // The continuation points
        private int _state = 0;

        // The local variables are re-homed
        private int _i;

        // The real one doesn't have a final result
        //private int _result;

        public bool MoveNext()
        {
            switch (_state)
            {
                case 0:
                    _i = _argument;
                    _state = 1;
                    return true;
                case 1:
                    _i++;
                    Console.WriteLine(_i);
                    _state = 2;
                    return true;
                case 2:
                    _i++;
                    Console.WriteLine(_i);
                    _state = 3;
                    return true;
                default:
                    //_result = _i;
                    return false;
            }
        }

        public Unit Current => throw new NotImplementedException();

        object IEnumerator.Current => throw new NotImplementedException();

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    [Test]
    public void CheckTheContinuations2()
    {
        var res =new MethodReplacement(10);

        var enumerator = res.GetEnumerator();

        enumerator.MoveNext();
        enumerator.MoveNext();
        enumerator.MoveNext();

        Assert.IsFalse(enumerator.MoveNext());
    }

    // And let's go to async

    class AsyncMethodEnumerator
    {
        private readonly int _argument;

        public AsyncMethodEnumerator(int argument)
        {
            _argument = argument;
        }

        // The continuation points
        private int _state = 0;

        // The local variables are re-homed
        private int _i;

        // And we need some plumbing for the result
        private TaskCompletionSource<int> _result = new TaskCompletionSource<int>();
        public Task<int> Result => _result.Task;

        public void MoveNext()
        {
            Task task;
            TaskAwaiter awaiter;

            switch (_state)
            {
                case 0:
                    _i = _argument;
                    _state = 1;

                    task = Task.Delay(TimeSpan.FromSeconds(1));
                    awaiter = task.GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        awaiter.OnCompleted(MoveNext);
                        return;
                    }

                    goto case 1;

                case 1:
                    _i++;
                    Console.WriteLine(_i);
                    _state = 2;

                    task = Task.Delay(TimeSpan.FromSeconds(1));
                    awaiter = task.GetAwaiter();
                    if (!awaiter.IsCompleted)
                    {
                        awaiter.OnCompleted(MoveNext);
                        return;
                    }

                    goto case 2;

                case 2:
                    _i++;
                    Console.WriteLine(_i);
                    _state = 3;
                    _result.SetResult(_i);
                    return;
            }
        }

    }

    [Test]
    public async Task CheckAsync2()
    {
        var method = new AsyncMethodEnumerator(10);
        method.MoveNext();
        var resultingTask = method.Result;

        var result = await resultingTask;

        Assert.That(result, Is.EqualTo(12));
    }

    // Discuss
    //   When we gave the thread back, and how we worked on a thread on a thread that was given to us
    //   Synchronous completion
    //   UnsafeContinuation
    //   The fast path and allocations
    //   Catching exceptions
    //   await is an expression
    //   await in a catch/finally have to be translated specially

    // What's doing the timer then? And into the murky world of SynchronizationContexts
    //   And why do you need to ConfigureAwait

    class MySynchronizationContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object? state)
        {
            Debugger.Break();
            base.Post(d, state);

            // public virtual void Post(SendOrPostCallback d, object? state) => ThreadPool.QueueUserWorkItem(static s => s.d(s.state), (d, state), preferLocal: false);
        }

    }

    [Test]
    public async Task CheckSynchronizationContext()
    {
        SynchronizationContext.SetSynchronizationContext(new MySynchronizationContext());

        var result = await TransformThis(10);
        Assert.That(result, Is.EqualTo(12));
    }

    // Set a breakpoint on the i++ in the original to see the thread jump

    // But we are jumping between threads so what do we take with us?

    [Test]
    public void WhatsAnExecutionContext()
    {
        var executionContext = ExecutionContext.Capture();
        Debugger.Break();
    }

}
