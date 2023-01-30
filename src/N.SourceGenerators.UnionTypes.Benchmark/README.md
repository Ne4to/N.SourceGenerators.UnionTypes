## Ctor
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-FJCCER : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|           Method |     Mean |     Error |    StdDev | Ratio | RatioSD | Code Size |   Gen0 | Allocated | Alloc Ratio |
|----------------- |---------:|----------:|----------:|------:|--------:|----------:|-------:|----------:|------------:|
|            Class | 9.577 ns | 0.7403 ns | 0.0406 ns |  1.00 |    0.00 |      86 B | 0.0153 |      64 B |        1.00 |
|  ClassWithLayout | 9.186 ns | 4.2621 ns | 0.2336 ns |  0.96 |    0.03 |      93 B | 0.0134 |      56 B |        0.88 |
|   ClassGenerated | 9.326 ns | 4.9795 ns | 0.2729 ns |  0.97 |    0.03 |      86 B | 0.0153 |      64 B |        1.00 |
|           Struct | 5.650 ns | 1.1563 ns | 0.0634 ns |  0.59 |    0.01 |      59 B |      - |         - |        0.00 |
| StructWithLayout | 4.400 ns | 1.1613 ns | 0.0637 ns |  0.46 |    0.01 |      31 B |      - |         - |        0.00 |
|  StructGenerated | 4.498 ns | 0.6512 ns | 0.0357 ns |  0.47 |    0.00 |      31 B |      - |         - |        0.00 |

## Hash
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-RIQQUW : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                   Method |      Mean |     Error |    StdDev | Ratio | RatioSD | Code Size |   Gen0 | Allocated | Alloc Ratio |
|------------------------- |----------:|----------:|----------:|------:|--------:|----------:|-------:|----------:|------------:|
|                    Class | 27.823 ns | 15.085 ns | 0.8268 ns |  1.00 |    0.00 |      15 B |      - |         - |          NA |
|            ClassNoBoxing | 25.913 ns | 14.242 ns | 0.7807 ns |  0.93 |    0.05 |      15 B |      - |         - |          NA |
|          ClassWithLayout | 27.978 ns | 14.215 ns | 0.7792 ns |  1.01 |    0.04 |      15 B |      - |         - |          NA |
|           ClassGenerated | 29.559 ns | 40.078 ns | 2.1968 ns |  1.06 |    0.10 |      15 B |      - |         - |          NA |
|                   Struct | 10.400 ns |  5.393 ns | 0.2956 ns |  0.37 |    0.00 |     260 B | 0.0057 |      24 B |          NA |
|         StructWithLayout |  9.895 ns |  4.774 ns | 0.2617 ns |  0.36 |    0.00 |     236 B | 0.0057 |      24 B |          NA |
| StructWithLayoutNoBoxing |  3.529 ns | 13.204 ns | 0.7237 ns |  0.13 |    0.02 |     137 B |      - |         - |          NA |
|          StructGenerated |  2.372 ns |  2.020 ns | 0.1107 ns |  0.09 |    0.01 |     137 B |      - |         - |          NA |

## ReadValue
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-ICEOHC : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|           Method |      Mean |     Error |    StdDev | Ratio | RatioSD | Code Size | Allocated | Alloc Ratio |
|----------------- |----------:|----------:|----------:|------:|--------:|----------:|----------:|------------:|
|            Class | 1.5325 ns | 0.9072 ns | 0.0497 ns |  1.00 |    0.00 |      91 B |         - |          NA |
|  ClassWithLayout | 1.6101 ns | 1.4895 ns | 0.0816 ns |  1.05 |    0.03 |      92 B |         - |          NA |
|   ClassGenerated | 1.9662 ns | 3.0406 ns | 0.1667 ns |  1.28 |    0.08 |      91 B |         - |          NA |
|           Struct | 2.2471 ns | 5.4308 ns | 0.2977 ns |  1.47 |    0.18 |     132 B |         - |          NA |
| StructWithLayout | 0.1461 ns | 0.0257 ns | 0.0014 ns |  0.10 |    0.00 |      90 B |         - |          NA |
|  StructGenerated | 0.1754 ns | 0.3844 ns | 0.0211 ns |  0.11 |    0.02 |      90 B |         - |          NA |

## ToString
``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  Job-CXIHIO : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                   Method |     Mean |     Error |  StdDev | Ratio | RatioSD | Code Size |   Gen0 | Allocated | Alloc Ratio |
|------------------------- |---------:|----------:|--------:|------:|--------:|----------:|-------:|----------:|------------:|
|                    Class | 258.5 ns |  64.96 ns | 3.56 ns |  1.00 |    0.00 |      15 B | 0.1507 |     632 B |        1.00 |
|            ClassNoBoxing | 213.9 ns |  71.34 ns | 3.91 ns |  0.83 |    0.02 |      15 B | 0.1507 |     632 B |        1.00 |
|          ClassWithLayout | 213.2 ns |  51.93 ns | 2.85 ns |  0.82 |    0.01 |      15 B | 0.1509 |     632 B |        1.00 |
|           ClassGenerated | 132.2 ns |  62.16 ns | 3.41 ns |  0.51 |    0.01 |      15 B | 0.1128 |     472 B |        0.75 |
|                   Struct | 238.8 ns |  85.52 ns | 4.69 ns |  0.92 |    0.03 |     436 B | 0.1564 |     656 B |        1.04 |
|         StructWithLayout | 227.1 ns | 167.14 ns | 9.16 ns |  0.88 |    0.03 |     424 B | 0.1564 |     656 B |        1.04 |
| StructWithLayoutNoBoxing | 224.9 ns |  73.94 ns | 4.05 ns |  0.87 |    0.01 |   1,256 B | 0.1509 |     632 B |        1.00 |
|          StructGenerated | 136.3 ns | 102.42 ns | 5.61 ns |  0.53 |    0.02 |     156 B | 0.1147 |     480 B |        0.76 |

