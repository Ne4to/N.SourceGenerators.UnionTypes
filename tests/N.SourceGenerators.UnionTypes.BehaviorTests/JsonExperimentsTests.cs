using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace N.SourceGenerators.UnionTypes.BehaviorTests;

[JsonPolymorphic]
[JsonDerivedType(typeof(FirstDelivered), "FirstDelivered")]
[JsonDerivedType(typeof(SecondDelivered), "SecondDelivered")]
public abstract record InnerType;

public record FirstDelivered(int Value) : InnerType();
public record SecondDelivered(string Value) : InnerType();

public record FooJ(int Id, string Title, InnerType Inner);

public enum BarStatus
{
    Active,
    Rejected
}

public record BarJ(int Id, int Cost, BarStatus Status);

[UnionType(typeof(FooJ))]
[UnionType(typeof(BarJ))]
[JsonUnionJsonConverter]
public partial class JsonUnion
{
}

public class MyNewAttribute : Attribute
{
    private object? _typeDiscriminator;
    public Type Type { get; }
    public string? Alias { get; }
    public int Order { get; }
    public bool AllowNull { get; set; }
    
    public MyNewAttribute(Type type, string? alias = null, [CallerLineNumber] int order = 0)
    {
        Type = type;
        Alias = alias;
        Order = order;
    }
    
    // public MyNewAttribute(Type type, string? alias = null, [CallerLineNumber] int order = 0)
    // {
    //     Type = type;
    //     Alias = alias;
    //     Order = order;
    // }

    public object? TypeDiscriminator { get; set; }
    // {
    //     get => _typeDiscriminator;
    //     set
    //     {
    //         if (value is not string && value is not int)
    //         {
    //             throw new InvalidOperationException("Only string and int are allowed");
    //         }
    //         
    //         _typeDiscriminator = value;
    //     }
    // }
}

// TODO validate TypeDiscriminator type = string | int
[MyNew(typeof(Guid), TypeDiscriminator = 1f)]
internal class JsonUnionJsonConverterAttribute : JsonConverterAttribute
{
    public override JsonConverter? CreateConverter(Type typeToConvert)
    {
        return new JsonUnionJsonConverter();
    }
}


internal class JsonUnionJsonConverter : JsonConverter<JsonUnion>
{
    private static void AddDiscriminatorModifier(JsonTypeInfo jsonTypeInfo)
    {
        if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
            return;

        if (jsonTypeInfo.Type != typeof(FooJ) && jsonTypeInfo.Type != typeof(BarJ))
        {
            return;
        }

        JsonPropertyInfo jsonPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(typeof(string), "$type");
        jsonPropertyInfo.Get = (x) =>
        {
            if (x is FooJ)
            {
                return "FooJ";
            }

            if (x is BarJ)
            {
                return "BarJ";
            }

            throw new ArgumentOutOfRangeException(nameof(x), x, null);
        };

        jsonTypeInfo.Properties.Insert(0, jsonPropertyInfo);
    }

    // based on https://github.com/rmja/PolyJson/blob/master/src/PolyJson/Converters/PolyJsonConverter.cs
    public override JsonUnion? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        JsonEncodedText discriminatorPropertyName = JsonEncodedText.Encode("$type");

        // Create a reader copy that we can use to find the discriminator value without advancing the original reader
        var nestedReader = reader;

        if (nestedReader.TokenType == JsonTokenType.None)
        {
            nestedReader.Read();
        }

        while (nestedReader.Read())
        {
            if (nestedReader.TokenType == JsonTokenType.PropertyName &&
                nestedReader.ValueTextEquals(discriminatorPropertyName.EncodedUtf8Bytes))
            {
                // Advance the reader to the property value
                nestedReader.Read();

                // Resolve the type from the discriminator value
                var subType = GetSubType(ref nestedReader);

                // Perform the actual deserialization with the original reader
                var result = JsonSerializer.Deserialize(ref reader, subType, options)!;
                if (result is BarJ barJ)
                {
                    return new JsonUnion(barJ);
                }

                if (result is FooJ fooJ)
                {
                    return new JsonUnion(fooJ);
                }

                throw new InvalidOperationException("Unable deserialize to JsonUnion");
            }
            else if (nestedReader.TokenType == JsonTokenType.StartObject ||
                     nestedReader.TokenType == JsonTokenType.StartArray)
            {
                // Skip until TokenType is EndObject/EndArray
                // Skip() always throws if IsFinalBlock == false, even when it could actually skip.
                // We therefore use TrySkip() and if that is impossible, we simply return without advancing the original reader.
                // For reference, see:
                // https://stackoverflow.com/questions/63038334/how-do-i-handle-partial-json-in-a-jsonconverter-while-using-deserializeasync-on
                // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Text.Json/src/System/Text/Json/Reader/Utf8JsonReader.cs#L310-L318
                if (!nestedReader.TrySkip())
                {
                    // Do not advance the reader, i.e. do not set reader = nestedReader,
                    // as that would cause us to have an incomplete object available for full final deserialization.

                    // NOTE: This does not actually work.
                    // The System.Text.Json reader will throw that the converter consumed too few bytes.
                    return default!;
                }
            }
            else if (nestedReader.TokenType == JsonTokenType.EndObject)
            {
                // We have exhausted the search within the object for a discriminator but it was not found.
                break;
            }
        }

