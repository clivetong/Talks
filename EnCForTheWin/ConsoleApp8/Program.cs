using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace ConsoleApp8
{
    // !bpmd System.Private.CoreLib.dll System.Reflection.Metadata.AssemblyExtensions.ApplyUpdate
    // DOTNET_MODIFIABLE_ASSEMBLIES
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            { 
                Console.WriteLine(C.F());
                Console.WriteLine(D.F());

                var meta = File.ReadAllBytes(@"C:\Users\clive.tong\Desktop\test.dll.dmeta");
                var il = File.ReadAllBytes(@"C:\Users\clive.tong\Desktop\test.dll.dil");
                var pdb = File.ReadAllBytes(@"C:\Users\clive.tong\Desktop\test.dll.dpdb");

                var assembly = typeof(C).Assembly;

                System.Reflection.Metadata.AssemblyExtensions.ApplyUpdate(assembly, meta, il, new byte[0]);

                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
            Console.WriteLine("sksksddk");
        } 

         
        static string Message() => "Hello";
    }
}
