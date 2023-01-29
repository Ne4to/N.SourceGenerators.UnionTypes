using Microsoft.AspNetCore.Mvc;

using N.SourceGenerators.UnionTypes;

Console.WriteLine("Hello, union types generator!");

public record Success(int Value);

public record ValidationError(string Message);

public record NotFoundError;

[UnionType(typeof(Success))]
[UnionType(typeof(ValidationError))]
[UnionType(typeof(NotFoundError))]
public partial class FooResult
{
}

[UnionType(typeof(Success))]
[UnionType(typeof(ValidationError))]
[UnionType(typeof(NotFoundError))]
public partial struct FooStructResult
{
}

[UnionType(typeof(Success))]
[UnionType(typeof(ValidationError))]
[UnionType(typeof(NotFoundError))]
public readonly partial struct FooStructReadonlyResult
{
}

public record struct SuccessStruct(int Value);
public record struct ValidationErrorStruct(int ErrorCode);
public record struct NotFoundErrorStruct;

[UnionType(typeof(SuccessStruct))]
[UnionType(typeof(ValidationErrorStruct))]
[UnionType(typeof(NotFoundErrorStruct))]
public partial struct ResultStructLayout
{
}

class Bar
{
    public FooResult ImplicitReturn()
    {
        // you can return any union type variant without creating FooResult
        return new NotFoundError();
    }

    public ValidationError ExplicitCast(FooResult result)
    {
        return (ValidationError)result;
    }
    
    public void ValueTypeProperty()
    {
        FooResult foo = GetFoo();
        Type valueType = foo.ValueType; // returns typeof(NotFoundError)

        static FooResult GetFoo()
        {
            return new NotFoundError();
        }
    }
    
    public void TryGetValue()
    {
        FooResult foo = GetFoo();
        if (foo.TryGetNotFoundError(out var notFoundError))
        {
            // make something with notFoundError
        }

        static FooResult GetFoo()
        {
            return new NotFoundError();
        }
    }

    public IActionResult MatchMethod(FooResult result)
    {
        return result.Match<IActionResult>(
            success => new OkResult(),
            validationError => new BadRequestResult(),
            notFoundError => new NotFoundResult()
        );
    }

    public async Task<IActionResult> MatchAsyncMethod(FooResult result, CancellationToken cancellationToken)
    {
        return await result.MatchAsync<IActionResult>(
            static async (success, ct) =>
            {
                await SomeWork(success, ct);
                return new OkResult();
            }, static async (validationError, ct) =>
            {
                await SomeWork(validationError, ct);
                return new BadRequestResult();
            }, static async (notFoundError, ct) =>
            {
                await SomeWork(notFoundError, ct);
                return new NotFoundResult();
            }, cancellationToken);

        static Task SomeWork<T>(T value, CancellationToken ct)
        {
            return Task.Delay(100, ct);
        }
    }

    public void SwitchMethod(FooResult result)
    {
        result.Switch(
            success => SomeWork(success),
            validationError => SomeWork(validationError),
            notFoundError => SomeWork(notFoundError)
        );

        static void SomeWork<T>(T value)
        {
            throw new NotImplementedException();
        }
    }

    public async Task SwitchAsyncMethod(FooResult result, CancellationToken cancellationToken)
    {
        await result.SwitchAsync(
            static async (success, ct) =>
            {
                await SomeWork(success, ct);
            }, static async (validationError, ct) =>
            {
                await SomeWork(validationError, ct);
            }, static async (notFoundError, ct) =>
            {
                await SomeWork(notFoundError, ct);
            }, cancellationToken);

        static Task SomeWork<T>(T value, CancellationToken ct)
        {
            return Task.Delay(100, ct);
        }
    }
}