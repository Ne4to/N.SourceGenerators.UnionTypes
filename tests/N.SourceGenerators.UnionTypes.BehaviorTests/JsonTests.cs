using System.Text.Json;
using System.Text.Json.Serialization;

namespace N.SourceGenerators.UnionTypes.BehaviorTests;


[JsonPolymorphic]
[JsonDerivedType(typeof(JsonTestsFirstDelivered), "FirstDelivered")]
[JsonDerivedType(typeof(JsonTestsSecondDelivered), "SecondDelivered")]
public abstract record JsonTestsInnerType;

public record JsonTestsFirstDelivered(int Value) : JsonTestsInnerType();
public record JsonTestsSecondDelivered(string Value) : JsonTestsInnerType();

public record JsonTestsFooJ(int Id, string Title, InnerType Inner);

public enum JsonTestsBarStatus
{
    Active,
    Rejected
}

public record JsonTestsBarJ(int Id, int Cost, JsonTestsBarStatus Status);

[UnionType(typeof(JsonTestsFooJ), TypeDiscriminator = "Foo")]
[UnionType(typeof(JsonTestsBarJ), TypeDiscriminator = "Bar")]
[JsonPolymorphicUnion]
public partial class JsonTestsUnion
{
}

public class JsonTests
{
    [Fact]
    public void SerializeBar()
    {
        JsonTestsUnion item = new JsonTestsBarJ(3, 42, JsonTestsBarStatus.Rejected);
    
        JsonSerializerOptions serializerOptions = new() { Converters = { new JsonStringEnumConverter() }, };
    
        var json = JsonSerializer.Serialize(item, serializerOptions);
        Assert.Equal("""{"$type":"Bar","Id":3,"Cost":42,"Status":"Rejected"}""", json);
    }
}