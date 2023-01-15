# N.SourceGenerators.UnionTypes
Discriminated union type source generator

## Motivation
C# doesn't support discriminated unions yet. This source generator helps automate writing union types.

## Using
Add package reference to `N.SourceGenerators.UnionTypes`
```shell
dotnet add package N.SourceGenerators.UnionTypes
```
Create a partial class that will be used as a union type
```csharp
public partial class FooResult
{
}
```
Add types you want to use in a discriminated union
```csharp
public record Success(int Value);
public record ValidationError(string Message);
public record NotFoundError;

public partial class FooResult
{
}
```
Add `N.SourceGenerators.UnionTypes.UnionTypeAttribute` to a union type.
```csharp
using N.SourceGenerators.UnionTypes;

public record Success(int Value);
public record ValidationError(string Message);
public record NotFoundError;

[UnionType(typeof(Success))]
[UnionType(typeof(ValidationError))]
[UnionType(typeof(NotFoundError))]
public partial class FooResult
{
}
```

## Examples

All examples can be found in [examples project](https://github.com/Ne4to/N.SourceGenerators.UnionTypes/blob/main/examples/N.SourceGenerators.UnionTypes.Examples/Program.cs)

Implicit conversion
```csharp
public FooResult ImplicitReturn()
{
    // you can return any union type variation without creating FooResult
    return new NotFoundError();
}
```
Explicit conversion
```csharp
public ValidationError ExplicitCast(FooResult result)
{
    return (ValidationError)result;
}
```
Match and MatchAsync methods force you to handle all possible variations
```csharp
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
            await SomeWork(ct);
            return new OkResult();
        }, static async (validationError, ct) =>
        {
            await SomeWork(ct);
            return new BadRequestResult();
        }, static async (notFoundError, ct) =>
        {
            await SomeWork(ct);
            return new NotFoundResult();
        }, cancellationToken);

    static Task SomeWork(CancellationToken ct)
    {
        return Task.Delay(100, ct);
    }
}
```