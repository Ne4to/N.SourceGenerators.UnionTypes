namespace N.SourceGenerators.UnionTypes.Models;

internal record CompilationContext(
    bool SupportsNotNullWhenAttribute, 
    bool SupportsThrowIfNull, 
    bool NullableContextEnabled,
    bool SupportsAutoDefaultField);