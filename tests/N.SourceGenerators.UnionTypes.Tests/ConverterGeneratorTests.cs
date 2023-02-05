namespace N.SourceGenerators.UnionTypes.Tests;

[UsesVerify]
public class ConverterGeneratorTests
{
    [Fact]
    public Task FromConverter()
    {
        const string source = """
using System;
using N.SourceGenerators.UnionTypes;

namespace MyApp
{
    public record Success(int Value);
    public record ValidationError(string Message);
    public record NotFoundError;

    [UnionType(typeof(Success))]
    [UnionType(typeof(NotFoundError))]
    public partial class DataAccessResult
    {
    }

    [UnionType(typeof(Success))]
    [UnionType(typeof(NotFoundError))]
    [UnionType(typeof(ValidationError))]
    [UnionConverterFrom(typeof(DataAccessResult))]
    public partial class BusinessLogicResult
    {
    }
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }

    [Fact]
    public Task FromConverterReportsDiagnostic()
    {
        const string source = """
using System;
using N.SourceGenerators.UnionTypes;

namespace MyApp
{
    public record Success(int Value);
    public record ValidationError(string Message);
    public record NotFoundError;

    [UnionType(typeof(Success))]
    [UnionType(typeof(NotFoundError))]
    public partial class DataAccessResult
    {
    }

    [UnionType(typeof(Success))]
    [UnionType(typeof(ValidationError))]
    [UnionConverterFrom(typeof(DataAccessResult))]
    public partial class BusinessLogicResult
    {
    }
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }

    [Fact]
    public Task ToConverter()
    {
        const string source = """
using System;
using N.SourceGenerators.UnionTypes;

namespace MyApp
{
    public record Success(int Value);
    public record ValidationError(string Message);
    public record NotFoundError;

    [UnionType(typeof(Success))]
    [UnionType(typeof(NotFoundError))]
    [UnionConverterTo(typeof(BusinessLogicResult))]
    public partial class DataAccessResult
    {
    }

    [UnionType(typeof(Success))]
    [UnionType(typeof(NotFoundError))]
    [UnionType(typeof(ValidationError))]
    public partial class BusinessLogicResult
    {
    }
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }

    [Fact]
    public Task ToConverterReportsDiagnostic()
    {
        const string source = """
using System;
using N.SourceGenerators.UnionTypes;

namespace MyApp
{
    public record Success(int Value);
    public record ValidationError(string Message);
    public record NotFoundError;

    [UnionType(typeof(Success))]
    [UnionType(typeof(NotFoundError))]
    [UnionConverterTo(typeof(BusinessLogicResult))]
    public partial class DataAccessResult
    {
    }

    [UnionType(typeof(NotFoundError))]
    [UnionType(typeof(ValidationError))]
    public partial class BusinessLogicResult
    {
    }
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }

    [Fact]
    public Task ExternalConverter()
    {
        const string source = """
using System;
using N.SourceGenerators.UnionTypes;

namespace MyApp
{
    public record Success(int Value);
    public record ValidationError(string Message);
    public record NotFoundError;

    [UnionType(typeof(Success))]
    [UnionType(typeof(NotFoundError))]
    public partial class DataAccessResult
    {
    }

    [UnionType(typeof(Success))]
    [UnionType(typeof(NotFoundError))]
    [UnionType(typeof(ValidationError))]
    public partial class BusinessLogicResult
    {
    }

    [UnionConverter(typeof(DataAccessResult), typeof(BusinessLogicResult))]
    public partial class Converters
    {
    }
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }
    
    [Fact]
    public Task ExternalConverterReportsDiagnostic()
    {
        const string source = """
using System;
using N.SourceGenerators.UnionTypes;

namespace MyApp
{
    public record Success(int Value);
    public record ValidationError(string Message);
    public record NotFoundError;

    [UnionType(typeof(Success))]
    [UnionType(typeof(NotFoundError))]
    public partial class DataAccessResult
    {
    }

    [UnionType(typeof(Success))]
    [UnionType(typeof(ValidationError))]
    public partial class BusinessLogicResult
    {
    }

    [UnionConverter(typeof(DataAccessResult), typeof(BusinessLogicResult))]
    public partial class Converters
    {
    }
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }
    
    [Fact]
    public Task StaticExternalConverter()
    {
        const string source = """
using System;
using N.SourceGenerators.UnionTypes;

namespace MyApp
{
    public record Success(int Value);
    public record ValidationError(string Message);
    public record NotFoundError;

    [UnionType(typeof(Success))]
    [UnionType(typeof(NotFoundError))]
    public partial class DataAccessResult
    {
    }

    [UnionType(typeof(Success))]
    [UnionType(typeof(NotFoundError))]
    [UnionType(typeof(ValidationError))]
    public partial class BusinessLogicResult
    {
    }

    [UnionConverter(typeof(DataAccessResult), typeof(BusinessLogicResult))]
    public static partial class Converters
    {
    }
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }
    
    [Fact]
    public Task StaticExternalConverterReportsDiagnostic()
    {
        const string source = """
using System;
using N.SourceGenerators.UnionTypes;

namespace MyApp
{
    public record Success(int Value);
    public record ValidationError(string Message);
    public record NotFoundError;

    [UnionType(typeof(Success))]
    [UnionType(typeof(NotFoundError))]
    public partial class DataAccessResult
    {
    }

    [UnionType(typeof(Success))]
    [UnionType(typeof(ValidationError))]
    public partial class BusinessLogicResult
    {
    }

    [UnionConverter(typeof(DataAccessResult), typeof(BusinessLogicResult))]
    public static partial class Converters
    {
    }
}
""";

        return TestHelper.Verify<UnionTypesGenerator>(source);
    }
}