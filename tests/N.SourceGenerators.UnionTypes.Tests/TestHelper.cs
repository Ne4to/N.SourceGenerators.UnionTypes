﻿// Ported from https://andrewlock.net/creating-a-source-generator-part-2-testing-an-incremental-generator-with-snapshot-testing/

using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace N.SourceGenerators.UnionTypes.Tests;

public static class TestHelper
{
    public static Task Verify<TGenerator>(
        string source,
        LanguageVersion languageVersion = LanguageVersion.Latest,
        string? parametersText = null,
        params PortableExecutableReference[] additionalReferences)
        where TGenerator : IIncrementalGenerator, new()
    {
        return Verify<TGenerator>(
            [source], 
            languageVersion, 
            parametersText,
            additionalReferences);
    }

    public static async Task Verify<TGenerator>(
        string[] sources,
        LanguageVersion languageVersion = LanguageVersion.Latest,
        string? parametersText = null,
        params PortableExecutableReference[] additionalReferences)
        where TGenerator : IIncrementalGenerator, new()
    {
        CSharpCompilation? previousCompilation = null;

        for (int sourceIndex = 0; sourceIndex < sources.Length; sourceIndex++)
        {
            string source = sources[sourceIndex];
            // Parse the provided string into a C# syntax tree
            var options = CSharpParseOptions.Default.WithLanguageVersion(languageVersion);
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source, options);
            // Create references for assemblies we require
            // We could add multiple references if required
            var references = new List<PortableExecutableReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
            };
            
            references.AddRange(additionalReferences);

            // /usr/local/share/dotnet/shared/Microsoft.NETCore.App/7.0.11/System.Text.Json.dll
            // Create a Roslyn compilation for the syntax tree.
            CSharpCompilationOptions compilationOptions = new(OutputKind.DynamicallyLinkedLibrary);
            
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName: $"Tests{sourceIndex}",
                syntaxTrees: new[] { syntaxTree },
                references,
                compilationOptions);
            
            if (previousCompilation != null)
            {
                compilation = compilation.AddReferences(previousCompilation.ToMetadataReference());
            }

            // Create an instance of our EnumGenerator incremental source generator
            var generator = new TGenerator();

            // The GeneratorDriver is used to run our generator against a compilation
            GeneratorDriver driver = CSharpGeneratorDriver.Create(parseOptions: options, generators: [generator.AsSourceGenerator()]);

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