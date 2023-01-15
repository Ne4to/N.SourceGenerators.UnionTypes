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

All examples can be found in [examples project](./examples/N.SourceGenerators.UnionTypes.Examples/Program.cs)

Implicit conversion
```csharp
public FooResult ImplicitReturn()
{
    // you can return any union type variant without creating FooResult
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