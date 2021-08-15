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
            if (v != 0)
            {
                Console.WriteLine("Entering2");
                Thread.Sleep(TimeSpan.FromSeconds(5));
                Recurse(v - 1);
                Console.WriteLine("Exiting2");
            }
        }
    }
}
