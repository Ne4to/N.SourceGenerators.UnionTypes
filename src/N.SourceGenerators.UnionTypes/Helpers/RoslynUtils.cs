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

    public static GenericNameSyntax TaskIdentifier(string type)
    {
        return GenericName(TaskType)
            .AddTypeArgumentListArguments(
                IdentifierName(type)
            );
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
}