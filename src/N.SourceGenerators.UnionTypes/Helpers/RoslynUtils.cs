namespace N.SourceGenerators.UnionTypes.Helpers;

internal static class RoslynUtils
{
    public const string TaskType = "global::System.Threading.Tasks.Task";

    public static ObjectCreationExpressionSyntax NewInvalidOperationException(string message)
    {
        return ObjectCreationExpression(
                IdentifierName("InvalidOperationException")
            )
            .AddArgumentListArguments(
                StringLiteralArgument(message)
            );
    }

    public static GenericNameSyntax GenericType(string type, string t1)
    {
        return GenericName(type)
            .AddTypeArgumentListArguments(
                IdentifierName(t1)
            );
    }

    public static GenericNameSyntax TaskIdentifier(string type)
    {
        return GenericType(TaskType, type);
    }

    private static ArgumentSyntax StringLiteralArgument(string value)
    {
        ArgumentSyntax argument = Argument(
            LiteralExpression(
                SyntaxKind.StringLiteralExpression,
                Literal(value)
            ));

        return argument;
    }

    public static MemberAccessExpressionSyntax MemberAccess(string expression, string name)
    {
        return MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName(expression),
            IdentifierName(name)
        );
    }

    public static ReturnStatementSyntax ReturnTrue()
    {
        return ReturnStatement(LiteralExpression(SyntaxKind.TrueLiteralExpression));
    }
    
    public static ReturnStatementSyntax ReturnFalse()
    {
        return ReturnStatement(LiteralExpression(SyntaxKind.FalseLiteralExpression));
    }
}