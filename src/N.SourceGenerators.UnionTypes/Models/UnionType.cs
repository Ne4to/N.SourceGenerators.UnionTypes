using N.SourceGenerators.UnionTypes.Extensions;

namespace N.SourceGenerators.UnionTypes.Models;

internal class UnionType
{
    public bool IsReferenceType { get; }
    public bool IsValueType { get; }
    public string Name { get; }
    public string? Namespace { get; }
    public IReadOnlyList<UnionTypeVariant> Variants { get; }

    private bool? _useStructLayout;

    public bool UseStructLayout
    {
        get
        {
            _useStructLayout ??= IsValueType && Variants.All(v => v.IsValueType);
            return _useStructLayout.Value;
        }
    }

    public UnionType(INamedTypeSymbol containerType, IReadOnlyList<UnionTypeVariant> variants)
    {
        IsReferenceType = containerType.IsReferenceType;
        IsValueType = containerType.IsValueType;
        Name = containerType.Name;
        Namespace = containerType.ContainingNamespace.IsGlobalNamespace 
            ? null
            : containerType.ContainingNamespace.ToString();
        Variants = variants;

        for (int variantIndex = 0; variantIndex < variants.Count; variantIndex++)
        {
            UnionTypeVariant variant = variants[variantIndex];
            variant.IdConstValue = variantIndex + 1;
        }
    }
}