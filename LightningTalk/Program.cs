using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

            // https://en.wikipedia.org/wiki/Give_Me_Convenience_or_Give_Me_Death#/media/File:Dead_Kennedys_-_Give_Me_Convenience_or_Give_Me_Death_cover.jpg

            // New language features could be implemented using a library

            // But unlike other languages C# doesn't have a flexible syntax or a macro language to exploit it (think Internal DSL)

            // So we end up instead with a library, new syntax in the language, and a hardwired code generating front end that sits on top of the library
            //     [though this is extensible via patterns]

            // How did we get here?

            // We started with a notion of task, most freqeuntly seen as

            var task0 = Task.Run<int>(() =>
            {
                Debugger.Break();
                return 10;
            });

            task0.Wait();
            Debugger.Break();

            // But task is a bit overloaded with convenience methods

            // The common case is to generate via the Run, but we can just make tasks as we like

            var task1 = Task.FromResult<int>(10);
            var task2 = Task.FromException<int>(new NotImplementedException("nooooooo"));

            // Task itself is really a batch of work perhaps like a future in other languages

            var tcs1 = new TaskCompletionSource<int>();
            var task3 = tcs1.Task;

            ThreadPool.QueueUserWorkItem(delegate { tcs1.SetResult(10); });

            task3.Wait();
            Debugger.Break();

            // Or set it up and control its start

            var task4 = new Task<int>(() =>
            {
                Debugger.Break();
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

            // And you might regard async as a DSL for dealing with tasks

            var faultingTask2 = PercyThrower();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            Debug.WriteLine(faultingTask2.Status);
            Debug.WriteLine(faultingTask2.Exception);
            Debugger.Break();

            // Or more consisely

            Func<Task> taskMaker =
                async () =>
                {
                    throw new NotImplementedException("If only I'd bothered");
                };

            // Though using the async syntax is not quite like the normal Task.Run

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
            // See the Status and the Exception

            // But there's mystery happening

            try
            {
                await faultingTask3;
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

            // And we see something different in the catch
            //  ... so by design the await takes the first exception of the aggregate so that the above works ok

            // With the classic gotcha

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

            // So we took the Task library, and added support for the front end code generation

            Func<Task> getAwaiter = async () =>
            {
                Console.WriteLine("Hello");
                await Task.Delay(TimeSpan.FromSeconds(2));
                Console.WriteLine("I'm still here");
            };

            var theTask = getAwaiter();
            var theAwaiter = theTask.GetAwaiter();
            theAwaiter.OnCompleted(() => Debug.WriteLine("And I finally finished"));

            Thread.Sleep(TimeSpan.FromSeconds(10));

            Debugger.Break();

            // And the synchronisation context is important too

            // BUT the real change is C# 6
            //    where we can now put an await in a finally block

            Func<Task> awaitingFinally = async () =>
            {
                try
                {
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
                await awaitingFinally();
            }
            catch (Exception)
            {
                Debugger.Break();
            }

            Debugger.Break();

            //But it's rather hard to implement that finally as a finally 
            //  finally is a thread local construct

            var finallyTask = Task.Run(() =>
            {
                try
                {
                    Debugger.Break();
                    Thread.CurrentThread.Abort();
                }
                finally
                {
                    Debugger.Break();
                }
            });

            await finallyTask;
            Debugger.Break();

            // But....

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
            Debugger.Break();
        }


        // Marking a method as async makes it into a task 
        static async Task PercyThrower()
        {
            Debugger.Break();
            throw new NotImplementedException("If only I'd bothered");
        }

    }
}
