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

    private const string VariantIdFieldName = "_variantId";

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

                        var typeSymbol = attribute.ConstructorArguments[0].Value as ITypeSymbol;

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

        TypeDeclarationSyntax typeDeclaration = unionType.IsReferenceType
            ? ClassDeclaration(unionType.Name)
            : StructDeclaration(unionType.Name);

        typeDeclaration = typeDeclaration
            .AddAttributeLists(GetTypeAttributes(unionType))
            .AddModifiers(Token(SyntaxKind.PartialKeyword))
            .AddMembersWhen(unionType.UseStructLayout, VariantIdField())
            .AddMembers(VariantsMembers(unionType))
            .AddMembers(
                MatchMethod(unionType, isAsync: false),
                MatchMethod(unionType, isAsync: true),
                SwitchMethod(unionType, isAsync: false),
                SwitchMethod(unionType, isAsync: true),
                ValueTypeProperty(unionType),
                GetHashCodeMethod(unionType),
                EqualsOperator(unionType, equal: true),
                EqualsOperator(unionType, equal: false),
                GenericEqualsMethod(unionType),
                ToStringMethod(unionType)
            )
            .AddMembersWhen(unionType.IsReferenceType, ClassEqualsMethod(unionType))
            .AddMembersWhen(!unionType.IsReferenceType, StructEqualsMethod(unionType))
            .WithBaseList(
                BaseList(
                    SeparatedList(
                        new BaseTypeSyntax[]
                        {
                            SimpleBaseType(
                                GenericType("System.IEquatable", unionType.Name)
                            )
                        }
                    )
                )
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
        if (unionType.Namespace is null)
        {
            compilationUnit = CompilationUnit()
                .AddMembers(typeDeclaration.WithLeadingTrivia(syntaxTriviaList))
                .NormalizeWhitespace();
        }
        else
        {
            compilationUnit = CompilationUnit()
                .AddMembers(
                    NamespaceDeclaration(IdentifierName(unionType.Namespace))
                        .WithLeadingTrivia(syntaxTriviaList)
                        .AddMembers(typeDeclaration)
                )
                .NormalizeWhitespace();
        }

        context.AddSource($"{unionType.Name}.g.cs", compilationUnit.GetText(Encoding.UTF8));
    }

    private static MemberDeclarationSyntax VariantIdField()
    {
        AttributeListSyntax attributes = AttributeList()
            .AddAttributes(
                FieldOffsetAttribute(0)
            );

        return FieldDeclaration(
                VariableDeclaration(PredefinedType(Token(SyntaxKind.IntKeyword)))
                    .AddVariables(VariableDeclarator(Identifier(VariantIdFieldName)))
            )
            .AddModifiers(
                Token(SyntaxKind.PrivateKeyword),
                Token(SyntaxKind.ReadOnlyKeyword)
            ).AddAttributeLists(attributes);
    }

    private static AttributeSyntax FieldOffsetAttribute(int offset)
    {
        return Attribute(IdentifierName("System.Runtime.InteropServices.FieldOffset"))
            .AddArgumentListArguments(
                AttributeArgument(NumericLiteral(offset))
            );
    }

    private static AttributeListSyntax[] GetTypeAttributes(UnionType unionType)
    {
        if (!unionType.UseStructLayout)
        {
            return Array.Empty<AttributeListSyntax>();
        }

        AttributeListSyntax attributes = AttributeList()
            .AddAttributes(
                Attribute(IdentifierName("System.Runtime.InteropServices.StructLayout"))
                    .AddArgumentListArguments(
                        AttributeArgument(
                            MemberAccess("System.Runtime.InteropServices.LayoutKind", "Explicit")
                        )
                    )
            );

        return new[] { attributes };
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
        if (unionType.UseStructLayout)
        {
            yield return VariantConstant(variant);
        }

        yield return Field(unionType, variant);
        yield return IsProperty(unionType, variant);

        yield return unionType.UseStructLayout
            ? AsPropertyWithStructLayout(unionType, variant)
            : AsProperty(variant);

        yield return Ctor(unionType, variant);
        if (!variant.IsInterface)
        {
            yield return ImplicitOperatorToUnion(unionType, variant);
            yield return ExplicitOperatorFromUnion(unionType, variant);
        }

        yield return TryGetMethod(unionType, variant);
    }

    private static MemberDeclarationSyntax VariantConstant(UnionTypeVariant variant)
    {
        return FieldDeclaration(
            VariableDeclaration(PredefinedType(Token(SyntaxKind.IntKeyword)))
                .AddVariables(
                    VariableDeclarator(
                        Identifier(variant.IdConstName),
                        null,
                        EqualsValueClause(
                            NumericLiteral(variant.IdConstValue)
                        )
                    )
                )
        ).AddModifiers(
            Token(SyntaxKind.PrivateKeyword),
            Token(SyntaxKind.ConstKeyword)
        );
    }

    private static FieldDeclarationSyntax Field(UnionType unionType, UnionTypeVariant variant)
    {
        TypeSyntax fieldType = IdentifierName(variant.TypeFullName)
            .NullableTypeWhen(!unionType.UseStructLayout);

        var field = FieldDeclaration(
                VariableDeclaration(fieldType)
                    .AddVariables(VariableDeclarator(Identifier(variant.FieldName)))
            )
            .AddModifiers(
                Token(SyntaxKind.PrivateKeyword),
                Token(SyntaxKind.ReadOnlyKeyword)
            ).AddAttributeListsWhen(
                unionType.UseStructLayout,
                AttributeList()
                    .AddAttributes(
                        FieldOffsetAttribute(4)
                    ));
        return field;
    }

    private static PropertyDeclarationSyntax IsProperty(UnionType unionType, UnionTypeVariant variant)
    {
        var condition = IsPropertyCondition(unionType, variant);

        return PropertyDeclaration(IdentifierName("bool"), Identifier(variant.IsPropertyName))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .WithExpressionBody(ArrowExpressionClause(condition))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static BinaryExpressionSyntax IsPropertyCondition(
        UnionType unionType,
        UnionTypeVariant variant,
        string? memberName = null)
    {
        return unionType.UseStructLayout
            ? BinaryExpression(
                SyntaxKind.EqualsExpression,
                memberName == null
                    ? IdentifierName(VariantIdFieldName)
                    : MemberAccess(memberName, VariantIdFieldName),
                IdentifierName(variant.IdConstName))
            : BinaryExpression(SyntaxKind.NotEqualsExpression,
                IdentifierName(variant.FieldName),
                LiteralExpression(SyntaxKind.NullLiteralExpression));
    }

    private static PropertyDeclarationSyntax AsProperty(UnionTypeVariant variant)
    {
        return PropertyDeclaration(IdentifierName(variant.TypeFullName), Identifier(variant.AsPropertyName))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .WithExpressionBody(AsPropertyBody(variant, null))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static ArrowExpressionClauseSyntax AsPropertyBody(UnionTypeVariant variant, string? memberName)
    {
        ExpressionSyntax left = memberName == null
            ? IdentifierName(variant.FieldName)
            : MemberAccess(memberName, variant.FieldName);

        return ArrowExpressionClause(
            BinaryExpression(SyntaxKind.CoalesceExpression,
                left,
                ThrowExpression(
                    NewInvalidOperationException($"Inner value is not {variant.Alias}")
                )
            ));
    }

    private static PropertyDeclarationSyntax AsPropertyWithStructLayout(UnionType unionType, UnionTypeVariant variant)
    {
        return PropertyDeclaration(IdentifierName(variant.TypeFullName), Identifier(variant.AsPropertyName))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddAccessorListAccessors(
                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .AddBodyStatements(
                        AsPropertyWithStructLayoutBody(unionType, variant, null)
                    )
            );
    }

    private static StatementSyntax[] AsPropertyWithStructLayoutBody(
        UnionType unionType,
        UnionTypeVariant variant,
        string? memberName)
    {
        ExpressionSyntax returnSyntax = memberName == null
            ? IdentifierName(variant.FieldName)
            : MemberAccess(memberName, variant.FieldName);

        return new StatementSyntax[]
        {
            IfStatement(
                IsPropertyCondition(unionType, variant, memberName),
                ReturnStatement(returnSyntax)
            ),
            ThrowStatement(
                NewInvalidOperationException(
                    $"Inner value is not {variant.Alias}"
                )
            )
        };
    }

    private static MemberDeclarationSyntax Ctor(UnionType unionType, UnionTypeVariant variant)
    {
        InvocationExpressionSyntax checkArgumentExpression = InvocationExpression(
                MemberAccess("System.ArgumentNullException", "ThrowIfNull")
            )
            .AddArgumentListArguments(
                Argument(IdentifierName(variant.ParameterName))
            );

        AssignmentExpressionSyntax assignmentExpression = AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            IdentifierName(variant.FieldName),
            IdentifierName(variant.ParameterName));

        return ConstructorDeclaration(Identifier(unionType.Name))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(
                Parameter(Identifier(variant.ParameterName))
                    .WithType(IdentifierName(variant.TypeFullName))
            )
            .AddBodyStatementsWhen(!unionType.UseStructLayout, ExpressionStatement(checkArgumentExpression))
            .AddBodyStatementsWhen(
                unionType.UseStructLayout,
                ExpressionStatement(AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(VariantIdFieldName),
                    IdentifierName(variant.IdConstName))))
            .AddBodyStatements(
                ExpressionStatement(assignmentExpression)
            );
    }

    private static ConversionOperatorDeclarationSyntax ImplicitOperatorToUnion(UnionType unionType,
        UnionTypeVariant variant)
    {
        return ConversionOperatorDeclaration(
                Token(SyntaxKind.ImplicitKeyword),
                IdentifierName(unionType.Name)
            )
            .AddModifiers(
                Token(SyntaxKind.PublicKeyword),
                Token(SyntaxKind.StaticKeyword)
            )
            .AddParameterListParameters(
                Parameter(Identifier(variant.ParameterName))
                    .WithType(IdentifierName(variant.TypeFullName))
            )
            .WithExpressionBody(
                ArrowExpressionClause(
                    ObjectCreationExpression(IdentifierName(unionType.Name))
                        .AddArgumentListArguments(
                            Argument(IdentifierName(variant.ParameterName))
                        )
                ))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static MemberDeclarationSyntax ExplicitOperatorFromUnion(UnionType unionType, UnionTypeVariant variant)
    {
        ParameterSyntax parameter =
            Parameter(Identifier("value"))
                .WithType(IdentifierName(unionType.Name));

        var operatorDeclaration = ConversionOperatorDeclaration(
                Token(SyntaxKind.ExplicitKeyword),
                IdentifierName(variant.TypeFullName)
            )
            .AddModifiers(
                Token(SyntaxKind.PublicKeyword),
                Token(SyntaxKind.StaticKeyword)
            )
            .AddParameterListParameters(parameter);

        return unionType.UseStructLayout
            ? operatorDeclaration
                .AddBodyStatements(AsPropertyWithStructLayoutBody(unionType, variant, "value"))
            : operatorDeclaration
                .WithExpressionBody(AsPropertyBody(variant, "value"))
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static MemberDeclarationSyntax TryGetMethod(UnionType unionType, UnionTypeVariant variant)
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
            ).AddBodyStatements(
                IfStatement(
                        IsPropertyCondition(unionType, variant),
                        Block(
                            ExpressionStatement(
                                AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                    IdentifierName("value"),
                                    IdentifierName(variant.FieldName)
                                )
                            ),
                            ReturnTrue()
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
                            ReturnFalse()
                        )))
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
                    VariantsBodyStatements(unionType, v => MatchStatement(unionType, v, isAsync))
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

    private static StatementSyntax MatchStatement(UnionType unionType, UnionTypeVariant variant, bool isAsync)
    {
        var argumentExpression = NotNullableArgumentExpression(unionType, variant);

        return IfStatement(
            IsPropertyCondition(unionType, variant),
            ReturnStatement(
                InvocationExpression(IdentifierName($"match{variant.Alias}"))
                    .AddArgumentListArguments(
                        Argument(argumentExpression)
                    )
                    .AddArgumentListArgumentWhen(isAsync, Argument(IdentifierName("ct")))
                    .AwaitWithConfigureAwaitWhen(isAsync)
            )
        );
    }

    private static ExpressionSyntax NotNullableArgumentExpression(UnionType unionType, UnionTypeVariant variant)
    {
        return unionType.UseStructLayout
            ? IdentifierName(variant.FieldName)
            : variant.IsValueType
                ? MemberAccess(variant.FieldName, "Value")
                : PostfixUnaryExpression(SyntaxKind.SuppressNullableWarningExpression,
                    IdentifierName(variant.FieldName));
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
                    VariantsBodyStatements(unionType, v => SwitchStatement(unionType, v, isAsync))
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

    private static StatementSyntax SwitchStatement(UnionType unionType, UnionTypeVariant variant, bool isAsync)
    {
        var argumentExpression = NotNullableArgumentExpression(unionType, variant);

        return IfStatement(
            IsPropertyCondition(unionType, variant),
            Block(
                ExpressionStatement(
                    InvocationExpression(IdentifierName($"switch{variant.Alias}"))
                        .AddArgumentListArguments(
                            Argument(argumentExpression)
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
                            VariantsBodyStatements(unionType, v => TypeStatement(unionType, v))
                        )
                    )
                );
    }

    private static StatementSyntax TypeStatement(UnionType unionType, UnionTypeVariant variant)
    {
        return IfStatement(
            IsPropertyCondition(unionType, variant),
            ReturnStatement(
                TypeOfExpression(
                    IdentifierName(variant.TypeFullName)
                )
            )
        );
    }

    private static MemberDeclarationSyntax ToStringMethod(UnionType unionType)
    {
        return MethodDeclaration(
                IdentifierName("string"),
                Identifier("ToString")
            )
            .AddModifiers(
                Token(SyntaxKind.PublicKeyword),
                Token(SyntaxKind.OverrideKeyword)
            )
            .AddBodyStatements(
                VariantsBodyStatements(unionType, v => AliasStatement(unionType, v))
            );

        static StatementSyntax AliasStatement(UnionType unionType, UnionTypeVariant variant)
        {
            return IfStatement(
                IsPropertyCondition(unionType, variant),
                ReturnStatement(
                    InvocationExpression(
                        MemberAccess(variant.FieldName, "ToString")
                    )
                )
            );
        }
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
            NewInvalidOperationException("Inner type is unknown")
        );
    }

    #endregion
}