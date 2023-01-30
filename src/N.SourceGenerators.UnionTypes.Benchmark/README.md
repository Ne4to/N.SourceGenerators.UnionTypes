## CtorBenchmark
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-UGDXBQ : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|           Method |     Mean |     Error |    StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|----------------- |---------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
|            Class | 8.122 ns | 2.2582 ns | 0.1238 ns |  1.00 |    0.00 | 0.0153 |      64 B |        1.00 |
|  ClassWithLayout | 8.368 ns | 2.7381 ns | 0.1501 ns |  1.03 |    0.03 | 0.0134 |      56 B |        0.88 |
|           Struct | 5.504 ns | 1.8887 ns | 0.1035 ns |  0.68 |    0.02 |      - |         - |        0.00 |
| StructWithLayout | 4.383 ns | 0.5708 ns | 0.0313 ns |  0.54 |    0.01 |      - |         - |        0.00 |

## HashBenchmark
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-TDHXMD : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                   Method |      Mean |      Error |    StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|------------------------- |----------:|-----------:|----------:|------:|--------:|-------:|----------:|------------:|
|                    Class | 27.926 ns |  2.9969 ns | 0.1643 ns |  1.00 |    0.00 |      - |         - |          NA |
|            ClassNoBoxing | 22.893 ns |  5.4500 ns | 0.2987 ns |  0.82 |    0.01 |      - |         - |          NA |
|          ClassWithLayout | 24.264 ns | 11.7406 ns | 0.6435 ns |  0.87 |    0.02 |      - |         - |          NA |
|                   Struct |  9.759 ns |  6.1008 ns | 0.3344 ns |  0.35 |    0.01 | 0.0057 |      24 B |          NA |
|         StructWithLayout |  9.000 ns |  0.2936 ns | 0.0161 ns |  0.32 |    0.00 | 0.0057 |      24 B |          NA |
| StructWithLayoutNoBoxing |  2.013 ns |  0.6225 ns | 0.0341 ns |  0.07 |    0.00 |      - |         - |          NA |

## ReadValueBenchmark
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-SCSEBH : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|           Method |      Mean |     Error |    StdDev |    Median | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------------- |----------:|----------:|----------:|----------:|------:|--------:|----------:|------------:|
|            Class | 1.5271 ns | 0.5704 ns | 0.0313 ns | 1.5196 ns |  1.00 |    0.00 |         - |          NA |
|  ClassWithLayout | 1.4549 ns | 1.0844 ns | 0.0594 ns | 1.4892 ns |  0.95 |    0.04 |         - |          NA |
|           Struct | 1.4973 ns | 0.6282 ns | 0.0344 ns | 1.4937 ns |  0.98 |    0.00 |         - |          NA |
| StructWithLayout | 0.0927 ns | 1.4655 ns | 0.0803 ns | 0.0666 ns |  0.06 |    0.05 |         - |          NA |

## ToStringBenchmark
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-YTWHJN : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                   Method |     Mean |    Error |   StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|------------------------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
|                    Class | 223.2 ns | 108.0 ns |  5.92 ns |  1.00 |    0.00 | 0.1509 |     632 B |        1.00 |
|            ClassNoBoxing | 209.7 ns | 166.7 ns |  9.14 ns |  0.94 |    0.03 | 0.1509 |     632 B |        1.00 |
|          ClassWithLayout | 241.1 ns | 673.7 ns | 36.93 ns |  1.08 |    0.19 | 0.1509 |     632 B |        1.00 |
|                   Struct | 237.2 ns | 559.7 ns | 30.68 ns |  1.06 |    0.11 | 0.1564 |     656 B |        1.04 |
|         StructWithLayout | 280.7 ns | 352.1 ns | 19.30 ns |  1.26 |    0.12 | 0.1564 |     656 B |        1.04 |
| StructWithLayoutNoBoxing | 230.2 ns | 679.4 ns | 37.24 ns |  1.03 |    0.17 | 0.1509 |     632 B |        1.00 |

