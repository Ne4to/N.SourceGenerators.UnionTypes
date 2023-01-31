namespace N.SourceGenerators.UnionTypes.Benchmark.Models;

[UnionType(typeof(Success))]
[UnionType(typeof(ValidationError))]
[UnionType(typeof(NotFoundError))]
public partial class Class
{
}

[UnionType(typeof(Success))]
[UnionType(typeof(ValidationError))]
[UnionType(typeof(NotFoundError))]
public sealed partial class ClassSealed
{
}

[UnionType(typeof(Success))]
[UnionType(typeof(ValidationError))]
[UnionType(typeof(NotFoundError))]
public partial struct Struct
{
}

[UnionType(typeof(Success))]
[UnionType(typeof(ValidationError))]
[UnionType(typeof(NotFoundError))]
public readonly partial struct StructReadonly
{
}

[UnionType(typeof(SuccessStruct))]
[UnionType(typeof(ValidationErrorStruct))]
[UnionType(typeof(NotFoundErrorStruct))]
public partial struct StructExplicitLayout
{
}

[UnionType(typeof(SuccessStruct))]
[UnionType(typeof(ValidationErrorStruct))]
[UnionType(typeof(NotFoundErrorStruct))]
public readonly partial struct StructReadonlyExplicitLayout
{
}