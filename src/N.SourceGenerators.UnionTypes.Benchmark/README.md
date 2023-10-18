## Ctor
``` ini

BenchmarkDotNet=v0.13.4, OS=macOS 14.0 (23A344) [Darwin 23.0.0]
Apple M2 Max, 1 CPU, 12 logical and 12 physical cores
.NET SDK=7.0.401
  [Host]     : .NET 7.0.11 (7.0.1123.42427), Arm64 RyuJIT AdvSIMD
  Job-CPQSKY : .NET 7.0.11 (7.0.1123.42427), Arm64 RyuJIT AdvSIMD

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                       Method |      Mean |     Error |    StdDev | Ratio |   Gen0 | Allocated | Alloc Ratio |
|----------------------------- |----------:|----------:|----------:|------:|-------:|----------:|------------:|
|                        Class | 6.0429 ns | 0.2688 ns | 0.0147 ns | 1.000 | 0.0086 |      72 B |        1.00 |
|                  ClassSealed | 6.0265 ns | 0.2679 ns | 0.0147 ns | 0.997 | 0.0086 |      72 B |        1.00 |
|                       Struct | 2.4038 ns | 0.2048 ns | 0.0112 ns | 0.398 | 0.0029 |      24 B |        0.33 |
|               StructReadonly | 2.4161 ns | 0.2119 ns | 0.0116 ns | 0.400 | 0.0029 |      24 B |        0.33 |
|         StructExplicitLayout | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.000 |      - |         - |        0.00 |
| StructReadonlyExplicitLayout | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.000 |      - |         - |        0.00 |

## GetHashCode
``` ini

BenchmarkDotNet=v0.13.4, OS=macOS 14.0 (23A344) [Darwin 23.0.0]
Apple M2 Max, 1 CPU, 12 logical and 12 physical cores
.NET SDK=7.0.401
  [Host]     : .NET 7.0.11 (7.0.1123.42427), Arm64 RyuJIT AdvSIMD
  Job-MVTOFC : .NET 7.0.11 (7.0.1123.42427), Arm64 RyuJIT AdvSIMD

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                       Method |     Mean |     Error |    StdDev | Ratio | Allocated | Alloc Ratio |
|----------------------------- |---------:|----------:|----------:|------:|----------:|------------:|
|                        Class | 4.119 ns | 0.0605 ns | 0.0033 ns |  1.00 |         - |          NA |
|                  ClassSealed | 4.133 ns | 0.0484 ns | 0.0027 ns |  1.00 |         - |          NA |
|                       Struct | 7.660 ns | 0.1243 ns | 0.0068 ns |  1.86 |         - |          NA |
|               StructReadonly | 7.677 ns | 0.2665 ns | 0.0146 ns |  1.86 |         - |          NA |
|         StructExplicitLayout | 1.467 ns | 0.0200 ns | 0.0011 ns |  0.36 |         - |          NA |
| StructReadonlyExplicitLayout | 1.470 ns | 0.0193 ns | 0.0011 ns |  0.36 |         - |          NA |

## ReadValue
``` ini

BenchmarkDotNet=v0.13.4, OS=macOS 14.0 (23A344) [Darwin 23.0.0]
Apple M2 Max, 1 CPU, 12 logical and 12 physical cores
.NET SDK=7.0.401
  [Host]     : .NET 7.0.11 (7.0.1123.42427), Arm64 RyuJIT AdvSIMD
  Job-NCEFIQ : .NET 7.0.11 (7.0.1123.42427), Arm64 RyuJIT AdvSIMD

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                       Method |      Mean |     Error |    StdDev | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------------------------- |----------:|----------:|----------:|------:|--------:|----------:|------------:|
|                        Class | 0.3540 ns | 0.0891 ns | 0.0049 ns |  1.00 |    0.00 |         - |          NA |
|                  ClassSealed | 0.3228 ns | 0.0092 ns | 0.0005 ns |  0.91 |    0.01 |         - |          NA |
|                       Struct | 3.2880 ns | 0.2043 ns | 0.0112 ns |  9.29 |    0.15 |         - |          NA |
|               StructReadonly | 3.2877 ns | 0.0631 ns | 0.0035 ns |  9.29 |    0.13 |         - |          NA |
|         StructExplicitLayout | 0.2804 ns | 0.0901 ns | 0.0049 ns |  0.79 |    0.02 |         - |          NA |
| StructReadonlyExplicitLayout | 0.3270 ns | 1.9317 ns | 0.1059 ns |  0.92 |    0.30 |         - |          NA |

## ToString
``` ini

BenchmarkDotNet=v0.13.4, OS=macOS 14.0 (23A344) [Darwin 23.0.0]
Apple M2 Max, 1 CPU, 12 logical and 12 physical cores
.NET SDK=7.0.401
  [Host]     : .NET 7.0.11 (7.0.1123.42427), Arm64 RyuJIT AdvSIMD
  Job-MVCCVK : .NET 7.0.11 (7.0.1123.42427), Arm64 RyuJIT AdvSIMD

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                       Method |     Mean |    Error |   StdDev | Ratio |   Gen0 | Allocated | Alloc Ratio |
|----------------------------- |---------:|---------:|---------:|------:|-------:|----------:|------------:|
|                        Class | 72.75 ns | 0.173 ns | 0.009 ns |  1.00 | 0.0564 |     472 B |        1.00 |
|                  ClassSealed | 72.83 ns | 2.486 ns | 0.136 ns |  1.00 | 0.0564 |     472 B |        1.00 |
|                       Struct | 77.59 ns | 3.612 ns | 0.198 ns |  1.07 | 0.0564 |     472 B |        1.00 |
|               StructReadonly | 76.85 ns | 2.708 ns | 0.148 ns |  1.06 | 0.0564 |     472 B |        1.00 |
|         StructExplicitLayout | 70.28 ns | 4.734 ns | 0.260 ns |  0.97 | 0.0573 |     480 B |        1.02 |
| StructReadonlyExplicitLayout | 71.65 ns | 1.785 ns | 0.098 ns |  0.98 | 0.0573 |     480 B |        1.02 |

