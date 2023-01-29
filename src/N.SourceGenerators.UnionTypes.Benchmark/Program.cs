using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

[MemoryDiagnoser]
public class CtorBenchmark
{
    [Benchmark]
    public FooStructResult Struct() =>
        new FooStructResult(new ValidationErrorStruct(42));

    [Benchmark]
    public FooStructResultWithStructLayout StructLayout() =>
        new FooStructResultWithStructLayout(new ValidationErrorStruct(42));

    [Benchmark(Baseline = true)]
    public FooResult Class() =>
        new FooResult(new ValidationError("something went wrong"));

    [Benchmark]
    public FooResultLayout ClassLayout() =>
        new FooResultLayout(new ValidationError("something went wrong"));
}

[MemoryDiagnoser]
public class ReadValueBenchmark
{
    private readonly FooStructResult _struct;
    private readonly FooResult _class;
    private readonly FooResultLayout _classLayout;
    private readonly FooStructResultWithStructLayout _structLayout;

    public ReadValueBenchmark()
    {
        _struct = new FooStructResult(new ValidationErrorStruct(42));
        _class = new FooResult(new ValidationError("something went wrong"));
        _classLayout = new FooResultLayout(new ValidationError("something went wrong"));
        _structLayout = new FooStructResultWithStructLayout(new ValidationErrorStruct(42));
    }

    [Benchmark]
    public ValidationErrorStruct Struct() =>
        _struct.AsValidationErrorStruct;

    [Benchmark]
    public ValidationErrorStruct WithLayout() =>
        _structLayout.AsValidationErrorStruct;

    [Benchmark(Baseline = true)]
    public ValidationError Class() =>
        _class.AsValidationError;

    [Benchmark]
    public ValidationError ClassLayout() =>
        _classLayout.AsValidationError;
}

public record struct SuccessStruct(int Value);

public record struct ValidationErrorStruct(int ErrorCode);

public record struct NotFoundErrorStruct;

public record Success(int Value);

public record ValidationError(string Message);

public record NotFoundError;