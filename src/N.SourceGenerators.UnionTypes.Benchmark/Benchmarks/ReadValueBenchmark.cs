using BenchmarkDotNet.Attributes;

using N.SourceGenerators.UnionTypes.Benchmark.Models;

namespace N.SourceGenerators.UnionTypes.Benchmark.Benchmarks;

[MemoryDiagnoser]
public class ReadValueBenchmark
{
    [Benchmark(Baseline = true)]
    public ValidationError Class()
        => Unions.Class.AsValidationError;

    [Benchmark]
    public ValidationError ClassSealed()
        => Unions.ClassSealed.AsValidationError;

    [Benchmark]
    public ValidationError Struct()
        => Unions.Struct.AsValidationError;

    [Benchmark]
    public ValidationError StructReadonly()
        => Unions.StructReadonly.AsValidationError;

    [Benchmark]
    public ValidationErrorStruct StructExplicitLayout()
        => Unions.StructExplicitLayout.AsValidationErrorStruct;

    [Benchmark]
    public ValidationErrorStruct StructReadonlyExplicitLayout()
        => Unions.StructReadonlyExplicitLayout.AsValidationErrorStruct;
}