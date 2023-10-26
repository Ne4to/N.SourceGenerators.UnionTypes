using System.Collections.Immutable;
using System.Text;

using N.SourceGenerators.UnionTypes.Extensions;
using N.SourceGenerators.UnionTypes.Models;

namespace N.SourceGenerators.UnionTypes;

public sealed partial class UnionTypesGenerator
{
    private static void GenerateConverters(IncrementalGeneratorInitializationContext context)
    {
        var toConverters = GetUnionConverters(
            context,
            UnionConverterToAttributeName,
            (containerType, otherTypes) => new UnionToConverter(containerType, otherTypes));
        ProcessConverters(context, toConverters);

        var fromConverters = GetUnionConverters(
            context,
            UnionConverterFromAttributeName,
            (containerType, otherTypes) => new UnionFromConverter(containerType, otherTypes));
        ProcessConverters(context, fromConverters);

        var unionConverters = GetUnionConverters(context);
        ProcessConverters(context, unionConverters);
    }

    private static void ProcessConverters(
        IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<UnionConverter> unionConverters)
    {
        context.RegisterImplementationSourceOutput(
            unionConverters,
            (ctx, item) =>
            {
                TypeDeclarationSyntax typeDeclaration = ClassDeclaration(item.Name);

                typeDeclaration = typeDeclaration
                    .AddModifiersWhen(
                        item.IsStatic,
                        Token(SyntaxKind.StaticKeyword)
                    ).AddModifiers(Token(SyntaxKind.PartialKeyword));

                bool saveSource = false;
                foreach (var converter in item.Converters)
                {
                    UnionType fromType = converter.FromType;
                    UnionType toType = converter.ToType;

                    bool canConvert = CanConvert(fromType, toType, item.Location, out var diagnostic);

                    if (canConvert)
                    {
                        var method = MethodDeclaration(
                            IdentifierName(toType.TypeFullName),
                            Identifier(converter.MethodName)
                        ).AddModifiers(
                            Token(SyntaxKind.PublicKeyword),
                            Token(SyntaxKind.StaticKeyword)
                        ).AddParameterListParameters(
                            Parameter(Identifier("value"))
                                .WithType(IdentifierName(fromType.TypeFullName))
                                .AddModifiersWhen(item.IsStatic, Token(SyntaxKind.ThisKeyword))
                        ).AddBodyStatements(
                            ReturnMatchConversion(fromType, toType)
                        );

                        saveSource = true;
                        typeDeclaration = typeDeclaration.AddMembers(method);
                    }
                    else
                    {
                        ctx.ReportDiagnostic(diagnostic!);
                    }
                }

                if (saveSource)
                {
                    CompilationUnitSyntax compilationUnit = GetCompilationUnit(typeDeclaration, item.Namespace);
                    ctx.AddSource($"{item.Name}Converters.g.cs", compilationUnit.GetText(Encoding.UTF8));
                }
            });
    }

    private static IncrementalValuesProvider<T> GetUnionConverters<T>(
        IncrementalGeneratorInitializationContext context,
        string attributeName,
        Func<UnionType, ImmutableArray<UnionType>, T> valueFunc)
        where T : class
    {
        return context.SyntaxProvider
            .ForAttributeWithMetadataName(
                attributeName,
                static (s, _) => s.IsTypeWithAttributes(),
                (ctx, ct) =>
                {
                    ct.ThrowIfCancellationRequested();

                    TypeDeclarationSyntax typeDeclaration = (TypeDeclarationSyntax)ctx.TargetNode;
                    INamedTypeSymbol targetSymbol = (INamedTypeSymbol)ctx.TargetSymbol;
                    var containerType = GetUnionType(targetSymbol, typeDeclaration);
                    if (containerType == null)
                    {
                        return default;
                    }

                    var otherTypes = ImmutableArray<UnionType>.Empty;
                    foreach (AttributeData attribute in targetSymbol.GetAttributes())
                    {
                        if (attribute.AttributeClass?.ToDisplayString() != attributeName)
                        {
                            continue;
                        }

                        UnionType? fromType = GetUnionType(attribute.ConstructorArguments[0]);
                        if (fromType != null)
                        {
                            otherTypes = otherTypes.Add(fromType);
                        }
                    }

                    return otherTypes.Length != 0
                        ? valueFunc(containerType, otherTypes)
                        : default;
                })
            .Where(static u => u is not null)!;
    }

