## Ctor
source `CtorBenchmark`

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
[Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
Job-BJGCRY : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3
LaunchCount=1  WarmupCount=3

|           Method |     Mean |     Error |    StdDev | Ratio | Allocated | Alloc Ratio |
|----------------- |---------:|----------:|----------:|------:|----------:|------------:|
|          Default | 6.617 ns | 0.8117 ns | 0.0445 ns |  1.00 |         - |          NA |
| WithStructLayout | 4.641 ns | 1.1639 ns | 0.0638 ns |  0.70 |         - |          NA |

## Read value
source `ReadValueBenchmark`

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.102
[Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
Job-PNQVZK : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3
LaunchCount=1  WarmupCount=3

|           Method |      Mean |     Error |    StdDev | Ratio | Allocated | Alloc Ratio |
|----------------- |----------:|----------:|----------:|------:|----------:|------------:|
|          Default | 1.7995 ns | 0.5954 ns | 0.0326 ns |  1.00 |         - |          NA |
| WithStructLayout | 0.0292 ns | 0.1820 ns | 0.0100 ns |  0.02 |         - |          NA |