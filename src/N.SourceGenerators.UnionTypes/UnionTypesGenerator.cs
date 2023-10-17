﻿using System.Collections.Immutable;
using System.Text;

using N.SourceGenerators.UnionTypes.Extensions;
using N.SourceGenerators.UnionTypes.Models;

namespace N.SourceGenerators.UnionTypes;

[Generator(LanguageNames.CSharp)]
public sealed partial class UnionTypesGenerator : IIncrementalGenerator
{
    private const string VariantIdFieldName = "_variantId";

    const string AutoGeneratedComment = 
"""
// <auto-generated>
//   This code was generated by https://github.com/Ne4to/N.SourceGenerators.UnionTypes
//   Feel free to open an issue
// </auto-generated>
""";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        AddAttributesSource(context);

        var unionTypes = GetUnionTypes(context);
        var genericUnionTypes = GetGenericUnionTypes(context);

        // TODO comparer does not work
        var uniqueUnionTypes = unionTypes.WithComparer(UnionTypeComparer.Instance).Collect()
            .Combine(genericUnionTypes.WithComparer(UnionTypeComparer.Instance).Collect())
            .SelectMany(ProcessCollections);
        
        var unionTypeDiagnostics = GetUnionTypeDiagnostics(uniqueUnionTypes);
        ProcessValues(context, unionTypeDiagnostics, GenerateUnionType);

