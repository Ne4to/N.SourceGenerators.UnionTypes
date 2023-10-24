namespace N.SourceGenerators.UnionTypes.Helpers;

internal static class RoslynUtils
{
    public const string FuncType = "global::System.Func";
    public const string ActionType = "global::System.Action";
    public const string TaskType = "global::System.Threading.Tasks.Task";
    public const string CancellationTokenType = "global::System.Threading.CancellationToken";

    private static ObjectCreationExpressionSyntax NewInvalidOperationException(ExpressionSyntax expression)
    {
        return ObjectCreationExpression(
                IdentifierName("System.InvalidOperationException")
            )
            .AddArgumentListArguments(
                Argument(expression)
            );
    }

    public static ObjectCreationExpressionSyntax NewInvalidOperationException(string message)
    {
        var expression = StringLiteral(message);
        return NewInvalidOperationException(expression);
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

    public static MemberAccessExpressionSyntax MemberAccess(string expression, string name)
    {
        return MemberAccess(IdentifierName(expression), name);
    }
    
    public static MemberAccessExpressionSyntax MemberAccess(ExpressionSyntax expression, string name)
    {
        return MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            expression,
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


    public static LiteralExpressionSyntax StringLiteral(string message)
    {
        return LiteralExpression(
            SyntaxKind.StringLiteralExpression,
            Literal(message)
        );
    }

    public static LiteralExpressionSyntax NumericLiteral(int value)
    {
        return LiteralExpression(
            SyntaxKind.NumericLiteralExpression,
            Literal(value)
        );
    }

    public static InterpolatedStringExpressionSyntax InterpolatedString(params InterpolatedStringContentSyntax[] items)
    {
        return InterpolatedStringExpression(
            Token(SyntaxKind.InterpolatedStringStartToken)
        ).AddContents(items);
    }

    public static InterpolatedStringTextSyntax InterpolatedText(string value)
    {
        return InterpolatedStringText(
            Token(
                SyntaxTriviaList.Empty,
                SyntaxKind.InterpolatedStringTextToken,
                value,
                value,
                SyntaxTriviaList.Empty
            )
        );
    }
}