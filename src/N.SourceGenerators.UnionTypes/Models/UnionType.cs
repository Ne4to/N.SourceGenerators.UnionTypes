using N.SourceGenerators.UnionTypes.Extensions;

namespace N.SourceGenerators.UnionTypes.Models;

internal class UnionType
{
    public Location Location { get; }
    public bool IsPartial { get; }
    public bool IsReferenceType { get; }
    public bool IsValueType { get; }
    public string TypeFullName { get; }
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

    public UnionType(INamedTypeSymbol containerType,
        TypeDeclarationSyntax syntax,
        IReadOnlyList<UnionTypeVariant> variants)
    {
        Location = syntax.GetLocation();
        IsPartial = syntax.IsPartial();

        IsReferenceType = containerType.IsReferenceType;
        IsValueType = containerType.IsValueType;
        TypeFullName = containerType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
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

    public Diagnostic CreateDiagnostic(DiagnosticDescriptor descriptor, params object?[]? messageArgs)
    {
        return Diagnostic.Create(
            descriptor,
            Location,
            messageArgs
        );
    }
}