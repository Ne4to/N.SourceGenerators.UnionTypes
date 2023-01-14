using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using N.SourceGenerators.UnionTypes.Extensions;
using N.SourceGenerators.UnionTypes.Models;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace N.SourceGenerators.UnionTypes;

[Generator(LanguageNames.CSharp)]
public class UnionTypesGenerator : IIncrementalGenerator
{
    // TODO add #if NET7_0_OR_GREATER
    // #if NET7_0_OR_GREATER
    // // class A()
    // #else
    // // class A()
    // #endif   
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

    private const string UnionTypeAttributeName = "N.SourceGenerators.UnionTypes.UnionTypeAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource("UnionTypeAttribute.g.cs", SourceText.From(UnionTypeAttributeText, Encoding.UTF8));
        });

        // Do a simple filter for classes
        IncrementalValuesProvider<UnionType> classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (s, _) => s.IsClassWithAttributes(),
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

                    bool isPartial = ((ClassDeclarationSyntax)ctx.Node).Modifiers.Any(SyntaxKind.PartialKeyword);

                    List<UnionTypeVariant>? variants = null;
                    foreach (AttributeData attribute in symbol.GetAttributes())
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

                    UnionType unionType = new(symbol, variants.OrderBy(t => t.Order).ToArray());
                    return unionType;
                })
            .Where(static m => m is not null)!;

        // Combine the selected enums with the `Compilation`
        IncrementalValueProvider<(Compilation, ImmutableArray<UnionType>)> compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterImplementationSourceOutput(compilationAndClasses, static (context, item) =>
        {
            Execute(item.Item1, item.Item2, context);
        });
    }

    static void Execute(
        Compilation compilation,
        ImmutableArray<UnionType> classes,
        SourceProductionContext context)
    {
        if (classes.IsDefaultOrEmpty)
        {
            // nothing to do yet
            return;
        }

        foreach (UnionType s in classes)
        {
            // TODO add property `public Type ValueType { get; }`
            ClassDeclarationSyntax classDeclarationSyntax = ClassDeclaration(s.ContainerType.Name)
                .AddModifiers(Token(SyntaxKind.PartialKeyword));

            foreach (UnionTypeVariant variant in s.Variants)
            {
                classDeclarationSyntax = classDeclarationSyntax.AddMembers(
                    Field(variant),
                    IsProperty(variant),
                    AsProperty(variant),
                    Ctor(s, variant),
                    ImplicitOperatorToUnion(s, variant));
            }

            SyntaxTriviaList syntaxTriviaList = TriviaList(
                Comment("// <auto-generated/>"),
                Trivia(PragmaWarningDirectiveTrivia(Token(SyntaxKind.DisableKeyword), true)),
                Trivia(NullableDirectiveTrivia(Token(SyntaxKind.EnableKeyword), true)));

            CompilationUnitSyntax compilationUnit;
            if (s.ContainerType.ContainingNamespace.Name is "")
            {
                compilationUnit = CompilationUnit()
                    .AddMembers(classDeclarationSyntax.WithLeadingTrivia(syntaxTriviaList))
                    .NormalizeWhitespace();
            }
            else
            {
                compilationUnit = CompilationUnit()
                    .AddMembers(
                        NamespaceDeclaration(IdentifierName(s.ContainerType.ContainingNamespace.Name))
                            .WithLeadingTrivia(syntaxTriviaList)
                            .AddMembers(classDeclarationSyntax)
                    )
                    .NormalizeWhitespace();
            }

            context.AddSource($"{s.ContainerType.Name}.g.cs", compilationUnit.GetText(Encoding.UTF8));
        }
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
        ArgumentSyntax argument = Argument(
            LiteralExpression(
                SyntaxKind.StringLiteralExpression,
                Literal($"This is not a {variant.TypeSymbol.Name}")
            ));

        return PropertyDeclaration(IdentifierName(variant.TypeFullName), Identifier(variant.AsPropertyName))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .WithExpressionBody(ArrowExpressionClause(
                BinaryExpression(SyntaxKind.CoalesceExpression,
                    IdentifierName(variant.FieldName),
                    ThrowExpression(
                        ObjectCreationExpression(
                            IdentifierName("System.InvalidOperationException"),
                            ArgumentList(new SeparatedSyntaxList<ArgumentSyntax>().Add(argument)),
                            null
                        )
                    )
                )))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static MemberDeclarationSyntax Ctor(UnionType unionType, UnionTypeVariant variant)
    {
        ParameterSyntax parameter = Parameter(Identifier(variant.Alias)).WithType(IdentifierName(variant.TypeFullName));
        ParameterListSyntax parameterList = ParameterList(new SeparatedSyntaxList<ParameterSyntax>().Add(parameter));

        var argument = Argument(IdentifierName(variant.Alias));

        InvocationExpressionSyntax checkArgumentExpression = InvocationExpression(
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName("System.ArgumentNullException"),
                IdentifierName("ThrowIfNull")),
            ArgumentList(new SeparatedSyntaxList<ArgumentSyntax>().Add(argument))
        );

        AssignmentExpressionSyntax assignmentExpression = AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            IdentifierName(variant.FieldName),
            IdentifierName(variant.Alias));

        BlockSyntax body =
            Block(new StatementSyntax[]
            {
                ExpressionStatement(checkArgumentExpression), 
                ExpressionStatement(assignmentExpression)
            });

        return ConstructorDeclaration(Identifier(unionType.ContainerType.Name))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .WithParameterList(parameterList)
            .WithBody(body);
    }

    private static ConversionOperatorDeclarationSyntax ImplicitOperatorToUnion(UnionType unionType, UnionTypeVariant variant)
    {
        ParameterSyntax parameter = Parameter(Identifier(variant.Alias)).WithType(IdentifierName(variant.TypeFullName));
        ParameterListSyntax parameterList = ParameterList(new SeparatedSyntaxList<ParameterSyntax>().Add(parameter));
        
        ArgumentSyntax argument = Argument(IdentifierName(variant.Alias));
        
        return ConversionOperatorDeclaration(
                Token(SyntaxKind.ImplicitKeyword),
                IdentifierName(unionType.ContainerType.Name)
                )
            .AddModifiers(
                Token(SyntaxKind.PublicKeyword),
                Token(SyntaxKind.StaticKeyword)
            )
            .WithParameterList(parameterList)
            .WithExpressionBody(
                ArrowExpressionClause(
                    ObjectCreationExpression(IdentifierName(unionType.ContainerType.Name),
                        ArgumentList(new SeparatedSyntaxList<ArgumentSyntax>().Add(argument)),
                        null)
                    ))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }
}