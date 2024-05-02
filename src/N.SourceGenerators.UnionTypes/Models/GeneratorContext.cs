namespace N.SourceGenerators.UnionTypes.Models;

internal record GeneratorContext(CompilationContext Compilation, GeneratorOptions Options);

internal record CompilationContext(
    bool SupportsNotNullWhenAttribute,
    bool SupportsThrowIfNull,
    bool NullableContextEnabled,
    bool SupportsAutoDefaultField);

internal record GeneratorOptions(bool ExcludeFromCodeCoverage);