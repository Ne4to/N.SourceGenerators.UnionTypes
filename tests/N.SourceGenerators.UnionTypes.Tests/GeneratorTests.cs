namespace N.SourceGenerators.UnionTypes.Tests;

[UsesVerify]
public class GeneratorTests
{
    [Fact]
    public Task WithoutNamespace()
    {
        const string source = @"
using System;
using N.SourceGenerators.UnionTypes;

public record Success;
public record Error;

[UnionType(typeof(Success))]
[UnionType(typeof(Error))]
public partial class Result
{
}
";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }
    
    [Fact]
    public Task WithNamespace()
    {
        const string source = @"
using System;
using N.SourceGenerators.UnionTypes;

namespace MyApp
{
    public record Success;
    public record Error;

    [UnionType(typeof(Success))]
    [UnionType(typeof(Error))]
    public partial class Result
    {
    }
}
";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }
    
    [Fact]
    public Task NotPartialTypeReportsDiagnostics()
    {
        const string source = @"
using System;
using N.SourceGenerators.UnionTypes;

namespace MyApp
{
    public record Success;
    public record Error;

    [UnionType(typeof(Success))]
    [UnionType(typeof(Error))]
    public class Result
    {
    }
}
";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }
}