using BenchmarkDotNet.Attributes;

using N.SourceGenerators.UnionTypes.Benchmark.Models;

namespace N.SourceGenerators.UnionTypes.Benchmark.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class ToStringBenchmark
{
    private readonly ClassUnion _class;
    private readonly ClassUnionNoBoxing _classNoBoxing;
    private readonly ClassUnionWithLayout _classWithLayout;
    private readonly ClassGenerated _classGenerated;
    
    private readonly StructUnion _struct;
    private readonly StructUnionWithLayout _structWithLayout;
    private readonly StructUnionWithLayoutNoBoxing _structWithLayoutNoBoxing;
    private readonly StructGenerated _structGenerated;

    public ToStringBenchmark()
    {
        _class = new ClassUnion(new ValidationError("something went wrong"));
        _classNoBoxing = new ClassUnionNoBoxing(new ValidationError("something went wrong"));
        _classWithLayout = new ClassUnionWithLayout(new ValidationError("something went wrong"));
        _classGenerated = new ClassGenerated(new ValidationError("something went wrong"));
        
        _struct = new StructUnion(new ValidationErrorStruct(42));
        _structWithLayout = new StructUnionWithLayout(new ValidationErrorStruct(42));
        _structWithLayoutNoBoxing = new StructUnionWithLayoutNoBoxing(new ValidationErrorStruct(42));
        _structGenerated = new StructGenerated(new ValidationErrorStruct(42));
    }
    
    [Benchmark(Baseline = true)]
    public string Class() =>
        _class.ToString();
    
    [Benchmark]
    public string ClassNoBoxing() =>
        _classNoBoxing.ToString();

    [Benchmark]
    public string ClassWithLayout() =>
        _classWithLayout.ToString();
    
    [Benchmark]
    public string ClassGenerated() =>
        _classGenerated.ToString();

    [Benchmark]
    public string Struct() =>
        _struct.ToString();

    [Benchmark]
    public string StructWithLayout() =>
        _structWithLayout.ToString();

    [Benchmark]
    public string StructWithLayoutNoBoxing() =>
        _structWithLayoutNoBoxing.ToString();
    
    [Benchmark]
    public string StructGenerated() =>
        _structGenerated.ToString();
}