    private static IncrementalValuesProvider<UnionConverter> GetUnionConverters(
        IncrementalGeneratorInitializationContext context)
    {
        return context.SyntaxProvider
            .ForAttributeWithMetadataName(
                UnionConverterAttributeName,
                static (s, _) => s.IsTypeWithAttributes(),
                (ctx, ct) =>
                {
                    ct.ThrowIfCancellationRequested();

                    TypeDeclarationSyntax typeDeclaration = (TypeDeclarationSyntax)ctx.TargetNode;
                    INamedTypeSymbol targetSymbol = (INamedTypeSymbol)ctx.TargetSymbol;

                    var converters = ImmutableArray<StaticConverter>.Empty;
                    foreach (AttributeData attribute in targetSymbol.GetAttributes())
                    {
                        if (attribute.AttributeClass?.ToDisplayString() != UnionConverterAttributeName)
                        {
                            continue;
                        }

                        UnionType? fromType = GetUnionType(attribute.ConstructorArguments[0]);
                        UnionType? toType = GetUnionType(attribute.ConstructorArguments[1]);
                        var methodName = attribute.ConstructorArguments[2].Value?.ToString();

                        if (fromType != null && toType != null)
                        {
                            StaticConverter converter = new(fromType, toType, methodName);
                            converters = converters.Add(converter);
                        }
                    }

                    return converters.Length != 0
                        ? new UnionConverter(typeDeclaration, targetSymbol, converters)
                        : default;
                })
            .Where(static u => u is not null)!;
    }

    private static void ProcessConverters<T>(IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<T> fromConverters)
        where T : IUnionConverter
    {
        context.RegisterImplementationSourceOutput(
            fromConverters,
            (ctx, item) =>
            {
                UnionType containerType = item.ContainerType;
                TypeDeclarationSyntax typeDeclaration = containerType.IsReferenceType
                    ? ClassDeclaration(containerType.Name)
                    : StructDeclaration(containerType.Name);

                typeDeclaration = typeDeclaration
                    .AddModifiers(Token(SyntaxKind.PartialKeyword));

                bool saveSource = false;
                foreach ((UnionType fromType, UnionType toType) in item)
                {
                    bool canConvert = CanConvert(fromType, toType, containerType.Location, out var diagnostic);

                    if (canConvert)
                    {
                        typeDeclaration = ConverterOperator(typeDeclaration, fromType, toType);
                        saveSource = true;
                    }
                    else
                    {
                        ctx.ReportDiagnostic(diagnostic!);
                    }
                }

                if (saveSource)
                {
                    CompilationUnitSyntax compilationUnit =
                        GetCompilationUnit(typeDeclaration, containerType.Namespace);
                    ctx.AddSource(item.SourceHintName, compilationUnit.GetText(Encoding.UTF8));
                }
            });
    }

    private static bool CanConvert(
        UnionType fromType,
        UnionType toType,
        Location? location,
        out Diagnostic? diagnostic)
    {
        foreach (UnionTypeVariant fromVariant in fromType.Variants)
        {
            var canConvert = toType.Variants
                .Any(toVariant => fromVariant.TypeFullName == toVariant.TypeFullName);

            if (canConvert)
            {
                continue;
            }

            diagnostic = Diagnostic.Create(
                CantConvertWarning,
                location,
                fromType.Name,
                toType.Name,
                fromVariant.Alias
            );

            return false;
        }

        diagnostic = null;
        return true;
    }

    private static TypeDeclarationSyntax ConverterOperator(
        TypeDeclarationSyntax typeDeclaration,
        UnionType fromType,
        UnionType toType)
    {
        var member = ConversionOperatorDeclaration(
            Token(SyntaxKind.ImplicitKeyword),
            IdentifierName(toType.Name)
        ).AddModifiers(
            Token(SyntaxKind.PublicKeyword),
            Token(SyntaxKind.StaticKeyword)
        ).AddParameterListParameters(
            Parameter(Identifier("value"))
                .WithType(IdentifierName(fromType.TypeFullName))
        ).AddBodyStatements(
            ReturnMatchConversion(fromType, toType)
        );

        return typeDeclaration.AddMembers(member);
    }

    private static ReturnStatementSyntax ReturnMatchConversion(UnionType fromType, UnionType toType)
    {
        const string lambdaParamName = "x";

        var arguments = fromType
            .Variants
            .Select(variant => Argument(
                ParenthesizedLambdaExpression()
                    .AddParameterListParameters(
                        Parameter(variant.TypeFullName, lambdaParamName)
                    )

                    // SimpleLambdaExpression(
                    //     // BracketedParameterList(
                    //         // new SeparatedSyntaxList<ParameterSyntax>()
                    //         
                    //         Parameter(Identifier(lambdaParamName)).WithType(IdentifierName(variant.TypeFullName))
                    //         
                    //         
                    //         // )
                    //     
                    // )
                    .WithExpressionBody(
                        CastExpression(IdentifierName(toType.Name),
                            ObjectCreationExpression(
                                IdentifierName(toType.Name)
                            ).AddArgumentListArguments(
                                Argument(IdentifierName(lambdaParamName))
                            ))
                    )
            )).ToArray();

        return ReturnStatement(
            InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName("value"),
                    GenericName("Match")
                        .AddTypeArgumentListArguments(IdentifierName(toType.Name))
                )
            ).AddArgumentListArguments(arguments)
        );
    }
}