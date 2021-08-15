using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TheInnerLoop
{
    static class Program
    {

        static async Task Main(string[] args)
        {
            await Task.Run(() => Recurse(10));
            Console.ReadLine();
        }

        private static void Recurse(int v)
        {
            #region Uncomment and remove sleep to see interesting debug effect
            //if (v==5)
            //{
            //    Debugger.Break();
            //}
            #endregion
            if (v != 0)
            {
                Console.WriteLine("Entering");
                Thread.Sleep(TimeSpan.FromSeconds(5));
                Recurse(v - 1);
                Console.WriteLine("Exiting");
            }
        }
    }
}
