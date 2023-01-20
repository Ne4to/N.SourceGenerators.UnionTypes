namespace N.SourceGenerators.UnionTypes.Models;

internal class UnionType
{
    public bool IsReferenceType { get; }
    public string Name { get; }
    public string Namespace { get; }
    public IReadOnlyList<UnionTypeVariant> Variants { get; }

    public UnionType(INamedTypeSymbol containerType, IReadOnlyList<UnionTypeVariant> variants)
    {
        IsReferenceType = containerType.IsReferenceType;
        Name = containerType.Name;
        Namespace = containerType.ContainingNamespace.Name;
        Variants = variants;
    }
}