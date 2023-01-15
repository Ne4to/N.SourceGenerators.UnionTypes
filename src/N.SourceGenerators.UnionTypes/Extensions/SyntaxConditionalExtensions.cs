namespace N.SourceGenerators.UnionTypes.Extensions;

internal static class SyntaxConditionalExtensions
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
}