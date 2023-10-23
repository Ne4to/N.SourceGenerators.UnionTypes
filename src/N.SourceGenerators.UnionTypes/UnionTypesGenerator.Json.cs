using System.Text;

using N.SourceGenerators.UnionTypes.Models;

namespace N.SourceGenerators.UnionTypes;

public sealed partial class UnionTypesGenerator
{
    private static void GenerateJsonConverter(UnionType unionType,
        SourceProductionContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        TypeDeclarationSyntax typeDeclaration = ClassDeclaration(unionType.Name + "JsonConverter")
            .WithBaseList(
                BaseList(
                    SeparatedList(
                        new BaseTypeSyntax[]
                        {
                            SimpleBaseType(
                                GenericType("System.Text.Json.Serialization.JsonConverter", unionType.Name)
                            )
                        })
                )
            )
            .AddMembers(
                AddDiscriminatorModifier(unionType),
                ReadJsonConverterMethod(unionType),
                WriteJsonConverterMethod(unionType)
            );

        CompilationUnitSyntax compilationUnit = GetCompilationUnit(typeDeclaration, unionType.Namespace);
        context.AddSource($"{unionType.SourceCodeFileName}JsonConverter.g.cs", compilationUnit.GetText(Encoding.UTF8));
    }

    private static MemberDeclarationSyntax AddDiscriminatorModifier(UnionType unionType)
    {
        return MethodDeclaration(
            PredefinedType(Token(SyntaxKind.VoidKeyword)),
            "AddDiscriminatorModifier"
        ).AddModifiers(
            Token(SyntaxKind.PrivateKeyword),
            Token(SyntaxKind.StaticKeyword)
        ).AddParameterListParameters(
            Parameter(Identifier("jsonTypeInfo"))
                .WithType(IdentifierName("System.Text.Json.Serialization.Metadata.JsonTypeInfo"))
        ).AddBodyStatements(
            // SingleVariableDesignation(Identifier("jsonPropertyInfo"))
        );
    }

    // public override JsonUnion? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    private static MemberDeclarationSyntax ReadJsonConverterMethod(UnionType unionType)
    {
        return MethodDeclaration(
            IdentifierName(unionType.TypeFullName),
            "Read"
        ).AddModifiers(
            Token(SyntaxKind.PublicKeyword),
            Token(SyntaxKind.OverrideKeyword)
        ).AddParameterListParameters(
            Parameter(Identifier("reader"))
                .AddModifiers(Token(SyntaxKind.RefKeyword))
                .WithType(IdentifierName("System.Text.Json.Utf8JsonReader")),
            Parameter(Identifier("typeToConvert"))
                .WithType(IdentifierName("System.Type")),
            Parameter(Identifier("options"))
                .WithType(IdentifierName("System.Text.Json.JsonSerializerOptions"))
        ).AddBodyStatements(
            ThrowUnknownType()
        );
    }

    // public override void Write(Utf8JsonWriter writer, JsonUnion value, JsonSerializerOptions options)
    private static MemberDeclarationSyntax WriteJsonConverterMethod(UnionType unionType)
    {
        return MethodDeclaration(
            PredefinedType(Token(SyntaxKind.VoidKeyword)),
            "Write"
        ).AddModifiers(
            Token(SyntaxKind.PublicKeyword),
            Token(SyntaxKind.OverrideKeyword)
        ).AddParameterListParameters(
            Parameter(Identifier("writer"))
                .WithType(IdentifierName("System.Text.Json.Utf8JsonWriter")),
            Parameter(Identifier("value"))
                .WithType(IdentifierName(unionType.TypeFullName)),
            Parameter(Identifier("options"))
                .WithType(IdentifierName("System.Text.Json.JsonSerializerOptions"))
        ).AddBodyStatements(
            ThrowUnknownType()
        );
    }
}