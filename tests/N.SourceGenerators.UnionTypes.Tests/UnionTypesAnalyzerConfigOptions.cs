using System.Diagnostics.CodeAnalysis;

using Microsoft.CodeAnalysis.Diagnostics;

namespace N.SourceGenerators.UnionTypes.Tests;

public class UnionTypesAnalyzerConfigOptions : AnalyzerConfigOptions
{
    private readonly Dictionary<string, string> _options = new();

    public static UnionTypesAnalyzerConfigOptions Default { get; } = new();

    public bool ExcludeFromCodeCoverage
    {
        get => _options.GetValueOrDefault(UnionTypesGenerator.ExcludeFromCodeCoverageOption) == "true";
        set => _options[UnionTypesGenerator.ExcludeFromCodeCoverageOption] = value.ToString();
    }

    public override bool TryGetValue(string key, [UnscopedRef] out string? value)
    {
        return _options.TryGetValue(key, out value);
    }
}