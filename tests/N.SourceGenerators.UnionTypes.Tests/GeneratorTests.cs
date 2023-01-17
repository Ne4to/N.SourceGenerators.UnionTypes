namespace N.SourceGenerators.UnionTypes.Tests;

[UsesVerify]
public class GeneratorTests
{
    [Theory]
    [InlineData("partial class")]
    [InlineData("partial struct")]
    [InlineData("readonly partial struct")]
    public Task WithoutNamespace(string typeModifiers)
    {
        string source = $$"""           
            using System;
            using N.SourceGenerators.UnionTypes;
            
            public record Success;
            public record Error;
            
            [UnionType(typeof(Success))]
            [UnionType(typeof(Error))]
            public {{typeModifiers}} Result
            {
            }
            """;

        return TestHelper.Verify<UnionTypesGenerator>(source, typeModifiers.Replace(' ', '-'));
    }

    [Theory]
    [InlineData("partial class")]
    [InlineData("partial struct")]
    [InlineData("readonly partial struct")]
    public Task WithNamespace(string typeModifiers)
    {
        string source = $$"""            
            using System;
            using N.SourceGenerators.UnionTypes;
            
            namespace MyApp
            {
                public record Success;
                public record Error;
            
                [UnionType(typeof(Success))]
                [UnionType(typeof(Error))]
                public {{typeModifiers}} Result
                {
                }
            }
            """;

        return TestHelper.Verify<UnionTypesGenerator>(source, typeModifiers.Replace(' ', '-'));
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

    [Fact]
    public Task VariantTypeDuplicateReportsDiagnostics()
    {
        const string source = @"
using System;
using N.SourceGenerators.UnionTypes;

namespace MyApp
{
    public record Success;
    public record Error;

    [UnionType(typeof(Success))]
    [UnionType(typeof(Success))]
    public partial class Result
    {
    }
}
";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }

    [Fact]
    public Task VariantAliasDuplicateReportsDiagnostics()
    {
        const string source = """
using System;
using N.SourceGenerators.UnionTypes;

namespace MyApp
{
    public record Success;
    public record Error;

    [UnionType(typeof(Success), alias: "FriendlyName")]
    [UnionType(typeof(Error), alias: "FriendlyName")]
    public partial class Result
    {
    }
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }

    [Fact]
    public Task VariantOrderDuplicateReportsDiagnostics()
    {
        const string source = """
using System;
using N.SourceGenerators.UnionTypes;

namespace MyApp
{
    public record Success;
    public record Error;

    [UnionType(typeof(Success), order: 42)]
    [UnionType(typeof(Error), order: 42)]
    public partial class Result
    {
    }
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }

    [Fact]
    public Task InvalidTypeReportsAllDiagnostics()
    {
        const string source = """
using System;
using N.SourceGenerators.UnionTypes;

namespace MyApp
{
    public record Success;
    public record Error;

    [UnionType(typeof(Success), alias: "FriendlyName", order: 42)]
    [UnionType(typeof(Success), alias: "FriendlyName", order: 42)]
    public class Result
    {
    }
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }
}