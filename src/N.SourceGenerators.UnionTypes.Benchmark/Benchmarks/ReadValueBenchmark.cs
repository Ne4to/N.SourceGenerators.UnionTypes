using BenchmarkDotNet.Attributes;

using N.SourceGenerators.UnionTypes.Benchmark.Models;

namespace N.SourceGenerators.UnionTypes.Benchmark.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class ReadValueBenchmark
{
    private readonly ClassUnion _class;
    private readonly ClassUnionWithLayout _classWithLayout;
    private readonly ClassGenerated _classGenerated;
    private readonly StructUnion _struct;
    private readonly StructUnionWithLayout _structWithLayout;
    private readonly StructGenerated _structGenerated;

    public ReadValueBenchmark()
    {
        _class = new ClassUnion(new ValidationError("something went wrong"));
        _classWithLayout = new ClassUnionWithLayout(new ValidationError("something went wrong"));
        _classGenerated = new ClassGenerated(new ValidationError("something went wrong"));

        _struct = new StructUnion(new ValidationErrorStruct(42));
        _structWithLayout = new StructUnionWithLayout(new ValidationErrorStruct(42));
        _structGenerated = new StructGenerated(new ValidationErrorStruct(42));
    }

    [Benchmark(Baseline = true)]
    public ValidationError Class() =>
        _class.AsValidationError;

    [Benchmark]
    public ValidationError ClassWithLayout() =>
        _classWithLayout.AsValidationError;

    [Benchmark]
    public ValidationError ClassGenerated() =>
        _classGenerated.AsValidationError;

    [Benchmark]
    public ValidationErrorStruct Struct() =>
        _struct.AsValidationErrorStruct;

    [Benchmark]
    public ValidationErrorStruct StructWithLayout() =>
        _structWithLayout.AsValidationErrorStruct;

    [Benchmark]
    public ValidationErrorStruct StructGenerated() =>
        _structGenerated.AsValidationErrorStruct;
}