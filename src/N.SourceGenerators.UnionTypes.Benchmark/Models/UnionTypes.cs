namespace N.SourceGenerators.UnionTypes.Benchmark.Models;

public record struct SuccessStruct(int Value);

public record struct ValidationErrorStruct(int ErrorCode);

public record struct NotFoundErrorStruct;

public record Success(int Value);

public record ValidationError(int ErrorCode);

public record NotFoundError;