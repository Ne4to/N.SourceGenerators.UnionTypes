namespace N.SourceGenerators.UnionTypes.Tests;

[UsesVerify]
public class NullableValueTests
{
    [Fact]
    public Task UseAllowNullAttribute()
    {
        const string source =
"""
using System;
using System.Collections.Generic;
using N.SourceGenerators.UnionTypes;

[UnionType(typeof(int?), AllowNull = true)]
[UnionType(typeof(string))]
public partial class Result
{
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }
}