        GenerateConverters(context);
    }

    private IEnumerable<UnionType> ProcessCollections((ImmutableArray<UnionType> Left, ImmutableArray<UnionType> Right) combination, CancellationToken arg2)
    {
        HashSet<string> processedIds = new();
        foreach (UnionType unionType in combination.Left)
        {
            if (processedIds.Add(unionType.TypeFullName))
            {
                yield return unionType;
            }
        }
        
        foreach (UnionType unionType in combination.Right)
        {
            if (processedIds.Add(unionType.TypeFullName))
            {
                yield return unionType;
            }
        }
    }

    private static UnionType? GetUnionType(TypedConstant typedConstant)
    {
        var fromTypeSymbol = typedConstant.Value as INamedTypeSymbol;
        TypeDeclarationSyntax? fromTypeDeclaration = null;
        if (fromTypeSymbol is { DeclaringSyntaxReferences.IsEmpty: false })
        {
            fromTypeDeclaration = fromTypeSymbol.DeclaringSyntaxReferences[0].GetSyntax() as TypeDeclarationSyntax;
        }

        return fromTypeSymbol != null
            ? GetUnionType(fromTypeSymbol, fromTypeDeclaration)
            : null;
    }

    private static IncrementalValuesProvider<UnionType> GetUnionTypes(IncrementalGeneratorInitializationContext context)
    {
        return context.SyntaxProvider
            .ForAttributeWithMetadataName(
                UnionTypeAttributeName,
                static (s, _) => s.IsTypeWithAttributes(),
                static (context, ct) =>
                {
                    ct.ThrowIfCancellationRequested();

                    TypeDeclarationSyntax typeDeclaration = (TypeDeclarationSyntax)context.TargetNode;
                    INamedTypeSymbol targetSymbol = (INamedTypeSymbol)context.TargetSymbol;

                    UnionType? unionType = GetUnionType(targetSymbol, typeDeclaration);
                    return unionType;
                })
            .Where(static u => u is not null)!;
    }

    private static IncrementalValuesProvider<UnionType> GetGenericUnionTypes(
        IncrementalGeneratorInitializationContext context)
    {
        return context.SyntaxProvider
            .ForAttributeWithMetadataName(
                GenericUnionTypeAttributeName,
                static (s, _) => s.IsGenericTypeAttribute(),
                static (context, ct) =>
                {
                    ct.ThrowIfCancellationRequested();

                    var typeParameter = (TypeParameterSyntax)context.TargetNode;
                    var typeParameterList = (TypeParameterListSyntax)typeParameter.Parent!;

                    TypeDeclarationSyntax typeDeclaration = (TypeDeclarationSyntax)typeParameterList.Parent!;
                    INamedTypeSymbol targetSymbol = ((ITypeParameterSymbol)context.TargetSymbol).ContainingType;

                    UnionType? unionType = GetUnionType(targetSymbol, typeDeclaration);
                    return unionType;
                })
            .Where(static u => u is not null)!;
    }

    private static UnionType? GetUnionType(INamedTypeSymbol targetSymbol, TypeDeclarationSyntax? typeDeclaration)
    {
        List<UnionTypeVariant>? variants = null;
        foreach (AttributeData attribute in targetSymbol.GetAttributes())
        {
            if (attribute.AttributeClass?.ToDisplayString() != UnionTypeAttributeName)
            {
                continue;
            }

            variants ??= new List<UnionTypeVariant>();

            var typeSymbol = attribute.ConstructorArguments[0].Value as ITypeSymbol;
            var alias = attribute.ConstructorArguments[1].Value?.ToString();
            var order = (int)attribute.ConstructorArguments[2].Value!;
            var allowNull = attribute.NamedArguments
                .Where(kvp => kvp.Key == "AllowNull")
                .Select(kvp => (bool)kvp.Value.Value!)
                .FirstOrDefault();

            variants.Add(new UnionTypeVariant(typeSymbol!, alias, order, allowNull));
        }

        int typeArgumentIndex = -100;
        foreach (ITypeSymbol typeArgument in targetSymbol.TypeArguments)
        {
            foreach (AttributeData attribute in typeArgument.GetAttributes())
            {
                if (attribute.AttributeClass?.ToDisplayString() != GenericUnionTypeAttributeName)
                {
                    continue;
                }
                
                variants ??= new List<UnionTypeVariant>();
                var alias = attribute.NamedArguments
                    .Where(kvp => kvp.Key == "Alias")
                    .Select(kvp => (string?)kvp.Value.Value!)
                    .FirstOrDefault();
                
                if (alias == null)
                {
                    alias = typeArgument.Name.StartsWith("T") && typeArgument.Name.Length > 1
                        ? typeArgument.Name.Substring(1)
                        : typeArgument.Name;
                }
                
                var allowNull = attribute.NamedArguments
                    .Where(kvp => kvp.Key == "AllowNull")
                    .Select(kvp => (bool)kvp.Value.Value!)
                    .FirstOrDefault();

                variants.Add(new UnionTypeVariant(typeArgument, alias, typeArgumentIndex, allowNull));
                typeArgumentIndex++;
            }
        }

        if (variants == null)
        {
            return default;
        }

        UnionTypeVariant[] sortedVariants = variants.OrderBy(t => t.Order).ToArray();

        UnionType unionType = new(
            targetSymbol,
            typeDeclaration,
            sortedVariants);

        return unionType;
    }

    static void GenerateUnionType(UnionType unionType,
        SourceProductionContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        TypeDeclarationSyntax typeDeclaration = unionType.IsReferenceType
            ? ClassDeclaration(unionType.Name)
            : StructDeclaration(unionType.Name);

        typeDeclaration = typeDeclaration
            .AddAttributeLists(GetTypeAttributes(unionType))
            .AddModifiers(Token(SyntaxKind.PartialKeyword))
            .AddMembers(VariantIdField(unionType.UseStructLayout))
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
                GenericEqualsMethod(unionType)
            )
            .AddMembersWhen(!unionType.HasToStringMethod, ToStringMethod(unionType))
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

        CompilationUnitSyntax compilationUnit = GetCompilationUnit(typeDeclaration, unionType.Namespace);
        context.AddSource($"{unionType.SourceCodeFileName}.g.cs", compilationUnit.GetText(Encoding.UTF8));
    }

    private static CompilationUnitSyntax GetCompilationUnit(TypeDeclarationSyntax typeDeclaration,
        string? typeNamespace)
    {
        SyntaxTriviaList syntaxTriviaList = GetTriviaList();

        CompilationUnitSyntax compilationUnit;
        if (typeNamespace is null)
        {
            compilationUnit = CompilationUnit()
                .AddMembers(typeDeclaration.WithLeadingTrivia(syntaxTriviaList))
                .NormalizeWhitespace();
        }
        else
        {
            compilationUnit = CompilationUnit()
                .AddMembers(
                    NamespaceDeclaration(IdentifierName(typeNamespace))
                        .WithLeadingTrivia(syntaxTriviaList)
                        .AddMembers(typeDeclaration)
                )
                .NormalizeWhitespace();
        }

        return compilationUnit;
    }

    private static SyntaxTriviaList GetTriviaList()
    {
        return TriviaList(
            Comment(AutoGeneratedComment),
            Trivia(PragmaWarningDirectiveTrivia(Token(SyntaxKind.DisableKeyword), true)),
            Trivia(NullableDirectiveTrivia(Token(SyntaxKind.EnableKeyword), true)));
    }

    private static void ProcessValues<T>(
        IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<ValueDiagnostics<T>> valueDiagnostics,
        Action<T, SourceProductionContext> processAction)
    {
        ReportDiagnostics(context, valueDiagnostics);

        var validValues = valueDiagnostics
            .Where(vd => vd.Diagnostics.Count == 0);

        context.RegisterImplementationSourceOutput(
            validValues,
            (ctx, item) => { processAction(item.Value, ctx); });
    }

    private static MemberDeclarationSyntax VariantIdField(bool useStructLayout)
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
            ).AddAttributeListsWhen(useStructLayout, attributes);
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
        yield return VariantConstant(variant);
        yield return Field(unionType, variant);
        yield return IsProperty(variant);
        yield return AsPropertyWithStructLayout(variant);
        yield return Ctor(unionType, variant);
        if (!variant.IsInterface)
        {
            yield return ImplicitOperatorToUnion(unionType, variant);
            yield return ExplicitOperatorFromUnion(unionType, variant);
        }

        yield return TryGetMethod(variant);
    }
    
    /// <code>
    /// private const int {Alias}Id = {IdConstValue};
    /// private const int SuccessId = 1;
    /// </code>
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
        TypeSyntax fieldType = IdentifierName(variant.TypeFullName);

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

    private static PropertyDeclarationSyntax IsProperty(UnionTypeVariant variant)
    {
        var condition = IsPropertyCondition(variant);

        return PropertyDeclaration(IdentifierName("bool"), Identifier(variant.IsPropertyName))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .WithExpressionBody(ArrowExpressionClause(condition))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static BinaryExpressionSyntax IsPropertyCondition(
        UnionTypeVariant variant,
        string? memberName = null)
    {
        return BinaryExpression(
            SyntaxKind.EqualsExpression,
            memberName == null
                ? IdentifierName(VariantIdFieldName)
                : MemberAccess(memberName, VariantIdFieldName),
            IdentifierName(variant.IdConstName));
    }

    private static PropertyDeclarationSyntax AsPropertyWithStructLayout(UnionTypeVariant variant)
    {
        return PropertyDeclaration(IdentifierName(variant.TypeFullName), Identifier(variant.AsPropertyName))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddAccessorListAccessors(
                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .AddBodyStatements(
                        AsPropertyWithStructLayoutBody(variant, null)
                    )
            );
    }

    private static StatementSyntax[] AsPropertyWithStructLayoutBody(UnionTypeVariant variant,
        string? memberName)
    {
        ExpressionSyntax returnSyntax = NotNullableArgumentExpression(variant, memberName);
        
        return new StatementSyntax[]
        {
            IfStatement(
                IsPropertyCondition(variant, memberName),
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

        return ConstructorDeclaration(Identifier(unionType.NameNoGenerics))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(
                Parameter(Identifier(variant.ParameterName))
                    .WithType(IdentifierName(variant.TypeFullName))
            )
            .AddBodyStatementsWhen(!unionType.UseStructLayout && !variant.AllowNull, ExpressionStatement(checkArgumentExpression))
            .AddBodyStatements(
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

        return operatorDeclaration
            .AddBodyStatements(AsPropertyWithStructLayoutBody(variant, "value"));
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
                    .WithType(IdentifierName(variant.TypeFullName))
                    .AddModifiers(Token(SyntaxKind.OutKeyword))
                    .AddAttributeLists(attributeList)
            ).AddBodyStatements(
                IfStatement(
                        IsPropertyCondition(variant),
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
        var argumentExpression = NotNullableArgumentExpression(variant);

        return IfStatement(
            IsPropertyCondition(variant),
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

    private static ExpressionSyntax NotNullableArgumentExpression(UnionTypeVariant variant, string? memberName = null)
    {
        return memberName == null 
            ? IdentifierName(variant.FieldName)
            : MemberAccess(memberName, variant.FieldName);
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
        var argumentExpression = NotNullableArgumentExpression(variant);

        return IfStatement(
            IsPropertyCondition(variant),
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
                            VariantsBodyStatements(unionType, v => TypeStatement(v))
                        )
                    )
                );
    }

    private static StatementSyntax TypeStatement(UnionTypeVariant variant)
    {
        return IfStatement(
            IsPropertyCondition(variant),
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
                VariantsBodyStatements(unionType, v => AliasStatement(v))
            );

        static StatementSyntax AliasStatement(UnionTypeVariant variant)
        {
            return IfStatement(
                IsPropertyCondition(variant),
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