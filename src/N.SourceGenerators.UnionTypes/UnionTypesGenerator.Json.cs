using System.Text;

using N.SourceGenerators.UnionTypes.Extensions;
using N.SourceGenerators.UnionTypes.Models;

namespace N.SourceGenerators.UnionTypes;

public sealed partial class UnionTypesGenerator
{
    private static void GenerateJsonConverter(UnionType unionType,
        SourceProductionContext context,
        GeneratorContext generatorContext)
    {
        context.CancellationToken.ThrowIfCancellationRequested();
        BuildJsonConverter(unionType, context, generatorContext);
        context.CancellationToken.ThrowIfCancellationRequested();
        BuildJsonConverterAttribute(unionType, context, generatorContext);
    }

    private static void BuildJsonConverter(UnionType unionType, SourceProductionContext context, GeneratorContext generatorContext)
    {
        TypeDeclarationSyntax converterDeclaration = ClassDeclaration(unionType.Name + "JsonConverter")
            .AddModifiers(Token(SyntaxKind.InternalKeyword))
            .AddBaseListTypes(
                SimpleBaseType(
                    GenericType("System.Text.Json.Serialization.JsonConverter", unionType.Name)
                )
            )
            .ExcludeFromCodeCoverage(generatorContext.Options)
            .AddMembers(
                GetDiscriminatorMethod(unionType),
                AddDiscriminatorModifier(unionType),
                GetSubTypeMethod(unionType),
                ReadJsonConverterMethod(unionType),
                WriteJsonConverterMethod(unionType)
            );

        CompilationUnitSyntax compilationUnit = GetCompilationUnit(converterDeclaration, unionType.Namespace, generatorContext.Compilation);
        context.AddSource($"{unionType.SourceCodeFileName}JsonConverter.g.cs", compilationUnit.GetText(Encoding.UTF8));
    }

