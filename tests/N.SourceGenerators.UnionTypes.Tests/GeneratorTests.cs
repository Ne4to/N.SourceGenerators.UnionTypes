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
            using System.Collections.Generic;
            using N.SourceGenerators.UnionTypes;
            
            namespace MyApp
            {
                public record Success;
                public record Error;
            
                [UnionType(typeof(Success))]
                [UnionType(typeof(Error))]
                [UnionType(typeof(IReadOnlyList<int>))]
                [UnionType(typeof(string[]))]
                [UnionType(typeof(Tuple<int,string>))]
                public {{typeModifiers}} Result
                {
                }
            }
            """;

        return TestHelper.Verify<UnionTypesGenerator>(source, typeModifiers.Replace(' ', '-'));
    }
    
    [Fact]
    public Task ExplicitStructLayout()
    {
        const string source = @"
using System;
using N.SourceGenerators.UnionTypes;

namespace MyApp
{
    public record struct SuccessStruct(int Value);
    public record struct ValidationErrorStruct(int ErrorCode);
    public record struct NotFoundErrorStruct;

    [UnionType(typeof(SuccessStruct))]
    [UnionType(typeof(ValidationErrorStruct))]
    [UnionType(typeof(NotFoundErrorStruct))]
    public partial struct Result
    {
    }
}
";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }   
    
    [Fact]
    public Task DifferentNamespaces()
    {
        string source = """
            using System;
            using System.Collections.Generic;
            using N.SourceGenerators.UnionTypes;
            
            namespace MyApp.Models.S.Child
            {
                public record Success;
            }

            namespace MyApp.Models.E.Child
            {
                public record Error;
            }

            namespace MyApp.Domain.Child
            {
                [UnionType(typeof(MyApp.Models.S.Child.Success))]
                [UnionType(typeof(MyApp.Models.E.Child.Error))]
                public partial class Result
                {
                }
            }
            """;

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