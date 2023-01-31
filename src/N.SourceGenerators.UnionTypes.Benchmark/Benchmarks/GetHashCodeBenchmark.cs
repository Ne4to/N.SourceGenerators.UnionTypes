using BenchmarkDotNet.Attributes;

namespace N.SourceGenerators.UnionTypes.Benchmark.Benchmarks;

[MemoryDiagnoser]
public class GetHashCodeBenchmark
{
    [Benchmark(Baseline = true)]
    public int Class() 
        => Unions.Class.GetHashCode();

    [Benchmark]
    public int ClassSealed() 
        => Unions.ClassSealed.GetHashCode();

    [Benchmark]
    public int Struct() 
        => Unions.Struct.GetHashCode();

    [Benchmark]
    public int StructReadonly() 
        => Unions.StructReadonly.GetHashCode();

    [Benchmark]
    public int StructExplicitLayout() 
        => Unions.StructExplicitLayout.GetHashCode();

    [Benchmark]
    public int StructReadonlyExplicitLayout() 
        => Unions.StructReadonlyExplicitLayout.GetHashCode();
}