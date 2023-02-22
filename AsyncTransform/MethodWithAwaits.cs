using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using NUnit.Framework;

namespace AsyncTransform;

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

    // Remind you of enumeration?

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
    public void CheckTheContinuations()
    {
        var res = DoSteps(10);

        var enumerator = res.GetEnumerator();
        
        enumerator.MoveNext();
        enumerator.MoveNext();
        enumerator.MoveNext();

        Assert.IsFalse(enumerator.MoveNext());
    }

    // So how does the compiler transform that?

    class MethodReplacement
    {
        private int _argument;
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

        // The real stuff
        private int _state = 0;

        private int _i;

        // The real one doesn't have a final result
        private int _result;

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
                    _result = _i;
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

        // The real stuff
        private int _state = 0;

        private int _i;

        // The real one doesn't have a final result
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
                        awaiter.OnCompleted(() => MoveNext());
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
                        awaiter.UnsafeOnCompleted(() => MoveNext());
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
        var result = await method.Result;

        Assert.That(result, Is.EqualTo(12));
    }

    // Discuss
    //   Synchronous completion
    //   UnsafeContinuation
    //   The fast path and allocations

}
