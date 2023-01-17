using N.SourceGenerators.UnionTypes.Extensions;

namespace N.SourceGenerators.UnionTypes.Models;

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