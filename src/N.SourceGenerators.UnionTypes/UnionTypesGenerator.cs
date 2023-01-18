using System.Text;

using Microsoft.CodeAnalysis.Text;

using N.SourceGenerators.UnionTypes.Extensions;
using N.SourceGenerators.UnionTypes.Models;

namespace N.SourceGenerators.UnionTypes;

[Generator(LanguageNames.CSharp)]
public partial class UnionTypesGenerator : IIncrementalGenerator
{
    private const string FuncType = "global::System.Func";
    private const string ActionType = "global::System.Action";
    private const string CancellationTokenType = "global::System.Threading.CancellationToken";

    private const string UnionTypeAttributeName = "N.SourceGenerators.UnionTypes.UnionTypeAttribute";

    private const string UnionTypeAttributeText = """        
        #nullable enable
        using System;
        using System.Runtime.CompilerServices;
        
        namespace N.SourceGenerators.UnionTypes
        {
            [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
            internal sealed class UnionTypeAttribute : Attribute
            {
                public Type Type { get; }
                public string? Alias { get; }
                public int Order { get; }
        
                public UnionTypeAttribute(Type type, string? alias = null, [CallerLineNumber] int order = 0)
                {
                    Type = type;
                    Alias = alias;
                    Order = order;
                }
            }
        }
        """;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource("UnionTypeAttribute.g.cs", SourceText.From(UnionTypeAttributeText, Encoding.UTF8));
        });

        var unionReferences = GetUnionReferences(context);
        var unionTypeReferences = GetUnionTypeReferences(unionReferences);
        var unionTypeDiagnostics = GetUnionTypeDiagnostics(unionTypeReferences);

        ReportDiagnostics(context, unionTypeDiagnostics);

        var validUnionTypes = unionTypeDiagnostics
            .Where(utd => utd.Diagnostics.Count == 0);

        context.RegisterImplementationSourceOutput(validUnionTypes, static (context, item) =>
        {
            Execute(item.UnionType, context);
        });
    }

    private static IncrementalValuesProvider<UnionReference> GetUnionReferences(
        IncrementalGeneratorInitializationContext context)
    {
        return context.SyntaxProvider
            .CreateSyntaxProvider(
                static (s, _) => s.IsTypeWithAttributes(),
                static (ctx, ct) =>
                {
                    ct.ThrowIfCancellationRequested();

                    // TODO add enum LanguageFeatures { GenericAttributes (CSharp11) }
                    if (!ctx.SemanticModel.Compilation.HasLanguageVersionAtLeastEqualTo(LanguageVersion.CSharp8))
                    {
                        return default;
                    }

                    // If the syntax node doesn't have a declared symbol, just skip this node. This would be
                    // the case for eg. lambda attributes, but those are not supported by the generator.
                    if (ctx.SemanticModel.GetDeclaredSymbol(ctx.Node, ct) is not INamedTypeSymbol symbol)
                    {
                        return default;
                    }

                    if (!symbol.HasAttributeWithFullyQualifiedMetadataName(UnionTypeAttributeName))
                    {
                        return default;
                    }

                    TypeDeclarationSyntax typeDeclaration = (TypeDeclarationSyntax)ctx.Node;
                    return new UnionReference(typeDeclaration, symbol);
                })
            .Where(static u => u is not null)!;
    }

    private static IncrementalValuesProvider<UnionTypeReference> GetUnionTypeReferences(
        IncrementalValuesProvider<UnionReference> unionReferences)
    {
        return unionReferences
            .Select(static (r, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                List<UnionTypeVariant>? variants = null;
                foreach (AttributeData attribute in r.Symbol.GetAttributes())
                {
                    if (attribute.AttributeClass?.ToDisplayString() == UnionTypeAttributeName)
                    {
                        variants ??= new List<UnionTypeVariant>();

                        var typeSymbol = attribute.ConstructorArguments[0].Value as INamedTypeSymbol;

                        var alias = attribute.ConstructorArguments[1].Value?.ToString();
                        var order = (int)attribute.ConstructorArguments[2].Value!;

                        variants.Add(new UnionTypeVariant(typeSymbol!, alias, order));
                    }
                }

                if (variants == null)
                {
                    return default;
                }

                UnionType unionType = new(r.Symbol, variants.OrderBy(t => t.Order).ToArray());
                return new UnionTypeReference(unionType, r);
            })
            .Where(static u => u is not null)!;
    }

    static void Execute(UnionType unionType,
        SourceProductionContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        // TODO add public bool TryGet{Alias}([NotNullWhen(true)] out Alias? alias)
        // TODO override ToString
        // TODO add DebuggerDisplayAttribute
        // TODO add equality operators / Equals / GetHashCode
        // TODO implement serialization to JSON (and back?)
        TypeDeclarationSyntax typeDeclaration = unionType.ContainerType.IsReferenceType
            ? ClassDeclaration(unionType.ContainerType.Name)
            : StructDeclaration(unionType.ContainerType.Name);

        typeDeclaration = typeDeclaration
            .AddModifiers(Token(SyntaxKind.PartialKeyword))
            .AddMembers(VariantsMembers(unionType))
            .AddMembers(
                MatchMethod(unionType, isAsync: false),
                MatchMethod(unionType, isAsync: true),
                SwitchMethod(unionType, isAsync: false),
                SwitchMethod(unionType, isAsync: true),
                ValueTypeProperty(unionType)
            );

        const string comment = """
            // <auto-generated>
            //   This code was generated by https://github.com/Ne4to/N.SourceGenerators.UnionTypes
            //   Feel free to open an issue
            // </auto-generated>
            """;

        SyntaxTriviaList syntaxTriviaList = TriviaList(
            Comment(comment),
            Trivia(PragmaWarningDirectiveTrivia(Token(SyntaxKind.DisableKeyword), true)),
            Trivia(NullableDirectiveTrivia(Token(SyntaxKind.EnableKeyword), true)));

        CompilationUnitSyntax compilationUnit;
        if (unionType.ContainerType.ContainingNamespace.Name is "")
        {
            compilationUnit = CompilationUnit()
                .AddMembers(typeDeclaration.WithLeadingTrivia(syntaxTriviaList))
                .NormalizeWhitespace();
        }
        else
        {
            compilationUnit = CompilationUnit()
                .AddMembers(
                    NamespaceDeclaration(IdentifierName(unionType.ContainerType.ContainingNamespace.Name))
                        .WithLeadingTrivia(syntaxTriviaList)
                        .AddMembers(typeDeclaration)
                )
                .NormalizeWhitespace();
        }

        context.AddSource($"{unionType.ContainerType.Name}.g.cs", compilationUnit.GetText(Encoding.UTF8));
    }

    private static MemberDeclarationSyntax[] VariantsMembers(UnionType unionType)
    {
        var variantsMembers = unionType
            .Variants
            .SelectMany(v => VariantMembers(unionType, v))
            .ToArray();

        return variantsMembers;
    }

    private static IEnumerable<MemberDeclarationSyntax> VariantMembers(
        UnionType unionType,
        UnionTypeVariant variant)
    {
        yield return Field(variant);
        yield return IsProperty(variant);
        yield return AsProperty(variant);
        yield return Ctor(unionType, variant);
        yield return ImplicitOperatorToUnion(unionType, variant);
        yield return ExplicitOperatorFromUnion(unionType, variant);
        yield return TryGetMethod(variant);
    }

    private static FieldDeclarationSyntax Field(UnionTypeVariant variant)
    {
        NullableTypeSyntax fieldType = NullableType(IdentifierName(variant.TypeFullName));
        var field = FieldDeclaration(
                VariableDeclaration(fieldType)
                    .AddVariables(VariableDeclarator(Identifier(variant.FieldName)))
            )
            .AddModifiers(
                Token(SyntaxKind.PrivateKeyword),
                Token(SyntaxKind.ReadOnlyKeyword)
            );
        return field;
    }

    private static PropertyDeclarationSyntax IsProperty(UnionTypeVariant variant)
    {
        return PropertyDeclaration(IdentifierName("bool"), Identifier(variant.IsPropertyName))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .WithExpressionBody(ArrowExpressionClause(
                BinaryExpression(SyntaxKind.NotEqualsExpression,
                    IdentifierName(variant.FieldName),
                    LiteralExpression(SyntaxKind.NullLiteralExpression))))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static PropertyDeclarationSyntax AsProperty(UnionTypeVariant variant)
    {
        return PropertyDeclaration(IdentifierName(variant.TypeFullName), Identifier(variant.AsPropertyName))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .WithExpressionBody(ArrowExpressionClause(
                BinaryExpression(SyntaxKind.CoalesceExpression,
                    IdentifierName(variant.FieldName),
                    ThrowExpression(
                        NewInvalidOperationException($"This is not a {variant.TypeSymbol.Name}")
                    )
                )))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static MemberDeclarationSyntax Ctor(UnionType unionType, UnionTypeVariant variant)
    {
        InvocationExpressionSyntax checkArgumentExpression = InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName("System.ArgumentNullException"),
                    IdentifierName("ThrowIfNull"))
            )
            .AddArgumentListArguments(
                Argument(IdentifierName(variant.Alias))
            );

        AssignmentExpressionSyntax assignmentExpression = AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            IdentifierName(variant.FieldName),
            IdentifierName(variant.Alias));

        BlockSyntax body =
            Block(
                ExpressionStatement(checkArgumentExpression),
                ExpressionStatement(assignmentExpression)
            );

        return ConstructorDeclaration(Identifier(unionType.ContainerType.Name))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(
                Parameter(Identifier(variant.Alias))
                    .WithType(IdentifierName(variant.TypeFullName))
            )
            .WithBody(body);
    }

    private static ConversionOperatorDeclarationSyntax ImplicitOperatorToUnion(UnionType unionType,
        UnionTypeVariant variant)
    {
        return ConversionOperatorDeclaration(
                Token(SyntaxKind.ImplicitKeyword),
                IdentifierName(unionType.ContainerType.Name)
            )
            .AddModifiers(
                Token(SyntaxKind.PublicKeyword),
                Token(SyntaxKind.StaticKeyword)
            )
            .AddParameterListParameters(
                Parameter(Identifier(variant.Alias))
                    .WithType(IdentifierName(variant.TypeFullName))
            )
            .WithExpressionBody(
                ArrowExpressionClause(
                    ObjectCreationExpression(IdentifierName(unionType.ContainerType.Name))
                        .AddArgumentListArguments(
                            Argument(IdentifierName(variant.Alias))
                        )
                ))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static MemberDeclarationSyntax ExplicitOperatorFromUnion(UnionType unionType, UnionTypeVariant variant)
    {
        ParameterSyntax parameter =
            Parameter(Identifier("value"))
                .WithType(IdentifierName(unionType.ContainerType.Name));

        return ConversionOperatorDeclaration(
                Token(SyntaxKind.ExplicitKeyword),
                IdentifierName(variant.TypeFullName)
            )
            .AddModifiers(
                Token(SyntaxKind.PublicKeyword),
                Token(SyntaxKind.StaticKeyword)
            )
            .AddParameterListParameters(parameter)
            .WithExpressionBody(
                ArrowExpressionClause(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("value"),
                        IdentifierName(variant.AsPropertyName)
                    )
                )
            )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static MemberDeclarationSyntax TryGetMethod(UnionTypeVariant variant)
    {
        AttributeListSyntax attributeList = AttributeList()
            .AddAttributes(
                Attribute(IdentifierName("System.Diagnostics.CodeAnalysis.NotNullWhen"),
                    AttributeArgumentList(
                        SeparatedList(
                            new[] { AttributeArgument(LiteralExpression(SyntaxKind.TrueLiteralExpression)) }
                        )
                    )
                )
            );

        return MethodDeclaration(
                IdentifierName("bool"),
                Identifier($"TryGet{variant.Alias}")
            )
            .AddModifiers(
                Token(SyntaxKind.PublicKeyword)
            )
            .AddParameterListParameters(
                Parameter(Identifier("value"))
                    .WithType(NullableType(IdentifierName(variant.TypeFullName)))
                    .AddModifiers(Token(SyntaxKind.OutKeyword))
                    .AddAttributeLists(attributeList)
            ).WithBody(
                Block(
                    IfStatement(
                            BinaryExpression(
                                SyntaxKind.NotEqualsExpression,
                                IdentifierName(variant.FieldName),
                                LiteralExpression(SyntaxKind.NullLiteralExpression)
                            ),
                            Block(
                                ExpressionStatement(
                                    AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                        IdentifierName("value"),
                                        IdentifierName(variant.FieldName)
                                    )
                                ),
                                ReturnStatement(LiteralExpression(SyntaxKind.TrueLiteralExpression))
                            )
                        )
                        .WithElse(ElseClause(
                            Block(
                                ExpressionStatement(
                                    AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                        IdentifierName("value"),
                                        LiteralExpression(SyntaxKind.DefaultLiteralExpression)
                                    )
                                ),
                                ReturnStatement(LiteralExpression(SyntaxKind.FalseLiteralExpression))
                            )))
                )
            );
    }

    private static MemberDeclarationSyntax MatchMethod(UnionType unionType, bool isAsync)
    {
        return
            MethodDeclaration(
                    isAsync ? TaskIdentifier("TOut") : IdentifierName("TOut"),
                    isAsync ? "MatchAsync" : "Match"
                )
                .AddModifiers(
                    Token(SyntaxKind.PublicKeyword)
                )
                .AddModifierWhen(isAsync, Token(SyntaxKind.AsyncKeyword))
                .AddTypeParameterListParameters(
                    TypeParameter("TOut")
                )
                .AddParameterListParameters(
                    VariantsParameters(unionType, v => MatchMethodParameter(v, isAsync))
                )
                .AddParameterListParameterWhen(
                    isAsync,
                    Parameter(Identifier("ct"))
                        .WithType(IdentifierName(CancellationTokenType))
                )
                .AddBodyStatements(
                    VariantsBodyStatements(unionType, v => MatchStatement(v, isAsync))
                );
    }

    private static ParameterSyntax MatchMethodParameter(UnionTypeVariant variant, bool isAsync)
    {
        var parameterType =
            GenericName(FuncType)
                .AddTypeArgumentListArguments(IdentifierName(variant.TypeFullName))
                .AddTypeArgumentListArgumentsWhen(isAsync, IdentifierName(CancellationTokenType))
                .AddTypeArgumentListArguments(isAsync ? TaskIdentifier("TOut") : IdentifierName("TOut"));

        var parameter =
            Parameter(Identifier($"match{variant.Alias}"))
                .WithType(parameterType);

        return parameter;
    }

    private static StatementSyntax MatchStatement(UnionTypeVariant variant, bool isAsync)
    {
        return IfStatement(
            IdentifierName(variant.IsPropertyName),
            ReturnStatement(
                InvocationExpression(IdentifierName($"match{variant.Alias}"))
                    .AddArgumentListArguments(
                        Argument(IdentifierName(variant.AsPropertyName))
                    )
                    .AddArgumentListArgumentWhen(isAsync, Argument(IdentifierName("ct")))
                    .AwaitWithConfigureAwaitWhen(isAsync)
            )
        );
    }

    private static MemberDeclarationSyntax SwitchMethod(UnionType unionType, bool isAsync)
    {
        return
            MethodDeclaration(
                    isAsync ? IdentifierName(TaskType) : PredefinedType(Token(SyntaxKind.VoidKeyword)),
                    isAsync ? "SwitchAsync" : "Switch"
                )
                .AddModifiers(
                    Token(SyntaxKind.PublicKeyword)
                )
                .AddModifierWhen(isAsync, Token(SyntaxKind.AsyncKeyword))
                .AddParameterListParameters(
                    VariantsParameters(unionType, v => SwitchMethodParameter(v, isAsync))
                )
                .AddParameterListParameterWhen(
                    isAsync,
                    Parameter(Identifier("ct"))
                        .WithType(IdentifierName(CancellationTokenType))
                )
                .AddBodyStatements(
                    VariantsBodyStatements(unionType, v => SwitchStatement(v, isAsync))
                );
    }

    private static ParameterSyntax SwitchMethodParameter(UnionTypeVariant variant, bool isAsync)
    {
        var parameterType =
            GenericName(isAsync ? FuncType : ActionType)
                .AddTypeArgumentListArguments(IdentifierName(variant.TypeFullName))
                .AddTypeArgumentListArgumentsWhen(isAsync, IdentifierName(CancellationTokenType))
                .AddTypeArgumentListArgumentsWhen(isAsync, IdentifierName(TaskType));

        var parameter =
            Parameter(Identifier($"switch{variant.Alias}"))
                .WithType(parameterType);

        return parameter;
    }

    private static StatementSyntax SwitchStatement(UnionTypeVariant variant, bool isAsync)
    {
        return IfStatement(
            IdentifierName(variant.IsPropertyName),
            Block(
                ExpressionStatement(
                    InvocationExpression(IdentifierName($"switch{variant.Alias}"))
                        .AddArgumentListArguments(
                            Argument(IdentifierName(variant.AsPropertyName))
                        )
                        .AddArgumentListArgumentWhen(isAsync, Argument(IdentifierName("ct")))
                        .AwaitWithConfigureAwaitWhen(isAsync)),
                ReturnStatement()
            )
        );
    }

    private static MemberDeclarationSyntax ValueTypeProperty(UnionType unionType)
    {
        return
            PropertyDeclaration(
                    IdentifierName("global::System.Type"),
                    "ValueType"
                )
                .AddModifiers(
                    Token(SyntaxKind.PublicKeyword)
                )
                .AddAccessorListAccessors(
                    AccessorDeclaration(
                        SyntaxKind.GetAccessorDeclaration,
                        Block(
                            VariantsBodyStatements(unionType, TypeStatement)
                        )
                    )
                );
    }

    private static StatementSyntax TypeStatement(UnionTypeVariant variant)
    {
        return IfStatement(
            IdentifierName(variant.IsPropertyName),
            ReturnStatement(
                TypeOfExpression(
                    IdentifierName(variant.TypeFullName)
                )
            )
        );
    }

    #region Helper methods

    private static StatementSyntax[] VariantsBodyStatements(UnionType unionType,
        Func<UnionTypeVariant, StatementSyntax> statementFunc)
    {
        return unionType
            .Variants
            .Select(statementFunc)
            .Concat(new StatementSyntax[] { ThrowUnknownType() })
            .ToArray();
    }

    private static ParameterSyntax[] VariantsParameters(UnionType unionType,
        Func<UnionTypeVariant, ParameterSyntax> selector)
    {
        return unionType
            .Variants
            .Select(selector)
            .ToArray();
    }

    private static ThrowStatementSyntax ThrowUnknownType()
    {
        return ThrowStatement(
            NewInvalidOperationException("Unknown type")
        );
    }

    #endregion
}