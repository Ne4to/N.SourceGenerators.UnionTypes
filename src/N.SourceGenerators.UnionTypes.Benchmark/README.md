## Ctor
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-BKNSSQ : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                       Method |     Mean |     Error |    StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|----------------------------- |---------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
|                        Class | 7.050 ns | 0.3546 ns | 0.0194 ns |  1.00 |    0.00 | 0.0153 |      64 B |        1.00 |
|                  ClassSealed | 6.769 ns | 2.5824 ns | 0.1416 ns |  0.96 |    0.02 | 0.0153 |      64 B |        1.00 |
|                       Struct | 3.302 ns | 0.5208 ns | 0.0285 ns |  0.47 |    0.00 | 0.0057 |      24 B |        0.38 |
|               StructReadonly | 3.264 ns | 0.6143 ns | 0.0337 ns |  0.46 |    0.00 | 0.0057 |      24 B |        0.38 |
|         StructExplicitLayout | 4.123 ns | 0.5004 ns | 0.0274 ns |  0.58 |    0.00 |      - |         - |        0.00 |
| StructReadonlyExplicitLayout | 4.115 ns | 0.8700 ns | 0.0477 ns |  0.58 |    0.01 |      - |         - |        0.00 |

## GetHashCode
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-EOFGDV : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                       Method |      Mean |     Error |    StdDev | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------------------------- |----------:|----------:|----------:|------:|--------:|----------:|------------:|
|                        Class |  6.758 ns | 0.5807 ns | 0.0318 ns |  1.00 |    0.00 |         - |          NA |
|                  ClassSealed |  6.731 ns | 0.7423 ns | 0.0407 ns |  1.00 |    0.01 |         - |          NA |
|                       Struct | 12.645 ns | 2.8756 ns | 0.1576 ns |  1.87 |    0.02 |         - |          NA |
|               StructReadonly | 12.605 ns | 1.5499 ns | 0.0850 ns |  1.87 |    0.01 |         - |          NA |
|         StructExplicitLayout |  2.465 ns | 0.1746 ns | 0.0096 ns |  0.36 |    0.00 |         - |          NA |
| StructReadonlyExplicitLayout |  2.315 ns | 0.7554 ns | 0.0414 ns |  0.34 |    0.01 |         - |          NA |

## ReadValue
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-RVTMNA : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                       Method |     Mean |     Error |    StdDev | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------------------------- |---------:|----------:|----------:|------:|--------:|----------:|------------:|
|                        Class | 1.674 ns | 0.2215 ns | 0.0121 ns |  1.00 |    0.00 |         - |          NA |
|                  ClassSealed | 1.675 ns | 0.8915 ns | 0.0489 ns |  1.00 |    0.02 |         - |          NA |
|                       Struct | 6.576 ns | 1.2893 ns | 0.0707 ns |  3.93 |    0.05 |         - |          NA |
|               StructReadonly | 6.749 ns | 2.4536 ns | 0.1345 ns |  4.03 |    0.07 |         - |          NA |
|         StructExplicitLayout | 1.727 ns | 1.0336 ns | 0.0567 ns |  1.03 |    0.03 |         - |          NA |
| StructReadonlyExplicitLayout | 1.636 ns | 0.1580 ns | 0.0087 ns |  0.98 |    0.01 |         - |          NA |

## ToString
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-PKSHPT : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                       Method |     Mean |     Error |   StdDev | Ratio |   Gen0 | Allocated | Alloc Ratio |
|----------------------------- |---------:|----------:|---------:|------:|-------:|----------:|------------:|
|                        Class | 95.39 ns | 10.200 ns | 0.559 ns |  1.00 | 0.1128 |     472 B |        1.00 |
|                  ClassSealed | 90.89 ns |  7.331 ns | 0.402 ns |  0.95 | 0.1128 |     472 B |        1.00 |
|                       Struct | 99.57 ns | 10.057 ns | 0.551 ns |  1.04 | 0.1128 |     472 B |        1.00 |
|               StructReadonly | 98.63 ns | 17.335 ns | 0.950 ns |  1.03 | 0.1128 |     472 B |        1.00 |
|         StructExplicitLayout | 94.54 ns |  5.686 ns | 0.312 ns |  0.99 | 0.1147 |     480 B |        1.02 |
| StructReadonlyExplicitLayout | 95.61 ns | 11.132 ns | 0.610 ns |  1.00 | 0.1147 |     480 B |        1.02 |

