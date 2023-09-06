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
