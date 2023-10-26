using System.Text;

using N.SourceGenerators.UnionTypes.Models;

namespace N.SourceGenerators.UnionTypes;

public sealed partial class UnionTypesGenerator
{
    private static void GenerateJsonConverter(UnionType unionType,
        SourceProductionContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();
        BuildJsonConverter(unionType, context);
        context.CancellationToken.ThrowIfCancellationRequested();
        BuildJsonConverterAttribute(unionType, context);
    }

    private static void BuildJsonConverter(UnionType unionType, SourceProductionContext context)
    {
        TypeDeclarationSyntax converterDeclaration = ClassDeclaration(unionType.Name + "JsonConverter")
            .AddModifiers(Token(SyntaxKind.InternalKeyword))
            .AddBaseListTypes(
                SimpleBaseType(
                    GenericType("System.Text.Json.Serialization.JsonConverter", unionType.Name)
                )
            )
            .AddMembers(
                GetDiscriminatorMethod(unionType),
                AddDiscriminatorModifier(unionType),
                ReadJsonConverterMethod(unionType),
                WriteJsonConverterMethod(unionType)
            );

        CompilationUnitSyntax compilationUnit = GetCompilationUnit(converterDeclaration, unionType.Namespace);
        context.AddSource($"{unionType.SourceCodeFileName}JsonConverter.g.cs", compilationUnit.GetText(Encoding.UTF8));
    }

    private static void BuildJsonConverterAttribute(UnionType unionType, SourceProductionContext context)
    {
        TypeDeclarationSyntax converterDeclaration = ClassDeclaration(unionType.Name + "JsonConverterAttribute")
            .AddModifiers(
                Token(SyntaxKind.InternalKeyword))
            .AddBaseListTypes(
                SimpleBaseType(
                    IdentifierName("System.Text.Json.Serialization.JsonConverterAttribute")
                )
            )
            .AddMembers(
                MethodDeclaration(
                        NullableType(IdentifierName("System.Text.Json.Serialization.JsonConverter")),
                        "CreateConverter"
                    ).AddModifiers(
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.OverrideKeyword))
                    .AddParameterListParameters(
                        Parameter("System.Type", "typeToConvert"))
                    .AddBodyStatements(
                        ReturnStatement(
                            ObjectCreationExpression(
                                    IdentifierName(unionType.Name + "JsonConverter")
                                )
                                .AddArgumentListArguments()
                        )
                    )
            );

