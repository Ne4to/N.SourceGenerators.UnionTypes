using BenchmarkDotNet.Attributes;

using N.SourceGenerators.UnionTypes.Benchmark.Models;

namespace N.SourceGenerators.UnionTypes.Benchmark.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class HashBenchmark
{
    private readonly ClassUnion _class;
    private readonly ClassUnionNoBoxing _classNoBoxing;
    private readonly ClassUnionWithLayout _classWithLayout;
    private readonly ClassGenerated _classGenerated;
    
    private readonly StructUnion _struct;
    private readonly StructUnionWithLayout _structWithLayout;
    private readonly StructUnionWithLayoutNoBoxing _structWithLayoutNoBoxing;
    private readonly StructGenerated _structGenerated;

    public HashBenchmark()
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
    public int Class() =>
        _class.GetHashCode();
    
    [Benchmark]
    public int ClassNoBoxing() =>
        _classNoBoxing.GetHashCode();

    [Benchmark]
    public int ClassWithLayout() =>
        _classWithLayout.GetHashCode();
    
    [Benchmark]
    public int ClassGenerated() =>
        _classGenerated.GetHashCode();

    [Benchmark]
    public int Struct() =>
        _struct.GetHashCode();

    [Benchmark]
    public int StructWithLayout() =>
        _structWithLayout.GetHashCode();

    [Benchmark]
    public int StructWithLayoutNoBoxing() =>
        _structWithLayoutNoBoxing.GetHashCode();

    [Benchmark]
    public int StructGenerated() =>
        _structGenerated.GetHashCode();
}