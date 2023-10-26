using N.SourceGenerators.UnionTypes.Extensions;
using N.SourceGenerators.UnionTypes.Models;

namespace N.SourceGenerators.UnionTypes;

public partial class UnionTypesGenerator
{
    private static MemberDeclarationSyntax GetHashCodeMethod(UnionType unionType)
    {
        return MethodDeclaration(
            IdentifierName("int"),
            Identifier("GetHashCode")
        ).AddModifiers(
            Token(SyntaxKind.PublicKeyword),
            Token(SyntaxKind.OverrideKeyword)
        ).AddBodyStatements(
            VariantsBodyStatements(unionType, AliasStatement)
        );

        static StatementSyntax AliasStatement(UnionTypeVariant variant)
        {
            return IfStatement(
                IsPropertyCondition(variant),
                ReturnStatement(
                    InvocationExpression(
                        MemberAccess(variant.FieldName, "GetHashCode")
                    )
                )
            );
        }
    }

    private static OperatorDeclarationSyntax EqualsOperator(UnionType unionType, bool equal)
    {
        TypeSyntax parameterType = IdentifierName(unionType.Name)
            .NullableTypeWhen(unionType.IsReferenceType);

        ExpressionSyntax equalExpression = unionType.IsReferenceType
            ? InvocationExpression(
                IdentifierName("Equals")
            ).AddArgumentListArguments(
                Argument("left"),
                Argument("right")
            )
            : InvocationExpression(
                MemberAccess("left", "Equals")
            ).AddArgumentListArguments(
                Argument("right")
            );

        equalExpression = equalExpression.LogicalNotWhen(!equal);

        return OperatorDeclaration(
                IdentifierName("bool"),
                equal ? Token(SyntaxKind.EqualsEqualsToken) : Token(SyntaxKind.ExclamationEqualsToken)
            ).AddModifiers(
                Token(SyntaxKind.PublicKeyword),
                Token(SyntaxKind.StaticKeyword)
            ).AddParameterListParameters(
                Parameter(parameterType, "left"),
                Parameter(parameterType, "right")
            )
            .AddBodyStatements(
                ReturnStatement(
                    equalExpression
                )
            );
    }

    private static MemberDeclarationSyntax GenericEqualsMethod(UnionType unionType)
    {
        bool isReferenceType = unionType.IsReferenceType;

        return MethodDeclaration(
            IdentifierName("bool"),
            Identifier("Equals")
        ).AddModifiers(
            Token(SyntaxKind.PublicKeyword)
        ).AddParameterListParameters(
            Parameter(Identifier("other"))
                .WithType(
                    IdentifierName(unionType.Name)
                        .NullableTypeWhen(isReferenceType)
                )
        ).AddBodyStatements(
            BodyStatements().ToArray()
        );

        IEnumerable<StatementSyntax> BodyStatements()
        {
            if (isReferenceType)
            {
                yield return ReferenceEqualsNullOther();
                yield return ReferenceEqualsThisOther();
            }

            yield return IfStatement(
                NotEqualsExpression(
                    IdentifierName("ValueType"),
                    MemberAccess("other", "ValueType")
                ),
                Block(
                    ReturnFalse()
                )
            );

            foreach (UnionTypeVariant variant in unionType.Variants)
            {
                MemberAccessExpressionSyntax memberAccess = MemberAccess("other", variant.FieldName);

                yield return IfStatement(
                    IsPropertyCondition(variant),
                    ReturnStatement(
                        InvocationExpression(
                            MemberAccess(
                                GenericType("System.Collections.Generic.EqualityComparer", variant.TypeFullName),
                                "Default",
                                "Equals"
                            )
                        ).AddArgumentListArguments(
                            Argument(NotNullableArgumentExpression(variant)),
                            Argument(memberAccess)
                        )
                    )
                );
            }

            yield return ThrowUnknownType();
        }
    }

    private static MemberDeclarationSyntax ClassEqualsMethod(UnionType unionType)
    {
        return MethodDeclaration(
            IdentifierName("bool"),
            Identifier("Equals")
        ).AddModifiers(
            Token(SyntaxKind.PublicKeyword),
            Token(SyntaxKind.OverrideKeyword)
        ).AddParameterListParameters(
            Parameter(NullableType(ObjectType()), "other")
        ).AddBodyStatements(
            BodyStatements().ToArray()
        );

        IEnumerable<StatementSyntax> BodyStatements()
        {
            yield return ReferenceEqualsNullOther();
            yield return ReferenceEqualsThisOther();

            yield return IfStatement(
                NotEqualsExpression(
                    InvocationExpression(MemberAccess("other", "GetType")),
                    TypeOfExpression(IdentifierName(unionType.Name))
                )
                ,
                Block(
                    ReturnFalse()
                )
            );

            yield return ReturnStatement(
                InvocationExpression(
                    IdentifierName("Equals")
                ).AddArgumentListArguments(
                    Argument(
                        CastExpression(
                            IdentifierName(unionType.Name),
                            IdentifierName("other")
                        )
                    )
                )
            );
        }
    }

    private static MemberDeclarationSyntax StructEqualsMethod(UnionType unionType)
    {
        return MethodDeclaration(
            IdentifierName("bool"),
            Identifier("Equals")
        ).AddModifiers(
            Token(SyntaxKind.PublicKeyword),
            Token(SyntaxKind.OverrideKeyword)
        ).AddParameterListParameters(
            Parameter(NullableType(ObjectType()), "obj")
        ).AddBodyStatements(
            ReturnStatement(
                BinaryExpression(
                    SyntaxKind.LogicalAndExpression,
                    IsPatternExpression(
                        IdentifierName("obj"),
                        DeclarationPattern(IdentifierName(unionType.Name),
                            SingleVariableDesignation(Identifier("other")))
                    ),
                    InvocationExpression(IdentifierName("Equals"))
                        .AddArgumentListArguments(
                            Argument("other")
                        )
                )
            )
        );
    }

    private static IfStatementSyntax ReferenceEqualsThisOther()
    {
        return IfStatement(
            InvocationExpression(
                IdentifierName("ReferenceEquals")
            ).AddArgumentListArguments(
                Argument(ThisExpression()),
                Argument("other")
            ),
            Block(
                ReturnTrue()
            )
        );
    }

    private static IfStatementSyntax ReferenceEqualsNullOther()
    {
        return IfStatement(
            InvocationExpression(
                IdentifierName("ReferenceEquals")
            ).AddArgumentListArguments(
                Argument(LiteralExpression(SyntaxKind.NullLiteralExpression)),
                Argument("other")
            ),
            Block(
                ReturnFalse()
            )
        );
    }
}