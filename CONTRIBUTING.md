# Contributing guide

## Benchmark
For performance improvements update [Benchmark](./src/N.SourceGenerators.UnionTypes.Benchmark/) project first.
- Copy existing generated code from Solution Explorer: Dependencies -> .NET 7.0 -> Source Generators -> UnionTypesGenerator
- Rename generated class or struct and modify it
- Add new class or struct to existing benchmarks
- Run `run.ps1 -Mode All` to run benchmarks and update README.md