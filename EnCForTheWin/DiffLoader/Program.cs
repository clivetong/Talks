using System;
using System.IO;
using System.Threading;

namespace DiffLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            { 
                Console.WriteLine(C.F());

                var meta = File.ReadAllBytes(@"C:\Users\clive.tong\Documents\git\Play\EnCForTheWin\temp\test.dll.dmeta");
                var il = File.ReadAllBytes(@"C:\Users\clive.tong\Documents\git\Play\EnCForTheWin\temp\test.dll.dil");

                var assembly = typeof(C).Assembly;

                System.Reflection.Metadata.AssemblyExtensions.ApplyUpdate(assembly, meta, il, new byte[0]);

                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

    }
}
