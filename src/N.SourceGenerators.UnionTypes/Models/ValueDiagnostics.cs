namespace N.SourceGenerators.UnionTypes.Models;

internal record ValueDiagnostics<T>(T Value, IReadOnlyList<Diagnostic> Diagnostics);