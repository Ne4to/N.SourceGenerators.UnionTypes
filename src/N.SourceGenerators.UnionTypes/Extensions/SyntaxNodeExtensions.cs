using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace N.SourceGenerators.UnionTypes.Extensions;

internal static class SyntaxNodeExtensions
{
    public static bool IsClassWithAttributes(this SyntaxNode s)
    {
        return s is ClassDeclarationSyntax
        {
            AttributeLists.Count: > 0
        };
    }
}