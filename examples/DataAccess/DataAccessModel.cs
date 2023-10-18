using N.SourceGenerators.UnionTypes;

namespace DataAccess;

public record DataAccessModel1;

public record DataAccessModel2(string Message);

[UnionType(typeof(DataAccessModel1))]
[UnionType(typeof(DataAccessModel2))]
public partial class DataAccessModel
{
    
}

public record DataAccessModel3();