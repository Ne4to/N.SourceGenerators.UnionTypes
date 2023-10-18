﻿namespace N.SourceGenerators.UnionTypes.Extensions;

internal static class SyntaxNodeExtensions
{
    public static bool IsTypeWithAttributes(this SyntaxNode s)
    {
        return s is TypeDeclarationSyntax
        {
            AttributeLists.Count: > 0
        };
    }
    
    public static bool IsGenericTypeAttribute(this SyntaxNode s)
    {
        if (s is not TypeParameterSyntax typeParameter)
        {
            return false;
        }

        if (typeParameter.Parent is not TypeParameterListSyntax typeParameterList)
        {
            return false;
        }

        return typeParameterList.Parent is TypeDeclarationSyntax;
    }

    public static bool IsPartial(this TypeDeclarationSyntax s)
    {
        return s.Modifiers.Any(SyntaxKind.PartialKeyword);
    }
    
    public static AwaitExpressionSyntax AwaitWithConfigureAwait(this ExpressionSyntax expression)
    {
        return AwaitExpression(
            InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    expression,
                    IdentifierName("ConfigureAwait")
                )
            ).AddArgumentListArguments(
                Argument(LiteralExpression(SyntaxKind.FalseLiteralExpression))
            )
        );
    }
}