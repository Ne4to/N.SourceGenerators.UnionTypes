namespace N.SourceGenerators.UnionTypes.Extensions;

internal static class ConditionalSyntaxExtensions
{
    public static MethodDeclarationSyntax AddModifierWhen(
        this MethodDeclarationSyntax syntax,
        bool condition,
        SyntaxToken item)
    {
        return condition
            ? syntax.AddModifiers(item)
            : syntax;
    }

    public static MethodDeclarationSyntax AddParameterListParameterWhen(
        this MethodDeclarationSyntax syntax,
        bool condition,
        ParameterSyntax item)
    {
        return condition
            ? syntax.AddParameterListParameters(item)
            : syntax;
    }

    public static GenericNameSyntax AddTypeArgumentListArgumentsWhen(
        this GenericNameSyntax syntax,
        bool condition,
        TypeSyntax item)
    {
        return condition
            ? syntax.AddTypeArgumentListArguments(item)
            : syntax;
    }

    public static InvocationExpressionSyntax AddArgumentListArgumentWhen(
        this InvocationExpressionSyntax syntax,
        bool condition,
        ArgumentSyntax item)
    {
        return condition
            ? syntax.AddArgumentListArguments(item)
            : syntax;
    }

    public static ExpressionSyntax AwaitWithConfigureAwaitWhen(this ExpressionSyntax expression, bool condition)
    {
        return condition
            ? expression.AwaitWithConfigureAwait()
            : expression;
    }

    public static ExpressionSyntax LogicalNotWhen(this ExpressionSyntax expression, bool condition)
    {
        return condition
            ? PrefixUnaryExpression(
                SyntaxKind.LogicalNotExpression,
                expression
            )
            : expression;
    }

    public static TypeSyntax NullableTypeWhen(this TypeSyntax syntax, bool condition)
    {
        return condition
            ? NullableType(syntax)
            : syntax;
    }

    public static TypeDeclarationSyntax AddMembersWhen(
        this TypeDeclarationSyntax syntax,
        bool condition,
        params MemberDeclarationSyntax[] items)
    {
        return condition
            ? syntax.AddMembers(items)
            : syntax;
    }

    public static FieldDeclarationSyntax AddAttributeListsWhen(
        this FieldDeclarationSyntax syntax,
        bool condition,
        params AttributeListSyntax[] items)
    {
        return condition
            ? syntax.AddAttributeLists(items)
            : syntax;
    }

    public static ConstructorDeclarationSyntax AddBodyStatementsWhen(
        this ConstructorDeclarationSyntax syntax,
        bool condition,
        params StatementSyntax[] items)
    {
        return condition
            ? syntax.AddBodyStatements(items)
            : syntax;
    }
    
    public static TypeDeclarationSyntax AddModifiersWhen(
        this TypeDeclarationSyntax syntax,
        bool condition,
        params SyntaxToken[] items)
    {
        return condition
            ? syntax.AddModifiers(items)
            : syntax;
    }

    public static ParameterSyntax AddModifiersWhen(
        this ParameterSyntax syntax,
        bool condition,
        params SyntaxToken[] items)
    {
        return condition
            ? syntax.AddModifiers(items)
            : syntax;
    }

    public static ParameterSyntax AddAttributeListsWhen(
        this ParameterSyntax syntax,
        bool condition,
        params AttributeListSyntax[] items)
    {
        return condition
            ? syntax.AddAttributeLists(items)
            : syntax;
    }
}