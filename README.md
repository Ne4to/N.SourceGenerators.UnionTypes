# N.SourceGenerators.UnionTypes
Discriminated union type source generator

## Motivation
C# doesn't support discriminated unions yet. This source generator helps automate writing union types with set of helper methods.

## Getting Started
Add package reference to `N.SourceGenerators.UnionTypes`
```shell
dotnet add package N.SourceGenerators.UnionTypes
```
Create a partial class or struct that will be used as a union type
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
Checking value type
```csharp
public void ValueTypeProperty()
{
    FooResult foo = GetFoo();
    Type valueType = foo.ValueType; // returns typeof(NotFoundError)

    static FooResult GetFoo()
    {
        return new NotFoundError();
    }
}
```
TryGet method is used to check if union contains a specific type
```csharp
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
```
Alias for each variant is generated based on type name. Use alias parameter to override it.
```csharp
[UnionType(typeof(int))]
[UnionType(typeof(string))]
// default alias is 'ArrayOfTupleOfIntAndString' but it is overriden by alias parameter
[UnionType(typeof(Tuple<int,string>[]), alias: "Items")]
public partial class AliasResult
{
}
```
Match and MatchAsync methods are used to convert union type to another type. These methods force you to handle all possible variations.
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
```
Switch and SwitchAsync methods are used to execute some work based on inner type
```csharp
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
```