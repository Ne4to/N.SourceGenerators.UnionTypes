// Ported from https://andrewlock.net/creating-a-source-generator-part-2-testing-an-incremental-generator-with-snapshot-testing/

using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace N.SourceGenerators.UnionTypes.Tests;

public static class TestHelper
{
    public static Task Verify<TGenerator>(string source, string? parametersText = null)
        where TGenerator : IIncrementalGenerator, new()
    {
        return Verify<TGenerator>(new[] { source }, parametersText);
    }

    public static async Task Verify<TGenerator>(string[] sources, string? parametersText = null)
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
            IEnumerable<PortableExecutableReference> references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
            };

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