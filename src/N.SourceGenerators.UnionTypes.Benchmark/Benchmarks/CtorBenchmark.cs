using BenchmarkDotNet.Attributes;

using N.SourceGenerators.UnionTypes.Benchmark.Models;

namespace N.SourceGenerators.UnionTypes.Benchmark.Benchmarks;

[MemoryDiagnoser]
public class CtorBenchmark
{
    [Benchmark(Baseline = true)]
    public Class Class() 
        => new Class(new ValidationError(42));
    
    [Benchmark]
    public ClassSealed ClassSealed() 
        => new ClassSealed(new ValidationError(42));
    
    [Benchmark]
    public Struct Struct() 
        => new Struct(new ValidationError(42));
    
    [Benchmark]
    public StructReadonly StructReadonly() 
        => new StructReadonly(new ValidationError(42));
    
    [Benchmark]
    public StructExplicitLayout StructExplicitLayout() 
        => new StructExplicitLayout(new ValidationErrorStruct(42));
    
    [Benchmark]
    public StructReadonlyExplicitLayout StructReadonlyExplicitLayout() 
        => new StructReadonlyExplicitLayout(new ValidationErrorStruct(42));
}