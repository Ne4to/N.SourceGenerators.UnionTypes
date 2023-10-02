using System.Reflection;
using System.Text;

using N.SourceGenerators.UnionTypes.Extensions;

namespace N.SourceGenerators.UnionTypes.Models;

internal class UnionTypeVariant
{
    private static readonly ISet<string> Keywords = GetKeywords();

    public string Alias { get; }
    public int Order { get; }
    public string TypeFullName { get; }
    public string FieldName { get; }
    public string ParameterName { get; }
    public string IsPropertyName { get; }
    public string AsPropertyName { get; }
    public string IdConstName { get; }
    public bool IsValueType { get; }
    public int IdConstValue { get; internal set; }
    public bool IsInterface { get; }

    public UnionTypeVariant(ITypeSymbol typeSymbol, string? alias, int order)
    {
        Alias = alias ?? GetAlias(typeSymbol);
        Order = order;

        TypeFullName = typeSymbol.GetFullyQualifiedName();
        FieldName = $"_{Alias.ToStartLowerCase()}";
        ParameterName = Alias.ToStartLowerCase();
        if (Keywords.Contains(ParameterName))
        {
            ParameterName = '@' + ParameterName;
        }

        IsPropertyName = $"Is{Alias}";
        AsPropertyName = $"As{Alias}";
        IdConstName = $"{Alias}Id";
        IsValueType = typeSymbol.IsValueType;
        IsInterface = typeSymbol is { IsReferenceType: true, BaseType: null };
    }

    private static HashSet<string> GetKeywords()
    {
        var memberInfos = typeof(SyntaxKind).GetMembers(BindingFlags.Public | BindingFlags.Static);
        var keywords = from memberInfo in memberInfos
            where memberInfo.Name.EndsWith("Keyword")
            let keyword = memberInfo.Name.Substring(0, memberInfo.Name.Length - "Keyword".Length)
            select keyword.ToLower();

        return new HashSet<string>(keywords, StringComparer.Ordinal);
    }

    private static string GetAlias(ITypeSymbol type)
    {
        return type switch
        {
            INamedTypeSymbol named => GetAlias(named),
            IArrayTypeSymbol array => GetAlias(array),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private static string GetAlias(IArrayTypeSymbol type)
    {
        return "ArrayOf" + GetAlias(type.ElementType);
    }

    private static string GetAlias(INamedTypeSymbol type)
    {
        if (!type.IsGenericType)
        {
            return type.Name;
        }

        StringBuilder sb = new();
        sb.Append(type.Name);
        sb.Append("Of");

        for (int argumentIndex = 0; argumentIndex < type.TypeArguments.Length; argumentIndex++)
        {
            ITypeSymbol argumentType = type.TypeArguments[argumentIndex];
            if (argumentIndex > 0)
            {
                sb.Append("And");
            }

            string argumentTypeName = GetAlias(argumentType);
            sb.Append(argumentTypeName.ToStartUpperCase());
        }

        return sb.ToString();
    }
}