using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace N.SourceGenerators.UnionTypes.Tests;

public class GeneratorAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
{
    public GeneratorAnalyzerConfigOptionsProvider(AnalyzerConfigOptions? globalOptions)
    {
        GlobalOptions = globalOptions ?? UnionTypesAnalyzerConfigOptions.Default;
    }

    public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
    {
        return UnionTypesAnalyzerConfigOptions.Default;
    }

    public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
    {
        return UnionTypesAnalyzerConfigOptions.Default;
    }

    public override AnalyzerConfigOptions GlobalOptions { get; }
}