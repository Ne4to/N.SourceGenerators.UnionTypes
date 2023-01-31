using N.SourceGenerators.UnionTypes.Benchmark.Models;

namespace N.SourceGenerators.UnionTypes.Benchmark.Benchmarks;

internal static class Unions
{
    public static Class Class { get; }
        = new Class(new ValidationError(42));

    public static ClassSealed ClassSealed { get; }
        = new ClassSealed(new ValidationError(42));

    public static Struct Struct { get; }
        = new Struct(new ValidationError(42));

    public static StructReadonly StructReadonly { get; }
        = new StructReadonly(new ValidationError(42));

    public static StructExplicitLayout StructExplicitLayout { get; }
        = new StructExplicitLayout(new ValidationErrorStruct(42));

    public static StructReadonlyExplicitLayout StructReadonlyExplicitLayout { get; }
        = new StructReadonlyExplicitLayout(new ValidationErrorStruct(42));
}