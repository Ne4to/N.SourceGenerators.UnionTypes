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
}
