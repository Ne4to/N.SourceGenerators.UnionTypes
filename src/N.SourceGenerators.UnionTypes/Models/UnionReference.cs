namespace N.SourceGenerators.UnionTypes.Models;

internal record UnionReference(ClassDeclarationSyntax Syntax, INamedTypeSymbol Symbol)
{
    public ClassDeclarationSyntax Syntax { get; } = Syntax;
    public INamedTypeSymbol Symbol { get; } = Symbol;
}