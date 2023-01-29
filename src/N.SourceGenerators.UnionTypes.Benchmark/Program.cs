using System.Runtime.CompilerServices;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

[MemoryDiagnoser]
public class CtorBenchmark
{
    [Benchmark(Baseline = true)]
    public FooStructResult Default() =>
        new FooStructResult(new ValidationErrorStruct(42));

    [Benchmark]
    public FooStructResultWithStructLayout WithStructLayout() =>
        new FooStructResultWithStructLayout(new ValidationErrorStruct(42));
}

[MemoryDiagnoser]
public class ReadValueBenchmark
{
    private readonly FooStructResult _default;
    private readonly FooStructResultWithStructLayout _withStructLayout;

    public ReadValueBenchmark()
    {
        _default = new FooStructResult(new ValidationErrorStruct(42));
        _withStructLayout = new FooStructResultWithStructLayout(new ValidationErrorStruct(42));
    }

    [Benchmark(Baseline = true)]
    public ValidationErrorStruct Default() =>
        _default.AsValidationErrorStruct;

    [Benchmark]
    public ValidationErrorStruct WithStructLayout() =>
        _withStructLayout.AsValidationErrorStruct;
}

public record struct SuccessStruct(int Value);

public record struct ValidationErrorStruct(int ErrorCode);

public record struct NotFoundErrorStruct;