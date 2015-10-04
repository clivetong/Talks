using System;
using System.Collections.Generic;
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
                Thread.Sleep(TimeSpan.FromSeconds(1));
                return 10;
            });

            task0.Wait();

            // But task is a bit overloaded with convenience methods

            // The common case is to generate via the Run, but we can just make tasks as we like

            var task1 = Task.FromResult<int>(10);
            var task2 = Task.FromException<int>(new NotImplementedException("nooooooo"));

            // Task itself is really a batch of work perhaps like a future in other languages

            var tcs1 = new TaskCompletionSource<int>();
            var task3 = tcs1.Task;

            ThreadPool.QueueUserWorkItem(delegate { tcs1.SetResult(10); });

            task3.Wait();

            // Or set it up and control its start

            var task4 = new Task<int>(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                return 10;
            });

            task4.Start();

            task4.Wait();

            // And of course there's a way to handle exceptions too

            var faultingTask = Task.Run(() => { throw new NotImplementedException("If only I'd bothered"); });

            Thread.Sleep(TimeSpan.FromSeconds(1));

            System.Diagnostics.Debug.WriteLine(faultingTask.Status);
            System.Diagnostics.Debug.WriteLine(faultingTask.Exception);

            // And you might regard async as a DSL for dealing with tasks

            var faultingTask2 = PercyThrower();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            System.Diagnostics.Debug.WriteLine(faultingTask2.Status);
            System.Diagnostics.Debug.WriteLine(faultingTask2.Exception);

            // Or more consisely

            Func<Task> taskMaker =
                async () =>
                {
                    throw new NotImplementedException("If only I'd bothered");
                };

            // Though using the async syntax is not quite like the normal Task.Run

            Func<Task> l = async () =>
            {
                // Running on start thread
                await Task.Yield();
                //Running on threadpool thread
                await Task.Yield();
            };

            await l();

            // But back to the example

            var faultingTask3 = taskMaker();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            System.Diagnostics.Debug.WriteLine(faultingTask3.Status);
            System.Diagnostics.Debug.WriteLine(faultingTask3.Exception);
          
            // But there's mystery happening

            try
            {
                await faultingTask3;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            // And we see something different in the catch
            //  ... so by design the await takes the first exception of the aggregate so that the above works ok

            // With the classic gotcha

            var waitingForYouAll = Task.WhenAll(taskMaker(), taskMaker());

            System.Diagnostics.Debug.WriteLine(waitingForYouAll.Status);
            System.Diagnostics.Debug.WriteLine(waitingForYouAll.Exception);

            try
            {
                await waitingForYouAll;
            }
            catch (Exception ex)
            {
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
            theAwaiter.OnCompleted(() => System.Diagnostics.Debug.WriteLine("And I finally finished"));

            Thread.Sleep(TimeSpan.FromSeconds(10));

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
                    System.Diagnostics.Debug.WriteLine("Inside Finally");
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    System.Diagnostics.Debug.WriteLine("Finishing Finally");
                }
            };

            try
            {
                await awaitingFinally();
            }
            catch (Exception)
            { }

        //But it's rather hard to implement that finally as a finally 
        //  finally is a thread local construct

            var finallyTask = Task.Run(() =>
            {
                try
                {
                    Thread.CurrentThread.Abort();
                }
                finally
                {
                    System.Diagnostics.Debug.WriteLine("Here");
                }
            });

            await finallyTask;


            Func<Task> awaitingFinally2 = async () =>
            {
                await Task.Yield();
                try
                {
                    System.Diagnostics.Debug.WriteLine("Here we go...");
                    Thread.CurrentThread.Abort();
                }
                finally
                {
                    System.Diagnostics.Debug.WriteLine("Inside Finally");
                    System.Diagnostics.Debug.WriteLine("Finishing Finally");
                }
            };


            var task = awaitingFinally2();

            Thread.Sleep(TimeSpan.FromSeconds(5));

            Func<Task> awaitingFinally3 = async () =>
            {
                await Task.Yield();
                try
                {
                    System.Diagnostics.Debug.WriteLine("Here we go...");
                    Thread.CurrentThread.Abort();
                }
                finally
                {
                    System.Diagnostics.Debug.WriteLine("Inside Finally");
                    await Task.Delay(TimeSpan.FromSeconds(10));
                    System.Diagnostics.Debug.WriteLine("Finishing Finally");
                }
            };


            var withAwaitInFinally = awaitingFinally3();

            Thread.Sleep(TimeSpan.FromSeconds(7));
        }


        // Marking a method as async makes it into a task 
        static async Task PercyThrower()
        {
            throw new NotImplementedException("If only I'd bothered");
        }


        static void ThrowMysteryException()
        {
            Thread.CurrentThread.Abort();
        }

        static async void FinallyExample()
        {
            try
            {
                ThrowMysteryException();
            }
            finally
            {
                System.Diagnostics.Debug.WriteLine("Inside Finally");
                System.Diagnostics.Debug.WriteLine("Finishing Finally");
            }

        }

        static async void FinallyExample2()
        {
            try
            {
                ThrowMysteryException();
            }
            finally
            {
                System.Diagnostics.Debug.WriteLine("Inside Finally");
                await Task.Delay(TimeSpan.FromSeconds(5));
                System.Diagnostics.Debug.WriteLine("Finishing Finally");
            }

        }
    }
}
