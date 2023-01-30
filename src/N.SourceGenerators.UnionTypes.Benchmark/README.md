## CtorBenchmark
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-KMZJRE : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|           Method |     Mean |     Error |    StdDev | Ratio | Code Size |   Gen0 | Allocated | Alloc Ratio |
|----------------- |---------:|----------:|----------:|------:|----------:|-------:|----------:|------------:|
|            Class | 8.669 ns | 3.0779 ns | 0.1687 ns |  1.00 |      86 B | 0.0153 |      64 B |        1.00 |
|  ClassWithLayout | 7.604 ns | 0.7412 ns | 0.0406 ns |  0.88 |      93 B | 0.0134 |      56 B |        0.88 |
|   ClassGenerated | 7.409 ns | 1.6346 ns | 0.0896 ns |  0.85 |      86 B | 0.0153 |      64 B |        1.00 |
|           Struct | 5.221 ns | 0.3375 ns | 0.0185 ns |  0.60 |      59 B |      - |         - |        0.00 |
| StructWithLayout | 4.176 ns | 1.0851 ns | 0.0595 ns |  0.48 |      31 B |      - |         - |        0.00 |
|  StructGenerated | 4.134 ns | 0.2065 ns | 0.0113 ns |  0.48 |      31 B |      - |         - |        0.00 |

## HashBenchmark
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-WFLBOV : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                   Method |      Mean |      Error |    StdDev | Ratio | RatioSD | Code Size |   Gen0 | Allocated | Alloc Ratio |
|------------------------- |----------:|-----------:|----------:|------:|--------:|----------:|-------:|----------:|------------:|
|                    Class | 23.142 ns | 14.0757 ns | 0.7715 ns |  1.00 |    0.00 |      15 B |      - |         - |          NA |
|            ClassNoBoxing | 20.706 ns |  2.9338 ns | 0.1608 ns |  0.90 |    0.03 |      15 B |      - |         - |          NA |
|          ClassWithLayout | 21.287 ns |  1.8445 ns | 0.1011 ns |  0.92 |    0.03 |      15 B |      - |         - |          NA |
|           ClassGenerated | 20.740 ns |  3.6116 ns | 0.1980 ns |  0.90 |    0.02 |      15 B |      - |         - |          NA |
|                   Struct |  7.234 ns |  0.8079 ns | 0.0443 ns |  0.31 |    0.01 |     260 B | 0.0057 |      24 B |          NA |
|         StructWithLayout |  7.070 ns |  1.0633 ns | 0.0583 ns |  0.31 |    0.01 |     236 B | 0.0057 |      24 B |          NA |
| StructWithLayoutNoBoxing |  1.963 ns |  0.5453 ns | 0.0299 ns |  0.08 |    0.00 |     137 B |      - |         - |          NA |
|          StructGenerated |  1.706 ns |  0.5156 ns | 0.0283 ns |  0.07 |    0.00 |     137 B |      - |         - |          NA |

## ReadValueBenchmark
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-NYNKQH : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|           Method |      Mean |     Error |    StdDev | Ratio | RatioSD | Code Size | Allocated | Alloc Ratio |
|----------------- |----------:|----------:|----------:|------:|--------:|----------:|----------:|------------:|
|            Class | 1.3420 ns | 0.4722 ns | 0.0259 ns |  1.00 |    0.00 |      91 B |         - |          NA |
|  ClassWithLayout | 1.3604 ns | 0.5252 ns | 0.0288 ns |  1.01 |    0.01 |      92 B |         - |          NA |
|   ClassGenerated | 1.3678 ns | 0.3114 ns | 0.0171 ns |  1.02 |    0.03 |      91 B |         - |          NA |
|           Struct | 1.4075 ns | 0.2281 ns | 0.0125 ns |  1.05 |    0.03 |     132 B |         - |          NA |
| StructWithLayout | 0.0478 ns | 0.1688 ns | 0.0093 ns |  0.04 |    0.01 |      90 B |         - |          NA |
|  StructGenerated | 0.0526 ns | 0.1921 ns | 0.0105 ns |  0.04 |    0.01 |      90 B |         - |          NA |

## ToStringBenchmark
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-VCROUC : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                   Method |      Mean |      Error |   StdDev | Ratio | RatioSD | Code Size |   Gen0 | Allocated | Alloc Ratio |
|------------------------- |----------:|-----------:|---------:|------:|--------:|----------:|-------:|----------:|------------:|
|                    Class | 167.01 ns | 125.408 ns | 6.874 ns |  1.00 |    0.00 |      15 B | 0.1509 |     632 B |        1.00 |
|            ClassNoBoxing | 160.93 ns |  15.805 ns | 0.866 ns |  0.96 |    0.05 |      15 B | 0.1509 |     632 B |        1.00 |
|          ClassWithLayout | 154.37 ns |   6.974 ns | 0.382 ns |  0.93 |    0.04 |      15 B | 0.1509 |     632 B |        1.00 |
|           ClassGenerated |  94.86 ns |  11.623 ns | 0.637 ns |  0.57 |    0.02 |      15 B | 0.1128 |     472 B |        0.75 |
|                   Struct | 173.59 ns |  71.155 ns | 3.900 ns |  1.04 |    0.03 |     436 B | 0.1566 |     656 B |        1.04 |
|         StructWithLayout | 165.27 ns |  15.396 ns | 0.844 ns |  0.99 |    0.04 |     424 B | 0.1566 |     656 B |        1.04 |
| StructWithLayoutNoBoxing | 161.67 ns |  44.898 ns | 2.461 ns |  0.97 |    0.03 |   1,256 B | 0.1509 |     632 B |        1.00 |
|          StructGenerated | 100.20 ns |  30.708 ns | 1.683 ns |  0.60 |    0.02 |     156 B | 0.1147 |     480 B |        0.76 |

