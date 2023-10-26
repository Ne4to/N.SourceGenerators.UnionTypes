namespace N.SourceGenerators.UnionTypes.Helpers;

internal static class RoslynUtils
{
    public const string FuncType = "global::System.Func";
    public const string ActionType = "global::System.Action";
    public const string TaskType = "global::System.Threading.Tasks.Task";

    public static PredefinedTypeSyntax VoidType()
    {
        return PredefinedType(Token(SyntaxKind.VoidKeyword));
    }

    public static PredefinedTypeSyntax ObjectType()
    {
        return PredefinedType(Token(SyntaxKind.ObjectKeyword));
    }

    public static PredefinedTypeSyntax StringType()
    {
        return PredefinedType(Token(SyntaxKind.StringKeyword));
    }

    public static TypeSyntax CancellationTokenType()
    {
        return IdentifierName("global::System.Threading.CancellationToken");
    }

    public static TypeSyntax DefaultJsonTypeInfoResolverType()
    {
        return IdentifierName("System.Text.Json.Serialization.Metadata.DefaultJsonTypeInfoResolver");
    }

    public static TypeSyntax JsonPropertyInfoType()
    {
        return IdentifierName("System.Text.Json.Serialization.Metadata.JsonPropertyInfo");
    }

    public static ParameterSyntax Parameter(string type, string name)
    {
        return Parameter(IdentifierName(type), name);
    }

    public static ParameterSyntax Parameter(TypeSyntax type, string name)
    {
        return SyntaxFactory.Parameter(Identifier(name))
            .WithType(type);
    }

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

    public static MemberAccessExpressionSyntax MemberAccess(
        string expression,
        string name,
        params string[] pathItems)
    {
        var result = MemberAccess(IdentifierName(expression), name);

        foreach (var path in pathItems)
        {
            result = MemberAccess(result, path);
        }

        return result;
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

    public static BinaryExpressionSyntax IsExpression(string varName, ExpressionSyntax right)
    {
        return BinaryExpression(
            SyntaxKind.IsExpression,
            IdentifierName(varName),
            right
        );
    }

    public static BinaryExpressionSyntax IsExpression(ExpressionSyntax left, ExpressionSyntax right)
    {
        return BinaryExpression(
            SyntaxKind.IsExpression,
            left,
            right
        );
    }

    public static BinaryExpressionSyntax NotEqualsExpression(ExpressionSyntax left, ExpressionSyntax right)
    {
        return BinaryExpression(
            SyntaxKind.NotEqualsExpression,
            left,
            right
        );
    }

    public static InvocationExpressionSyntax NameOfExpressions(string varName)
    {
        return InvocationExpression(IdentifierName("nameof"))
            .AddArgumentListArguments(
                Argument(IdentifierName(varName))
            );
    }

    public static InitializerExpressionSyntax ObjectInitializerExpression(params ExpressionSyntax[] items)
    {
        return InitializerExpression(
            SyntaxKind.ObjectInitializerExpression
        ).AddExpressions(
            items
        );
    }

    public static InitializerExpressionSyntax CollectionInitializerExpression(params ExpressionSyntax[] items)
    {
        return InitializerExpression(
            SyntaxKind.CollectionInitializerExpression
        ).AddExpressions(
            items
        );
    }

    public static AssignmentExpressionSyntax SimpleAssignmentExpression(string left, string right)
    {
        return AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            IdentifierName(left),
            IdentifierName(right)
        );
    }

    public static AssignmentExpressionSyntax SimpleAssignmentExpression(string left, ExpressionSyntax right)
    {
        return AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            IdentifierName(left),
            right
        );
    }

    public static AssignmentExpressionSyntax SimpleAssignmentExpression(ExpressionSyntax left, ExpressionSyntax right)
    {
        return AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            left,
            right
        );
    }
}