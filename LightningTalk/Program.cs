using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 1998

namespace Lightning_Talk
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(delegate { TheTalk(); });
            Console.ReadLine();
        }

        static async void TheTalk()
        {
            // https://en.wikipedia.org/wiki/Give_Me_Convenience_or_Give_Me_Death

            Debugger.Break();

            // I think that sometimes we try to make things too simple. 
            // It works well for the common case, but makes the edge cases way too confusing.

            Debugger.Break();

            // I think async is exactly matches this. It's good for the common cases
            // but the edge cases just make you doubt everything.

            Debugger.Break();

            // It some languages we could implement async as a language extension, say by using macros.

            // In C# we have ended up with 
            //    Extending an existing library - the TPL
            //    Adding a pattern for the await mechanism to the front end
            //    Having methods marked with async code generate to a mass of code implementing a state machine
            //    Add hook points into the TPL

            // And all the generated code is straightforward C#

            Debugger.Break();

            // How did we get here?

            // We started with a notion of task, which you'd probably call a promise in other languages
            //  ... and we'd probably make it an interface or abstract class

            // To be honest, Task in .NET is way overpopulated with convenience methods
            // so it is easy to think this is the nature of a Task.

            var task0 = Task.Run<int>(() =>
            {
                Debugger.Break();
                return 10;
            });

            task0.Wait();

            var x = task0.Result;

            Debugger.Break();

            // And inspect to see the promise items

            var status = task0.Status;

            Debugger.Break();

            // But this is really its essence

            var task1 = Task.FromResult<int>(10);

            var task2 = Task.FromException<int>(new NotImplementedException("nooooooo"));

            Debugger.Break();

            // And its nature as a box comes via TaskCompletionSource

            var tcs1 = new TaskCompletionSource<int>();
            var task3 = tcs1.Task;

            Debugger.Break();

            ThreadPool.QueueUserWorkItem(delegate { tcs1.SetResult(10); });

            task3.Wait();

            Debugger.Break();

            // But Task feels like it has too much inside one class

            var task4 = new Task<int>(() =>
            {
                Debugger.Break(); // Where is this going to run?
                return 10;
            });

            Debugger.Break();

            task4.Start(); 
            task4.Wait();

            Debugger.Break();

            // And of course there's a way to handle exceptions too

            var faultingTask = Task.Run(() => { throw new NotImplementedException("If only I'd bothered"); });

            Thread.Sleep(TimeSpan.FromSeconds(1));

            Debug.WriteLine(faultingTask.Status);
            Debug.WriteLine(faultingTask.Exception);

            Debugger.Break();

            // And waiting for a faulting task does the throwing

            try
            {
                faultingTask.Wait();
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

            // And you might regard async as a DSL for dealing with tasks

            var faultingTask2 = PercyThrower();

            var x2 = faultingTask2.Result;
            var x3 = await faultingTask2;

            Debugger.Break();

            // Or more consisely

            Func<Task> taskMaker =
                async () =>
                {
                    throw new NotImplementedException("If only I'd bothered");
                };

            // Though using the async syntax is not quite like the normal Task.Run
            // Task.Run generally makes things run on the threadpool, whereas
            // async knows about the local context and tries to make sure that code
            // only runs in the context

            Debugger.Break();

            Func<Task> l = async () =>
            {
                Debugger.Break();
                await Task.Yield();
                Debugger.Break();
                await Task.Yield();
            };

            await l();
            Debugger.Break();

            // But back to the example

            var faultingTask3 = taskMaker();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            Debugger.Break();

            var exceptionType = faultingTask3.Exception.GetType().Name;

            Debugger.Break();

            // But there's mystery happening

            try
            {
                await faultingTask3;
            }
            catch (Exception ex)
            {
                // And we see something different in the catch
                //  ... by design the await takes the first exception of the aggregate so that the above works ok

                Debugger.Break();
            }

            // leading to the classic gotcha

            var waitingForYouAll = Task.WhenAll(taskMaker(), taskMaker());

            Debug.WriteLine(waitingForYouAll.Status);
            Debug.WriteLine(waitingForYouAll.Exception);

            try
            {
                await waitingForYouAll;
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

            // They took the Task library, and added support for the front end code generation

            Func<Task> awaitMaker = async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
            };

            // await awaitMaker()
            //        =>

            var theTask = awaitMaker();
            var theAwaiter = theTask.GetAwaiter();
            theAwaiter.OnCompleted(() => Debugger.Break());

            Thread.Sleep(TimeSpan.FromSeconds(10));

            Debugger.Break();

            // We have chosen to generate C# code to implement the async methods and the awaiter calls,
            // and that leads to the edge case.

            // In C# 6, we can now put an await in a finally block

            Func<Task> awaitingFinally = async () =>
            {
                try
                {
                    Debugger.Break();
                    throw new NotImplementedException();
                }
                finally
                {
                    Debugger.Break();
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    Debugger.Break();
                }
            };

            try
            {
                Debugger.Break();
                await awaitingFinally();
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

            // But it's rather hard to implement that finally as a real finally 

            // Finally in C# maps to finally blocks in IL which are a thread related concept.
            // An await related finally requires thread stack unwinding and re-establishment

            Debugger.Break();

            // Now there's a very special exception in .NET

            try
            {
                Debugger.Break();
                Thread.CurrentThread.Abort();
            }
            catch (Exception ex)
            {
                Debugger.Break();
                Thread.ResetAbort();
            }

            Debugger.Break();

            // And this is fine if we don't do any awaiting.

            Func<Task> awaitingFinally2 = async () =>
            {
                await Task.Yield();
                try
                {
                    Debugger.Break();
                    Thread.CurrentThread.Abort();
                }
                finally
                {
                    Debugger.Break();
                }
            };

            var task = awaitingFinally2();

            Thread.Sleep(TimeSpan.FromSeconds(5));

            Debugger.Break();

            // But....

            Func<Task> awaitingFinally3 = async () =>
            {
                await Task.Yield();
                try
                {
                    Debugger.Break();
                    Thread.CurrentThread.Abort();
                }
                finally
                {
                    Debugger.Break();
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            };

            var withAwaitInFinally = awaitingFinally3();

            Thread.Sleep(TimeSpan.FromSeconds(7));

            // And that's it. It's the first time that try...finally doesn't map to try...finally.
            // It maps to
            //   try { ... } catch(Exception ex) { theException = ex; }
            //   ... do stuff with the exception...
            //   ... and then re-raise it 

            Debugger.Break();
        }


        // Marking a method as async makes it into a task 
        static async Task<int> PercyThrower()
        {
            Debugger.Break();
            return 10;
        }

    }
}
