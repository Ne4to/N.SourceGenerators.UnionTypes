namespace N.SourceGenerators.UnionTypes.Benchmark.Models;

[UnionType(typeof(SuccessStruct))]
[UnionType(typeof(ValidationErrorStruct))]
[UnionType(typeof(NotFoundErrorStruct))]
public partial struct StructGenerated
{
}