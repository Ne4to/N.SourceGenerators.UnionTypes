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

    public IActionResult MatchMethod(FooResult result)
    {
        return result.Match<IActionResult>(
            success => new OkResult(),
            validationError => new BadRequestResult(),
            notFoundError => new NotFoundResult()
        );
    }
}