using Microsoft.CodeAnalysis;

using N.SourceGenerators.UnionTypes.Extensions;

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

internal class UnionTypeVariant
{
    public INamedTypeSymbol TypeSymbol { get; }
    public string Alias { get; }
    public int Order { get; }

    public string TypeFullName { get; }
    public string FieldName { get; }
    public string IsPropertyName { get; }
    public string AsPropertyName { get; }

    public UnionTypeVariant(INamedTypeSymbol typeSymbol, string? alias, int order)
    {
        TypeSymbol = typeSymbol;
        // TODO add default naming schema
        // int[] -> ArrayOfInt32
        // IReadOnlyList<string> -> IReadOnlyListOfString
        Alias = alias ?? typeSymbol.Name;
        Order = order;

        TypeFullName = TypeSymbol.GetFullyQualifiedName();
        FieldName = $"_{Alias}";
        IsPropertyName = $"Is{Alias}";
        AsPropertyName = $"As{Alias}";
    }
}