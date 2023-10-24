// Ported from https://andrewlock.net/creating-a-source-generator-part-2-testing-an-incremental-generator-with-snapshot-testing/

using System.Collections.Immutable;
using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace N.SourceGenerators.UnionTypes.Tests;

public static class TestHelper
{
    public static Task Verify<TGenerator>(
        string source,
        string? parametersText = null,
        Type? additionalReferenceTypeAssembly = null)
        where TGenerator : IIncrementalGenerator, new()
    {
        return Verify<TGenerator>(new[] { source }, parametersText, additionalReferenceTypeAssembly);
    }

    public static async Task Verify<TGenerator>(
        string[] sources,
        string? parametersText = null,
        Type? additionalReferenceTypeAssembly = null)
        where TGenerator : IIncrementalGenerator, new()
    {
        CSharpCompilation? previousCompilation = null;

        for (int sourceIndex = 0; sourceIndex < sources.Length; sourceIndex++)
        {
            string source = sources[sourceIndex];
            // Parse the provided string into a C# syntax tree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);
            // Create references for assemblies we require
            // We could add multiple references if required
            var references = new List<PortableExecutableReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
            };

            if (additionalReferenceTypeAssembly != null)
            {
                // TODO fix reference to System.Text.Json
                references.Add(MetadataReference.CreateFromFile(additionalReferenceTypeAssembly.Assembly.Location));
                // System.Text.Json.JsonSerializer
                // references.Add(MetadataReference.CreateFromFile(typeof(System.Runtime.CompilerServices.RuntimeHelpers).Assembly.Location));
                
                // // System.Runtime.InteropServices
                // references.Add(MetadataReference.CreateFromFile(typeof(System.Runtime.InteropServices.RuntimeEnvironment).Assembly.Location));
                //
                // // System.Memory
                // references.Add(MetadataReference.CreateFromFile(typeof(System.Buffers.MemoryPool<>).Assembly.Location));
                //
                // // System.Collections
                // references.Add(MetadataReference.CreateFromFile(typeof(System.Collections.BitArray).Assembly.Location));
                //
                // // System.Collections.Concurrent
                // references.Add(MetadataReference.CreateFromFile(typeof(System.Collections.Concurrent.ConcurrentBag<>).Assembly.Location));
                //
                // // System.Text.Encodings.Web
                // references.Add(MetadataReference.CreateFromFile(typeof(System.Text.Encodings.Web.TextEncoder).Assembly.Location));

                references.Add(MetadataReference.CreateFromFile(Assembly
                    .Load(new AssemblyName("System.Runtime")).Location));
                
                // references.Add(MetadataReference.CreateFromFile(Assembly
                //     .Load(new AssemblyName("System.ComponentModel.Primitives")).Location));
            }
            // /usr/local/share/dotnet/shared/Microsoft.NETCore.App/7.0.11/System.Text.Json.dll
            // Create a Roslyn compilation for the syntax tree.
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName: $"Tests{sourceIndex}",
                syntaxTrees: new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            if (previousCompilation != null)
            {
                compilation = compilation.AddReferences(previousCompilation.ToMetadataReference());
            }

            // Create an instance of our EnumGenerator incremental source generator
            var generator = new TGenerator();

            // The GeneratorDriver is used to run our generator against a compilation
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            // Run the source generator!
            driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation,
                out ImmutableArray<Diagnostic> _);

            // check code can be compiled
            foreach (var diagnostic in outputCompilation.GetDiagnostics())
            {
                if (diagnostic.DefaultSeverity == DiagnosticSeverity.Error)
                {
                    throw new InvalidOperationException($"Compilation error: {diagnostic}");
                }
            }

            // Use verify to snapshot test the source generator output!
            SettingsTask settingsTask = Verifier
                .Verify(driver)
                .UseDirectory("Snapshots");

            if (sources.Length > 1)
            {
                settingsTask = settingsTask
                    .UseTextForParameters(sourceIndex.ToString());
            }

            if (parametersText != null)
            {
                settingsTask = settingsTask
                    .UseTextForParameters(parametersText);
            }

            await settingsTask;

            previousCompilation = outputCompilation as CSharpCompilation;
        }
    }
}