using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;

namespace Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            // C:\Users\clive.tong\Documents\git\roslyn\src\Compilers\CSharp\Test\Emit\Emit\EditAndContinue\EditAndContinueTests.cs

            var source0 =
@"
[assembly: System.Runtime.Versioning.TargetFramework("".NETCoreApp, Version=v6.0"")]
[assembly: System.Reflection.AssemblyVersionAttribute(""1.0.0.0"")]
[assembly: System.Reflection.AssemblyConfiguration(""Debug"")]
public class C
{
    public static string F() { return ""original""; }
}";
            var source1 =
@"
[assembly: System.Runtime.Versioning.TargetFramework("".NETCoreApp, Version=v6.0"")]
[assembly: System.Reflection.AssemblyVersionAttribute(""1.0.0.0"")]
[assembly: System.Reflection.AssemblyConfiguration(""Debug"")]
public class C
{
    public static string F() { return ""modified""; }
}";

             
            var references = ImmutableArray.Create<MetadataReference>(
                MetadataReference.CreateFromFile(@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.0-preview.5.21301.5\ref\net6.0\netstandard.dll"),
                MetadataReference.CreateFromFile(@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.0-preview.5.21301.5\ref\net6.0\System.Runtime.dll"),
                MetadataReference.CreateFromFile(@"C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.0-preview.5.21301.5\ref\net6.0\mscorlib.dll")
                );

            var tree = SyntaxFactory.ParseSyntaxTree(source0, null, "foo.cs", encoding: Encoding.UTF8);
            var compilation0 = CSharpCompilation.Create(
                "test",
                new[] { tree },  
                references, 
                new CSharpCompilationOptions(Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary, optimizationLevel: Microsoft.CodeAnalysis.OptimizationLevel.Debug, deterministic: true));

            var compilation1 = compilation0.RemoveAllSyntaxTrees().AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(source1, null, "foo.cs", encoding: Encoding.UTF8));

            var method0 = compilation0.GlobalNamespace.GetMembers("C").Single().GetMembers("F").Single();
            var method1 = compilation1.GlobalNamespace.GetMembers("C").Single().GetMembers("F").Single();

            var outputFile = @"C:\Users\clive.tong\Documents\git\Play\EnCForTheWin\temp\test.dll";

            var eo = new EmitOptions(runtimeMetadataVersion: "v6.0.0", fileAlignment: 512);
            var pe = new MemoryStream();
            var pdbs = new MemoryStream();
            var result = compilation0.Emit(pe, options: eo, pdbStream: pdbs);

            Debug.Assert(result.Success);

            File.WriteAllBytes(outputFile, pe.ToArray());
            File.WriteAllBytes(Path.ChangeExtension(outputFile, "pdb"), pdbs.ToArray());

            using var md0 = ModuleMetadata.CreateFromFile(outputFile);
            var reader0 = md0.GetMetadataReader();

            var edit = new SemanticEdit(SemanticEditKind.Update, method0, method1);
            var metadata = new MemoryStream();
            var il = new MemoryStream();
            var pdb = new MemoryStream();
            var updatedMethods = new List<MethodDefinitionHandle>();

            var diff1 = compilation1.EmitDifference(
                EmitBaseline.CreateInitialBaseline(md0, handle => default(EditAndContinueMethodDebugInformation)),
                new[] { edit},
                metadata, il, pdb, updatedMethods);

            File.WriteAllBytes(outputFile + ".dmeta", metadata.ToArray());
            File.WriteAllBytes(outputFile + ".dil", il.ToArray());
            File.WriteAllBytes(outputFile + ".dpdb", pdb.ToArray());
        }
    }
}
