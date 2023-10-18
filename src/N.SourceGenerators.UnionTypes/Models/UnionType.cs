using System.Collections.Immutable;
using System.Text;

using N.SourceGenerators.UnionTypes.Extensions;

namespace N.SourceGenerators.UnionTypes.Models;

internal class UnionType
{
    public Location? Location { get; }
    public bool IsPartial { get; }
    public bool IsReferenceType { get; }
    public bool IsValueType { get; }
    public bool HasToStringMethod { get; }
    public string TypeFullName { get; }
    public string Name { get; }
    public string NameNoGenerics { get; }
    public string SourceCodeFileName { get; }
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
        TypeDeclarationSyntax? syntax,
        IReadOnlyList<UnionTypeVariant> variants)
    {
        if (syntax != null)
        {
            Location = syntax.GetLocation();
            IsPartial = syntax.IsPartial();
        }

        IsReferenceType = containerType.IsReferenceType;
        IsValueType = containerType.IsValueType;
        HasToStringMethod = containerType.GetMembers().Any(IsToStringMethod);
        TypeFullName = containerType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var nameBuilder = new StringBuilder(containerType.Name);
        var sourceCodeFileNameBuilder = new StringBuilder(containerType.Name);
        if (!containerType.TypeArguments.IsEmpty)
        {
            nameBuilder.Append('<');
            sourceCodeFileNameBuilder.Append("Of");
            int index = 0;
            foreach (ITypeSymbol typeArgument in containerType.TypeArguments)
            {
                if (index > 0)
                {
                    nameBuilder.Append(", ");
                    sourceCodeFileNameBuilder.Append("And");
                }

                nameBuilder.Append(typeArgument.Name);
                sourceCodeFileNameBuilder.Append(typeArgument.Name);
                index++;
            }

            nameBuilder.Append('>');
        }

        Name = nameBuilder.ToString();
        NameNoGenerics = containerType.Name;
        SourceCodeFileName = sourceCodeFileNameBuilder.ToString();

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

    private static bool IsToStringMethod(ISymbol symbol)
    {
        return symbol is IMethodSymbol
        {
            Name: nameof(ToString),
            Parameters.Length: 0,
            ReturnType.SpecialType: SpecialType.System_String
        };
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

internal class UnionTypeComparer : IEqualityComparer<UnionType>
{
    public static UnionTypeComparer Instance { get; } = new();

    private UnionTypeComparer()
    {
    }

    public bool Equals(UnionType x, UnionType y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (ReferenceEquals(x, null))
        {
            return false;
        }

        if (ReferenceEquals(y, null))
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return x.TypeFullName == y.TypeFullName;
    }

    public int GetHashCode(UnionType obj)
    {
        return obj.TypeFullName.GetHashCode();
    }
}