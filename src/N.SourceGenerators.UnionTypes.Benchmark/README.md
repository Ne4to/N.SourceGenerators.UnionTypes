## Ctor
``` ini

BenchmarkDotNet=v0.13.4, OS=macOS 14.1.1 (23B81) [Darwin 23.1.0]
Apple M2 Max, 1 CPU, 12 logical and 12 physical cores
.NET SDK=8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
  Job-GPAWCK : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD

Runtime=.NET 8.0  Toolchain=net8.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                       Method |      Mean |     Error |    StdDev | Ratio |   Gen0 | Allocated | Alloc Ratio |
|----------------------------- |----------:|----------:|----------:|------:|-------:|----------:|------------:|
|                        Class | 6.2787 ns | 1.6709 ns | 0.0916 ns | 1.000 | 0.0086 |      72 B |        1.00 |
|                  ClassSealed | 6.1926 ns | 0.3691 ns | 0.0202 ns | 0.986 | 0.0086 |      72 B |        1.00 |
|                       Struct | 2.5874 ns | 0.1017 ns | 0.0056 ns | 0.412 | 0.0029 |      24 B |        0.33 |
|               StructReadonly | 2.6129 ns | 0.1318 ns | 0.0072 ns | 0.416 | 0.0029 |      24 B |        0.33 |
|         StructExplicitLayout | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.000 |      - |         - |        0.00 |
| StructReadonlyExplicitLayout | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.000 |      - |         - |        0.00 |

## GetHashCode
``` ini

BenchmarkDotNet=v0.13.4, OS=macOS 14.1.1 (23B81) [Darwin 23.1.0]
Apple M2 Max, 1 CPU, 12 logical and 12 physical cores
.NET SDK=8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
  Job-DWCWZM : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD

Runtime=.NET 8.0  Toolchain=net8.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                       Method |      Mean |     Error |    StdDev | Ratio | Allocated | Alloc Ratio |
|----------------------------- |----------:|----------:|----------:|------:|----------:|------------:|
|                        Class | 2.3616 ns | 0.0667 ns | 0.0037 ns |  1.00 |         - |          NA |
|                  ClassSealed | 2.2898 ns | 0.1473 ns | 0.0081 ns |  0.97 |         - |          NA |
|                       Struct | 1.7766 ns | 0.0301 ns | 0.0016 ns |  0.75 |         - |          NA |
|               StructReadonly | 1.7756 ns | 0.1693 ns | 0.0093 ns |  0.75 |         - |          NA |
|         StructExplicitLayout | 0.6548 ns | 0.0899 ns | 0.0049 ns |  0.28 |         - |          NA |
| StructReadonlyExplicitLayout | 0.6548 ns | 0.0919 ns | 0.0050 ns |  0.28 |         - |          NA |

## ReadValue
``` ini

BenchmarkDotNet=v0.13.4, OS=macOS 14.1.1 (23B81) [Darwin 23.1.0]
Apple M2 Max, 1 CPU, 12 logical and 12 physical cores
.NET SDK=8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
  Job-WVHIPW : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD

Runtime=.NET 8.0  Toolchain=net8.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                       Method |      Mean |     Error |    StdDev | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------------------------- |----------:|----------:|----------:|------:|--------:|----------:|------------:|
|                        Class | 0.2982 ns | 0.1581 ns | 0.0087 ns |  1.00 |    0.00 |         - |          NA |
|                  ClassSealed | 0.2935 ns | 0.0739 ns | 0.0040 ns |  0.98 |    0.02 |         - |          NA |
|                       Struct | 0.3001 ns | 0.0391 ns | 0.0021 ns |  1.01 |    0.02 |         - |          NA |
|               StructReadonly | 0.2973 ns | 0.0775 ns | 0.0042 ns |  1.00 |    0.02 |         - |          NA |
|         StructExplicitLayout | 0.2729 ns | 0.0535 ns | 0.0029 ns |  0.92 |    0.03 |         - |          NA |
| StructReadonlyExplicitLayout | 0.2802 ns | 0.0359 ns | 0.0020 ns |  0.94 |    0.03 |         - |          NA |

## ToString
``` ini

BenchmarkDotNet=v0.13.4, OS=macOS 14.1.1 (23B81) [Darwin 23.1.0]
Apple M2 Max, 1 CPU, 12 logical and 12 physical cores
.NET SDK=8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
  Job-PQJRUG : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD

Runtime=.NET 8.0  Toolchain=net8.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

```
|                       Method |     Mean |    Error |   StdDev | Ratio |   Gen0 | Allocated | Alloc Ratio |
|----------------------------- |---------:|---------:|---------:|------:|-------:|----------:|------------:|
|                        Class | 57.24 ns | 2.845 ns | 0.156 ns |  1.00 | 0.0526 |     440 B |        1.00 |
|                  ClassSealed | 57.20 ns | 0.197 ns | 0.011 ns |  1.00 | 0.0526 |     440 B |        1.00 |
|                       Struct | 57.28 ns | 3.862 ns | 0.212 ns |  1.00 | 0.0526 |     440 B |        1.00 |
|               StructReadonly | 57.42 ns | 2.337 ns | 0.128 ns |  1.00 | 0.0526 |     440 B |        1.00 |
|         StructExplicitLayout | 60.19 ns | 1.652 ns | 0.091 ns |  1.05 | 0.0535 |     448 B |        1.02 |
| StructReadonlyExplicitLayout | 58.10 ns | 2.044 ns | 0.112 ns |  1.01 | 0.0535 |     448 B |        1.02 |

