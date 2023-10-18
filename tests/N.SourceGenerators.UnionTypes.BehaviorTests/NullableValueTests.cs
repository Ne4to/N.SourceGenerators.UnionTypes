using System.Runtime.CompilerServices;

namespace N.SourceGenerators.UnionTypes.BehaviorTests;

[UnionType(typeof(int?))]
[UnionType(typeof(string))]
public partial class ResultNotNullable
{
}

[UnionType(typeof(int?), AllowNull = true)]
[UnionType(typeof(string), AllowNull = true)]
public partial class ResultNullable
{
}

[UnionType(typeof(string), "Status", AllowNull = true)]
public partial class GenericResultNullable<[GenericUnionType(AllowNull = true, Alias = "Value")] T>
{
}

public class NullableValueTests
{
    [Fact]
    public void NullValueNotAllowed()
    {
        Assert.Throws<ArgumentNullException>(() => new ResultNotNullable((int?)null));
        Assert.Throws<ArgumentNullException>(() => new ResultNotNullable((string)null));
    }

    [Fact]
    public void NullValueAllowed()
    {
        var intResult = new ResultNullable((int?)null);
        Assert.True(intResult.IsNullableOfInt32);
        Assert.False(intResult.IsString);

        var stringResult = new ResultNullable((string)null);
        Assert.True(stringResult.IsString);
        Assert.False(stringResult.IsNullableOfInt32);
    }
    
    [Fact]
    public void GenericNullValueAllowed()
    {
        var intResult = new GenericResultNullable<int?>((int?)null);
        Assert.True(intResult.IsValue);
        Assert.False(intResult.IsStatus);

        var stringResult = new GenericResultNullable<int?>((string)null);
        Assert.True(stringResult.IsStatus);
        Assert.False(stringResult.IsValue);
    }
}