    private static void BuildJsonConverterAttribute(UnionType unionType, SourceProductionContext context, GeneratorContext generatorContext)
    {
        TypeDeclarationSyntax converterDeclaration = ClassDeclaration(unionType.Name + "JsonConverterAttribute")
            .AddModifiers(
                Token(SyntaxKind.InternalKeyword))
            .AddBaseListTypes(
                SimpleBaseType(
                    IdentifierName("System.Text.Json.Serialization.JsonConverterAttribute")
                )
            )
            .ExcludeFromCodeCoverage(generatorContext.Options)
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

        CompilationUnitSyntax compilationUnit = GetCompilationUnit(converterDeclaration, unionType.Namespace, generatorContext.Compilation);
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

            yield return ThrowException(
                "System.ArgumentOutOfRangeException",
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
            );
        }
    }

    private static MemberDeclarationSyntax GetSubTypeMethod(UnionType unionType)
    {
        return MethodDeclaration(
                IdentifierName("System.Type"),
                "GetSubType"
            )
            .AddModifiers(
                Token(SyntaxKind.PrivateKeyword),
                Token(SyntaxKind.StaticKeyword)
            ).AddParameterListParameters(
                Parameter("System.Text.Json.Utf8JsonReader", "reader")
                    .AddModifiers(Token(SyntaxKind.RefKeyword))
            )
            .AddBodyStatements(
                IfStatement(
                    NotEqualsExpression(
                        MemberAccess("reader", "TokenType"),
                        MemberAccess("System.Text.Json.JsonTokenType", "String")
                    ),
                    Block(
                        ThrowException(
                            "System.Text.Json.JsonException",
                            Argument(
                                InterpolatedString(
                                    InterpolatedText("Expected string discriminator value, got "),
                                    Interpolation(MemberAccess("reader", "TokenType"))
                                )
                            )
                        )
                    )
                )
            )
            .AddBodyStatements(
                GetTypeFromDiscriminator().ToArray()
            )
            .AddBodyStatements(
                ThrowException(
                    "System.Text.Json.JsonException",
                    Argument(
                        InterpolatedString(
                            Interpolation(
                                InvocationExpression(MemberAccess("reader", "GetString"))
                            ),
                            InterpolatedText(" is not a valid discriminator value")
                        )
                    )
                )
            );

        IEnumerable<StatementSyntax> GetTypeFromDiscriminator()
        {
            foreach (var variant in unionType.Variants)
            {
                if (!variant.HasValidTypeDiscriminator)
                {
                    continue;
                }

                yield return IfStatement(
                    InvocationExpression(
                        MemberAccess("reader", "ValueTextEquals")
                    ).AddArgumentListArguments(
                        Argument(
                            // TODO support int discriminator
                            Utf8StringLiteral((string)variant.TypeDiscriminator!)
                        )
                    ),
                    Block(
                        ReturnStatement(
                            TypeOfExpression(
                                IdentifierName(variant.TypeFullName)
                            )
                        )
                    )
                );
            }
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
            LocalVariableDeclaration(
                JsonPropertyInfoType(),
                "jsonPropertyInfo",
                InvocationExpression(
                    MemberAccess("jsonTypeInfo", "CreateJsonPropertyInfo")
                ).AddArgumentListArguments(
                    Argument(TypeOfExpression(StringType())),
                    Argument(StringLiteral(unionType.JsonOptions!.TypeDiscriminatorPropertyName))
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
            LocalVariableDeclaration(
                IdentifierName("System.Text.Json.JsonEncodedText"),
                "discriminatorPropertyName",
                InvocationExpression(
                    MemberAccess("System.Text.Json.JsonEncodedText", "Encode")
                ).AddArgumentListArguments(
                    Argument(StringLiteral(unionType.JsonOptions!.TypeDiscriminatorPropertyName))
                )
            ),
            LocalVariableDeclaration(
                IdentifierName("var"),
                "nestedReader",
                IdentifierName("reader")
            ),
            IfStatement(
                EqualsExpression(
                    MemberAccess("nestedReader", "TokenType"),
                    MemberAccess("System.Text.Json.JsonTokenType", "None")
                ),
                Block(
                    ExpressionStatement(
                        InvocationExpression(
                            MemberAccess("nestedReader", "Read")
                        )
                    )
                )
            ),
            WhileStatement(
                InvocationExpression(MemberAccess("nestedReader", "Read")),
                Block(
                    IfStatement(
                        BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            EqualsExpression(
                                MemberAccess("nestedReader", "TokenType"),
                                MemberAccess("System.Text.Json.JsonTokenType", "PropertyName")
                            ),
                            InvocationExpression(
                                MemberAccess("nestedReader", "ValueTextEquals"),
                                ArgumentList().AddArguments(
                                    Argument(
                                        MemberAccess("discriminatorPropertyName", "EncodedUtf8Bytes")
                                    )
                                )
                            )
                        ),
                        Block(
                                ExpressionStatement(
                                    InvocationExpression(MemberAccess("nestedReader", "Read"))
                                ),
                                LocalVariableDeclaration(
                                    IdentifierName("var"),
                                    "subType",
                                    InvocationExpression(IdentifierName("GetSubType"))
                                        .AddArgumentListArguments(
                                            Argument(IdentifierName("nestedReader"))
                                                .WithRefKindKeyword(Token(SyntaxKind.RefKeyword))
                                        )
                                ),
                                LocalVariableDeclaration(
                                    IdentifierName("var"),
                                    "result",
                                    InvocationExpression(
                                        MemberAccess("System.Text.Json.JsonSerializer", "Deserialize")
                                    ).AddArgumentListArguments(
                                        Argument("reader")
                                            .WithRefKindKeyword(Token(SyntaxKind.RefKeyword)),
                                        Argument("subType"),
                                        Argument("options")
                                    )
                                )
                            )
                            .AddStatements(CreateUnionFromJson(unionType).ToArray())
                            .AddStatements(
                                ThrowInvalidOperationException($"Unable to deserialize to {unionType.Name}"))
                    ).WithElse(
                        ElseClause(
                            IfStatement(
                                BinaryExpression(
                                    SyntaxKind.LogicalOrExpression,
                                    EqualsExpression(
                                        MemberAccess("nestedReader", "TokenType"),
                                        MemberAccess("System.Text.Json.JsonTokenType", "StartObject")
                                    ),
                                    EqualsExpression(
                                        MemberAccess("nestedReader", "TokenType"),
                                        MemberAccess("System.Text.Json.JsonTokenType", "StartArray")
                                    )
                                ),
                                Block(
                                    IfStatement(
                                        PrefixUnaryExpression(
                                            SyntaxKind.LogicalNotExpression,
                                            InvocationExpression(MemberAccess("nestedReader", "TrySkip"))
                                        ),
                                        Block(
                                            ReturnStatement(
                                                PostfixUnaryExpression(
                                                    SyntaxKind.SuppressNullableWarningExpression,
                                                    LiteralExpression(SyntaxKind.DefaultLiteralExpression)
                                                )
                                            )
                                        )
                                    )
                                )
                            ).WithElse(
                                ElseClause(
                                    IfStatement(
                                        EqualsExpression(
                                            MemberAccess("nestedReader", "TokenType"),
                                            MemberAccess("System.Text.Json.JsonTokenType", "EndObject")
                                        ),
                                        Block(
                                            BreakStatement()
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            ),
            IfStatement(
                MemberAccess("reader", "IsFinalBlock"),
                Block(
                    ThrowException(
                        "System.Text.Json.JsonException",
                        Argument(
                            InterpolatedString(
                                InterpolatedText(
                                    $"Unable to find discriminator property {unionType.JsonOptions!.TypeDiscriminatorPropertyName}")
                            )
                        )
                    )
                )
            ),
            ReturnStatement(
                PostfixUnaryExpression(
                    SyntaxKind.SuppressNullableWarningExpression,
                    LiteralExpression(
                        SyntaxKind.DefaultLiteralExpression
                    )
                )
            )
        );
    }

    private static IEnumerable<StatementSyntax> CreateUnionFromJson(UnionType unionType)
    {
        foreach (UnionTypeVariant variant in unionType.Variants)
        {
            if (variant.HasValidTypeDiscriminator)
            {
                yield return IfStatement(
                    IsPatternExpression(
                        IdentifierName("result"),
                        DeclarationPattern(
                            IdentifierName(variant.TypeFullName),
                            SingleVariableDesignation(Identifier(variant.Alias.ToStartLowerCase()))
                        )
                    ),
                    Block(
                        ReturnStatement(
                            ObjectCreationExpression(IdentifierName(unionType.TypeFullName))
                                .AddArgumentListArguments(
                                    Argument(variant.Alias.ToStartLowerCase())
                                )
                        )
                    )
                );
            }
        }
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
            LocalVariableDeclaration(
                IdentifierName("var"),
                "customOptions",
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
                        // TODO throw exception if variant doesn't have discriminator
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