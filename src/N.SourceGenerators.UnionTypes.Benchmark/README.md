## Ctor
source `CtorBenchmark`

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)  
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores  
.NET SDK=7.0.102  
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2  
  Job-HBDHOK : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2  

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

|       Method |     Mean |    Error |    StdDev | Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|------------- |---------:|---------:|----------:|------:|--------:|-------:|----------:|------------:|
|       Struct | 5.209 ns | 1.376 ns | 0.0754 ns |  0.73 |    0.02 |      - |         - |        0.00 |
| StructLayout | 4.194 ns | 1.251 ns | 0.0686 ns |  0.59 |    0.01 |      - |         - |        0.00 |
|        Class | 7.154 ns | 2.432 ns | 0.1333 ns |  1.00 |    0.00 | 0.0153 |      64 B |        1.00 |
|  ClassLayout | 7.665 ns | 2.384 ns | 0.1307 ns |  1.07 |    0.04 | 0.0134 |      56 B |        0.88 |

## Read value
source `ReadValueBenchmark`

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)  
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores  
.NET SDK=7.0.102  
[Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2  
Job-NWHKYF : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2  

Runtime=.NET 7.0  Toolchain=net7.0  IterationCount=3  
LaunchCount=1  WarmupCount=3  

|      Method |      Mean |     Error |    StdDev |    Median | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------ |----------:|----------:|----------:|----------:|------:|--------:|----------:|------------:|
|      Struct | 1.4743 ns | 0.4473 ns | 0.0245 ns | 1.4631 ns |  0.99 |    0.02 |         - |          NA |
|  WithLayout | 0.0368 ns | 0.3963 ns | 0.0217 ns | 0.0271 ns |  0.02 |    0.01 |         - |          NA |
|       Class | 1.4950 ns | 1.0329 ns | 0.0566 ns | 1.4876 ns |  1.00 |    0.00 |         - |          NA |
| ClassLayout | 1.4178 ns | 1.1878 ns | 0.0651 ns | 1.4425 ns |  0.95 |    0.05 |         - |          NA |