        if (reader.IsFinalBlock)
        {
            throw new JsonException($"Unable to find discriminator property '{discriminatorPropertyName}'");
        }

        return default!;
    }

    private static Type GetSubType(ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected string discriminator value, got '{reader.TokenType}'");
        }

        if (reader.ValueTextEquals("FooJ"u8))
        {
            return typeof(FooJ);
        }

        if (reader.ValueTextEquals("BarJ"u8))
        {
            return typeof(BarJ);
        }

        throw new JsonException($"'{reader.GetString()}' is not a valid discriminator value");
    }

    public override void Write(Utf8JsonWriter writer, JsonUnion value, JsonSerializerOptions options)
    {
        var customOptions = new JsonSerializerOptions(options)
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver { Modifiers = { AddDiscriminatorModifier } }
        };

        value.Switch(
            x => JsonSerializer.Serialize(writer, x, customOptions),
            x => JsonSerializer.Serialize(writer, x, customOptions)
        );

        // TODO serialize simple types (int, string, etc.)
        // writer.WriteStartObject();
        // writer.WriteString("$type"u8, value.ValueType.ToString()); // discriminator
        // writer.WritePropertyName("Value"u8);
        // writer.WriteNull();
        // writer.WriteBoolean();
        // writer.WriteNumber();
        // writer.WriteString();
        // writer.WriteEndObject();
    }
}

public class JsonExperimentsTests
{
    [Fact]
    public void SerializeBar()
    {
        JsonUnion item = new BarJ(3, 42, BarStatus.Rejected);

        JsonSerializerOptions serializerOptions = new() { Converters = { new JsonStringEnumConverter() }, };

        var json = JsonSerializer.Serialize(item, serializerOptions);
        Assert.Equal("""{"$type":"BarJ","Id":3,"Cost":42,"Status":"Rejected"}""", json);
    }

    [Fact]
    public void DeserializeBar()
    {
        var json = """{"Id":3,"Cost":42,"Status":"Rejected","$type":"BarJ"}""";

        JsonSerializerOptions serializerOptions = new() { Converters = { new JsonStringEnumConverter() } };

        var result = JsonSerializer.Deserialize<JsonUnion>(json, serializerOptions);
        Assert.NotNull(result);
        Assert.True(result.IsBarJ);
        Assert.Equal(3, result.AsBarJ.Id);
        Assert.Equal(42, result.AsBarJ.Cost);
        Assert.Equal(BarStatus.Rejected, result.AsBarJ.Status);
    }

    [Fact]
    public void SerializeFoo()
    {
        JsonUnion item = new FooJ(3, "zzzz", new SecondDelivered("StringValue"));

        var json = JsonSerializer.Serialize(item);
        Assert.Equal("""{"$type":"FooJ","Id":3,"Title":"zzzz","Inner":{"$type":"SecondDelivered","Value":"StringValue"}}""", json);
    }

    [Fact]
    public void DeserializeFoo()
    {
        var json = """{"$type":"FooJ","Id":3,"Title":"zzzz","Inner":{"$type":"SecondDelivered","Value":"StringValue"}}""";

        var result = JsonSerializer.Deserialize<JsonUnion>(json);
        Assert.NotNull(result);
        Assert.True(result.IsFooJ);
        Assert.Equal(3, result.AsFooJ.Id);
        Assert.Equal("zzzz", result.AsFooJ.Title);
        var inner = Assert.IsType<SecondDelivered>(result.AsFooJ.Inner);
        Assert.Equal("StringValue", inner.Value);
    }
}