        CompilationUnitSyntax compilationUnit = GetCompilationUnit(converterDeclaration, unionType.Namespace);
        context.AddSource($"{unionType.SourceCodeFileName}JsonConverterAttribute.g.cs",
            compilationUnit.GetText(Encoding.UTF8));
    }

    private static MemberDeclarationSyntax GetDiscriminatorMethod(UnionType unionType)
    {
        return MethodDeclaration(
            NullableType(ObjectType()),
            "GetDiscriminator"
        ).AddModifiers(
            Token(SyntaxKind.PrivateKeyword),
            Token(SyntaxKind.StaticKeyword)
        ).AddParameterListParameters(
            Parameter(ObjectType(), "x")
        ).AddBodyStatements(
            BodyStatements().ToArray()
        );

        IEnumerable<StatementSyntax> BodyStatements()
        {
            foreach (var variant in unionType.Variants)
            {
                if (variant.HasValidTypeDiscriminator)
                {
                    yield return IfStatement(
                        IsExpression("x", IdentifierName(variant.TypeFullName)),
                        Block(
                            // TODO support int discriminator
                            ReturnStatement(StringLiteral(variant.TypeDiscriminator!.ToString()))
                        )
                    );
                }
            }

            yield return ThrowStatement(
                ObjectCreationExpression(IdentifierName("System.ArgumentOutOfRangeException"))
                    .AddArgumentListArguments(
                        Argument(NameOfExpressions("x")),
                        Argument("x"),
                        Argument(
                            InterpolatedString(
                                Interpolation(
                                    InvocationExpression(MemberAccess("x", "GetType"))
                                ),
                                InterpolatedText(" has no discriminator specified in "),
                                Interpolation(TypeOfExpression(IdentifierName(unionType.TypeFullName)))
                            )
                        )
                    )
            );
        }
    }

    private static MemberDeclarationSyntax AddDiscriminatorModifier(UnionType unionType)
    {
        return MethodDeclaration(
            VoidType(),
            "AddDiscriminatorModifier"
        ).AddModifiers(
            Token(SyntaxKind.PrivateKeyword),
            Token(SyntaxKind.StaticKeyword)
        ).AddParameterListParameters(
            Parameter("System.Text.Json.Serialization.Metadata.JsonTypeInfo", "jsonTypeInfo")
        ).AddBodyStatements(
            // if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
            //     return;
            IfStatement(
                NotEqualsExpression(
                    MemberAccess("jsonTypeInfo", "Kind"),
                    MemberAccess("System.Text.Json.Serialization.Metadata.JsonTypeInfoKind", "Object")
                ),
                ReturnStatement()
            ),

            // TODO check x type
            // if (jsonTypeInfo.Type != typeof(FooJ) && jsonTypeInfo.Type != typeof(BarJ))
            // {
            //  return;
            // }

            // JsonPropertyInfo jsonPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(typeof(string), "$type");
            LocalDeclarationStatement(
                VariableDeclaration(
                    JsonPropertyInfoType()
                ).AddVariables(
                    VariableDeclarator("jsonPropertyInfo")
                        .WithInitializer(
                            EqualsValueClause(
                                InvocationExpression(
                                    MemberAccess("jsonTypeInfo", "CreateJsonPropertyInfo")
                                ).AddArgumentListArguments(
                                    Argument(TypeOfExpression(StringType())),
                                    Argument(StringLiteral(unionType.JsonOptions!.TypeDiscriminatorPropertyName))
                                )
                            )
                        )
                )
            ),

            // jsonPropertyInfo.Get = GetDiscriminator;
            ExpressionStatement(
                SimpleAssignmentExpression(
                    MemberAccess("jsonPropertyInfo", "Get"),
                    IdentifierName("GetDiscriminator")
                )
            ),

            // jsonTypeInfo.Properties.Insert(0, jsonPropertyInfo);
            ExpressionStatement(
                InvocationExpression(
                    MemberAccess("jsonTypeInfo", "Properties", "Insert")
                ).AddArgumentListArguments(
                    Argument(NumericLiteral(0)),
                    Argument("jsonPropertyInfo")
                )
            )
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
            Parameter("System.Text.Json.Utf8JsonReader", "reader")
                .AddModifiers(Token(SyntaxKind.RefKeyword)),
            Parameter("System.Type", "typeToConvert"),
            Parameter("System.Text.Json.JsonSerializerOptions", "options")
        ).AddBodyStatements(
            ThrowUnknownType()
        );
    }

    // public override void Write(Utf8JsonWriter writer, JsonUnion value, JsonSerializerOptions options)
    private static MemberDeclarationSyntax WriteJsonConverterMethod(UnionType unionType)
    {
        return MethodDeclaration(
            VoidType(),
            "Write"
        ).AddModifiers(
            Token(SyntaxKind.PublicKeyword),
            Token(SyntaxKind.OverrideKeyword)
        ).AddParameterListParameters(
            Parameter("System.Text.Json.Utf8JsonWriter", "writer"),
            Parameter(unionType.TypeFullName, "value"),
            Parameter("System.Text.Json.JsonSerializerOptions", "options")
        ).AddBodyStatements(
            // TODO simplify
            LocalDeclarationStatement(
                VariableDeclaration(
                    IdentifierName("var")
                ).AddVariables(
                    VariableDeclarator("customOptions")
                        .WithInitializer(
                            EqualsValueClause(
                                ObjectCreationExpression(
                                    IdentifierName("System.Text.Json.JsonSerializerOptions"),
                                    ArgumentList().AddArguments(Argument("options")),
                                    ObjectInitializerExpression(
                                        SimpleAssignmentExpression(
                                            "TypeInfoResolver",
                                            ObjectCreationExpression(
                                                DefaultJsonTypeInfoResolverType()
                                            ).WithInitializer(
                                                ObjectInitializerExpression(
                                                    SimpleAssignmentExpression(
                                                        "Modifiers",
                                                        CollectionInitializerExpression(
                                                            IdentifierName("AddDiscriminatorModifier")
                                                        )
                                                    )
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        )
                )
            ),
            ExpressionStatement(
                InvocationExpression(MemberAccess("value", "Switch"))
                    .AddArgumentListArguments(
                        GetSerializeVariantArguments().ToArray()
                    )
            )
        );

        IEnumerable<ArgumentSyntax> GetSerializeVariantArguments()
        {
            foreach (var _ in unionType.Variants)
            {
                yield return Argument(
                    SimpleLambdaExpression(
                        Parameter(Identifier("x")),
                        null,
                        InvocationExpression(
                            MemberAccess("System.Text.Json.JsonSerializer", "Serialize")
                        ).AddArgumentListArguments(
                            Argument("writer"),
                            Argument("x"),
                            Argument("customOptions")
                        )
                    )
                );
            }
        }
    }
}