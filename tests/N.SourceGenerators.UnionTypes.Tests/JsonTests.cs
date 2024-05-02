using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace N.SourceGenerators.UnionTypes.Tests;

[UsesVerify]
public class JsonTests
{
    [Fact]
    public Task ItDoesNotFail()
    {
        const string source =
            """
            using System;
            using N.SourceGenerators.UnionTypes;

            public record JsonTestsFooJ(int Id, string Title);

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
            """;

        return TestHelper.Verify<UnionTypesGenerator>(
            source, 
            LanguageVersion.Latest,
            null,
            null,
            MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location),
            MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Text.Json")).Location),
            MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Text.Encodings.Web")).Location)
        );
    }
}