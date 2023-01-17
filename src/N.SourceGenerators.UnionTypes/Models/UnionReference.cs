namespace N.SourceGenerators.UnionTypes.Models;

internal record UnionReference(TypeDeclarationSyntax Syntax, INamedTypeSymbol Symbol)
{
    public TypeDeclarationSyntax Syntax { get; } = Syntax;
    public INamedTypeSymbol Symbol { get; } = Symbol;
}