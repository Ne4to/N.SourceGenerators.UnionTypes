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
}