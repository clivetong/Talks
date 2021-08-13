using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace ConsoleApp8
{
    // https://github.com/dotnet/runtime/pull/48366
    // $env:DOTNET_MODIFIABLE_ASSEMBLIES="debug"
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
