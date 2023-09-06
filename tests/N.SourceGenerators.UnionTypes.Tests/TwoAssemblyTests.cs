namespace N.SourceGenerators.UnionTypes.Tests;

[UsesVerify]
public class TwoAssemblyTests
{
    [Fact]
    public async Task UnionConverterFromInOtherAssembly()
    {
        const string dataAccessSource =
            """
            using N.SourceGenerators.UnionTypes;
            
            namespace DataAccess;
            
            
            // TODO - uncomment to negative case
            // -->
            public record DataAccessModel1;
            
            public record DataAccessModel2(string Message);
            
            [UnionType(typeof(DataAccessModel1))]
            [UnionType(typeof(DataAccessModel2))]
            public partial class DataAccessModel
            {
                
            }
            """;

        const string businessLogicSource =
            """
            using DataAccess;
            using N.SourceGenerators.UnionTypes;
            
            namespace BusinessLogic;
            
            public record BusinessLogicModel1;
            
            [UnionType(typeof(DataAccessModel1))]
            [UnionType(typeof(DataAccessModel2))]
            [UnionType(typeof(BusinessLogicModel1))]
            [UnionConverterFrom(typeof(DataAccessModel))]
            // ReSharper disable once ClassNeverInstantiated.Global
            public partial class BusinessLogicModel
            {
            }
            
            public class Foo
            {
                public BusinessLogicModel Get(DataAccessModel? input)
                {
                    if (input == null)
                    {
                        return new BusinessLogicModel1();
                    }
                
                    return input;
                }
            }
            """;

        await TestHelper.Verify<UnionTypesGenerator>(new[] { dataAccessSource, businessLogicSource });
    }
}