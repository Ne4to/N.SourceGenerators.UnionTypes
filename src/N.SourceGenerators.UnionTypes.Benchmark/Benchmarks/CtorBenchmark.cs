﻿using BenchmarkDotNet.Attributes;

using N.SourceGenerators.UnionTypes.Benchmark.Models;

namespace N.SourceGenerators.UnionTypes.Benchmark.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class CtorBenchmark
{
    [Benchmark(Baseline = true)]
    public ClassUnion Class() =>
        new ClassUnion(new ValidationError("something went wrong"));

    [Benchmark]
    public ClassUnionWithLayout ClassWithLayout() =>
        new ClassUnionWithLayout(new ValidationError("something went wrong"));
    
    [Benchmark]
    public ClassGenerated ClassGenerated() =>
        new ClassGenerated(new ValidationError("something went wrong"));
    
    [Benchmark]
    public StructUnion Struct() =>
        new StructUnion(new ValidationErrorStruct(42));

    [Benchmark]
    public StructUnionWithLayout StructWithLayout() =>
        new StructUnionWithLayout(new ValidationErrorStruct(42));
    
    [Benchmark]
    public StructGenerated StructGenerated() =>
        new StructGenerated(new ValidationErrorStruct(42));
}