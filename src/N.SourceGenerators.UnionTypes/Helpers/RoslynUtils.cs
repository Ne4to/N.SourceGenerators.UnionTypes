namespace N.SourceGenerators.UnionTypes.Helpers;

internal static class RoslynUtils
{
    public const string FuncType = "global::System.Func";
    public const string ActionType = "global::System.Action";
    private const string TaskTypeName = "global::System.Threading.Tasks.Task";

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

    public static TypeSyntax TaskType()
    {
        return IdentifierName(TaskTypeName);
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

    public static ArgumentSyntax Argument(string name)
    {
        return SyntaxFactory.Argument(IdentifierName(name));
    }

    public static ThrowStatementSyntax ThrowInvalidOperationException(string message)
    {
        var expression = StringLiteral(message);
        return ThrowInvalidOperationException(expression);
    }

    public static ThrowStatementSyntax ThrowInvalidOperationException(ExpressionSyntax expression)
    {
        return ThrowException(
            "System.InvalidOperationException",
            SyntaxFactory.Argument(expression)
        );
    }

    public static ThrowStatementSyntax ThrowException(string type, params ArgumentSyntax[] arguments)
    {
        return ThrowException(IdentifierName(type), arguments);
    }

    private static ThrowStatementSyntax ThrowException(TypeSyntax type, params ArgumentSyntax[] arguments)
    {
        return ThrowStatement(
            ObjectCreationExpression(
                    type
                )
                .AddArgumentListArguments(
                    arguments
                )
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
        return GenericType(TaskTypeName, type);
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

    public static MemberAccessExpressionSyntax MemberAccess(
        ExpressionSyntax expression,
        string name,
        params string[] pathItems)
    {
        var result = MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            expression,
            IdentifierName(name)
        );

        foreach (var path in pathItems)
        {
            result = MemberAccess(result, path);
        }

        return result;
    }

    public static ReturnStatementSyntax ReturnTrue()
    {
        return ReturnStatement(TrueLiteralExpression());
    }

    public static LiteralExpressionSyntax TrueLiteralExpression()
    {
        return LiteralExpression(SyntaxKind.TrueLiteralExpression);
    }

    public static ReturnStatementSyntax ReturnFalse()
    {
        return ReturnStatement(FalseLiteralExpression());
    }

    public static LiteralExpressionSyntax FalseLiteralExpression()
    {
        return LiteralExpression(SyntaxKind.FalseLiteralExpression);
    }

    public static LiteralExpressionSyntax Utf8StringLiteral(string value)
    {
        return LiteralExpression(
            SyntaxKind.Utf8StringLiteralExpression,
            ParseToken($"\"{value}\"u8")
        );
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
                Argument(varName)
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

    public static LocalDeclarationStatementSyntax LocalVariableDeclaration(
        TypeSyntax type,
        string name,
        ExpressionSyntax initExpression)
    {
        return LocalDeclarationStatement(
            VariableDeclaration(
                type
            ).AddVariables(
                VariableDeclarator(name)
                    .WithInitializer(
                        EqualsValueClause(
                            initExpression
                        )
                    )
            )
        );
    }
}