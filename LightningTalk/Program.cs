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
            goto debugging;

            // https://en.wikipedia.org/wiki/Give_Me_Convenience_or_Give_Me_Death#/media/File:Dead_Kennedys_-_Give_Me_Convenience_or_Give_Me_Death_cover.jpg
            Console.WriteLine();

            // New language features could be implemented using a library

            // But unlike other languages C# doesn't have a flexible syntax or a macro language to exploit it (think Internal DSL)

            // So we end up instead with a library, new syntax in the language, and a hardwired code generating front end that sits on top of the library
            //     [though this is extensible via patterns]

            // Example:

            var task1 = new Task(() =>
            {
                System.Diagnostics.Debug.WriteLine("Starting");
                Thread.Sleep(TimeSpan.FromSeconds(2));
                System.Diagnostics.Debug.WriteLine("Finished");
            });

            task1.Start();

            task1.Wait();

            Console.WriteLine();

            // And of course there's a way to handle exceptions too

            var faultingTask = Task.Run(() => { throw new NotImplementedException("If only I'd bothered"); });

            Thread.Sleep(TimeSpan.FromSeconds(1));

            System.Diagnostics.Debug.WriteLine(faultingTask.Status);
            System.Diagnostics.Debug.WriteLine(faultingTask.Exception);

            Console.WriteLine();

            // And you might regard async as a DSL for dealing with tasks

            var faultingTask2 = PercyThrower();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            System.Diagnostics.Debug.WriteLine(faultingTask2.Status);
            System.Diagnostics.Debug.WriteLine(faultingTask2.Exception);

            Console.WriteLine();

            // Or more consisely

            Func<Task> taskMaker =
                async () =>
                {
                    throw new NotImplementedException("If only I'd bothered");
                };

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



            // So we took the Task library, and added support for the front end code generation

            Func<Task> getAwaiter = async () =>
            {
                Console.WriteLine("Hello");
                await Task.Delay(TimeSpan.FromSeconds(2));
                Console.WriteLine("I'm still here");
            };

            var theAwaiter = getAwaiter().GetAwaiter();
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

        //Func<Task> awaitingFinally2 = async () =>
        //{
        //    try
        //    {
        //        ThrowMysteryException();
        //    }
        //    finally
        //    {
        //        System.Diagnostics.Debug.WriteLine("Inside Finally");
        //        await Task.Delay(TimeSpan.FromSeconds(5));
        //        System.Diagnostics.Debug.WriteLine("Finishing Finally");
        //    }
        //};

        //try
        //{
        //    await awaitingFinally2();
        //}
        //catch (Exception)
        //{ }

        //try
        //{
        //    await Task.Run(() => FinallyExample());
        //}
        //catch (Exception)
        //{
        //}

        //try
        //{
        //    await Task.Run(() => FinallyExample2());
        //}
        //catch (Exception)
        //{
        //}

        // But not as lambda expressions

        debugging:;
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
