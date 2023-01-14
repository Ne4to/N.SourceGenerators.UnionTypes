// Ported from https://github.com/CommunityToolkit/dotnet/tree/main/src/CommunityToolkit.Mvvm.SourceGenerators

using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

namespace N.SourceGenerators.UnionTypes.Extensions;

/// <summary>
/// Extension methods for the <see cref="ISymbol"/> type.
/// </summary>
internal static class ISymbolExtensions
{
    /// <summary>
    /// Checks whether or not a given symbol has an attribute with the specified fully qualified metadata name.
    /// </summary>
    /// <param name="symbol">The input <see cref="ISymbol"/> instance to check.</param>
    /// <param name="name">The attribute name to look for.</param>
    /// <returns>Whether or not <paramref name="symbol"/> has an attribute with the specified name.</returns>
    public static bool HasAttributeWithFullyQualifiedMetadataName(this ISymbol symbol, string name)
    {
        ImmutableArray<AttributeData> attributes = symbol.GetAttributes();

        foreach (AttributeData attribute in attributes)
        {
            if (attribute.AttributeClass?.HasFullyQualifiedMetadataName(name) == true)
            {
                return true;
            }
        }

        return false;
    }
}