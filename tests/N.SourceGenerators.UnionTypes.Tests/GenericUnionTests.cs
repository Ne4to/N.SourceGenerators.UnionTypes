namespace N.SourceGenerators.UnionTypes.Tests;

[UsesVerify]
public class GenericUnionTests
{
    [Fact]
    public Task OnlyGenericTypes()
    {
        const string source = """
using System;
using N.SourceGenerators.UnionTypes;

[GenericUnionType]
public partial class OperationDataResult<TResult, TError>
{
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }
    
    [Fact]
    public Task WithCustomUnionType()
    {
        const string source = """
using System;
using N.SourceGenerators.UnionTypes;

[UnionType(typeof(int))]
public partial class OperationDataResult<TResult, TError>
{
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }
}