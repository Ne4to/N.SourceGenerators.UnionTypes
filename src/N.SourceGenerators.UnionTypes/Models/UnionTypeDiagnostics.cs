namespace N.SourceGenerators.UnionTypes.Models;

internal record UnionTypeDiagnostics(UnionType UnionType, IReadOnlyList<Diagnostic> Diagnostics)
{
    public UnionType UnionType { get; } = UnionType;
    public IReadOnlyList<Diagnostic> Diagnostics { get; } = Diagnostics;
}