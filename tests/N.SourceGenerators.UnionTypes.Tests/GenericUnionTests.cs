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

public partial class OperationDataResult<[GenericUnionType(Alias = "Result")] T, [GenericUnionType] TError>
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
public partial class OperationDataResult<[GenericUnionType] TResult, [GenericUnionType] TError>
{
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }
    
    [Fact]
    public Task WithAlias()
    {
        const string source = """
using System;
using N.SourceGenerators.UnionTypes;

[UnionType(typeof(string), "Status", AllowNull = true)]
public partial class GenericResultNullable<[GenericUnionType(AllowNull = true, Alias = "Value")] T>
{
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }
}