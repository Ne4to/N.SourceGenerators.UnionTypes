namespace N.SourceGenerators.UnionTypes.Models;

internal class UnionType
{
    public INamedTypeSymbol ContainerType { get; }
    public IReadOnlyList<UnionTypeVariant> Variants { get; }

    public UnionType(INamedTypeSymbol containerType, IReadOnlyList<UnionTypeVariant> variants)
    {
        ContainerType = containerType;
        Variants = variants;
    }

    internal void Deconstruct(out INamedTypeSymbol containerType, out IReadOnlyList<UnionTypeVariant> variants)
    {
        containerType = ContainerType;
        variants = Variants;
    }
}