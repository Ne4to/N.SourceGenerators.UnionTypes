using BenchmarkDotNet.Attributes;

using N.SourceGenerators.UnionTypes.Benchmark.Models;

namespace N.SourceGenerators.UnionTypes.Benchmark.Benchmarks;

[MemoryDiagnoser]
public class ToStringBenchmark
{
    [Benchmark(Baseline = true)]
    public string Class()
        => Unions.Class.ToString();

    [Benchmark]
    public string ClassSealed()
        => Unions.ClassSealed.ToString();

    [Benchmark]
    public string Struct()
        => Unions.Struct.ToString();

    [Benchmark]
    public string StructReadonly()
        => Unions.StructReadonly.ToString();

    [Benchmark]
    public string StructExplicitLayout()
        => Unions.StructExplicitLayout.ToString();

    [Benchmark]
    public string StructReadonlyExplicitLayout()
        => Unions.StructReadonlyExplicitLayout.ToString();
}