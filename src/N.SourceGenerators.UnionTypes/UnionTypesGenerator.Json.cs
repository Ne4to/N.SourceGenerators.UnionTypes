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
                GetDiscriminatorMethod(unionType),
                AddDiscriminatorModifier(unionType),
                ReadJsonConverterMethod(unionType),
                WriteJsonConverterMethod(unionType)
            );

        CompilationUnitSyntax compilationUnit = GetCompilationUnit(typeDeclaration, unionType.Namespace);
        context.AddSource($"{unionType.SourceCodeFileName}JsonConverter.g.cs", compilationUnit.GetText(Encoding.UTF8));
    }

    private static MemberDeclarationSyntax GetDiscriminatorMethod(UnionType unionType)
    {
        return MethodDeclaration(
            NullableType(PredefinedType(Token(SyntaxKind.ObjectKeyword))),
            "GetDiscriminator"
        ).AddModifiers(
            Token(SyntaxKind.PrivateKeyword),
            Token(SyntaxKind.StaticKeyword)
        ).AddParameterListParameters(
            Parameter(Identifier("x"))
                .WithType(PredefinedType(Token(SyntaxKind.ObjectKeyword)))
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
                        BinaryExpression(
                            SyntaxKind.IsExpression,
                            IdentifierName("x"),
                            IdentifierName(variant.TypeFullName)
                        ),
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
                        Argument(InvocationExpression(IdentifierName("nameof"))
                            .AddArgumentListArguments(Argument(IdentifierName("x")))),
                        Argument(IdentifierName("x")),
                        Argument(InterpolatedString(
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
            PredefinedType(Token(SyntaxKind.VoidKeyword)),
            "AddDiscriminatorModifier"
        ).AddModifiers(
            Token(SyntaxKind.PrivateKeyword),
            Token(SyntaxKind.StaticKeyword)
        ).AddParameterListParameters(
            Parameter(Identifier("jsonTypeInfo"))
                .WithType(IdentifierName("System.Text.Json.Serialization.Metadata.JsonTypeInfo"))
        ).AddBodyStatements(
            // if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
            //     return;
            IfStatement(
                BinaryExpression(
                    SyntaxKind.NotEqualsExpression,
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

            // TODO simplify
            // JsonPropertyInfo jsonPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(typeof(string), "$type");
            LocalDeclarationStatement(
                VariableDeclaration(
                    IdentifierName("System.Text.Json.Serialization.Metadata.JsonPropertyInfo")
                ).AddVariables(
                    VariableDeclarator("jsonPropertyInfo")
                        .WithInitializer(
                            EqualsValueClause(
                                InvocationExpression(
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName("jsonTypeInfo"),
                                        IdentifierName("CreateJsonPropertyInfo")
                                    )
                                ).AddArgumentListArguments(
                                    Argument(TypeOfExpression(PredefinedType(Token(SyntaxKind.StringKeyword)))),
                                    Argument(StringLiteral(unionType.JsonOptions!.TypeDiscriminatorPropertyName))
                                )
                            )
                        )
                )
            ),

            // jsonPropertyInfo.Get = GetDiscriminator;
            ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    MemberAccess("jsonPropertyInfo", "Get"),
                    IdentifierName("GetDiscriminator")
                )
            ),

            // jsonTypeInfo.Properties.Insert(0, jsonPropertyInfo);
            ExpressionStatement(
                InvocationExpression(
                    MemberAccess(MemberAccess("jsonTypeInfo", "Properties"), "Insert")
                ).AddArgumentListArguments(
                    Argument(NumericLiteral(0)),
                    Argument(IdentifierName("jsonPropertyInfo"))
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