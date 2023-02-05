using System.Collections.Immutable;

namespace N.SourceGenerators.UnionTypes.Models;

internal record Converter(UnionType FromType, UnionType ToType);

internal record StaticConverter(UnionType FromType, UnionType ToType, string? MethodName)
{
    public string MethodName { get; } = MethodName ?? "Convert";
}

internal record UnionConverter
{
    public Location Location { get; }
    public bool IsStatic { get; }
    public string Name { get; }
    public string? Namespace { get; }
    public ImmutableArray<StaticConverter> Converters { get; }

    public UnionConverter(
        TypeDeclarationSyntax syntax,
        INamedTypeSymbol symbol,
        ImmutableArray<StaticConverter> converters)
    {
        Location = syntax.GetLocation();
        IsStatic = syntax.Modifiers.Any(SyntaxKind.StaticKeyword);
        Name = symbol.Name;
        Namespace = symbol.ContainingNamespace.IsGlobalNamespace
            ? null
            : symbol.ContainingNamespace.ToString();
        Converters = converters;
    }
}