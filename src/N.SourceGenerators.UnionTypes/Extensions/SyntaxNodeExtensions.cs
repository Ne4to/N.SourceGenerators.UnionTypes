namespace N.SourceGenerators.UnionTypes.Extensions;

internal static class SyntaxNodeExtensions
{
    public static bool IsClassWithAttributes(this SyntaxNode s)
    {
        return s is ClassDeclarationSyntax
        {
            AttributeLists.Count: > 0
        };
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