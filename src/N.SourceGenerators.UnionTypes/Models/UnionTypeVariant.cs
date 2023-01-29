using System.Text;

using N.SourceGenerators.UnionTypes.Extensions;

namespace N.SourceGenerators.UnionTypes.Models;

internal class UnionTypeVariant
{
    public string Alias { get; }
    public int Order { get; }

    public string TypeFullName { get; }
    public string FieldName { get; }
    public string IsPropertyName { get; }
    public string AsPropertyName { get; }
    public bool IsValueType { get; }
    public string IdConstName { get; }
    public int IdConstValue { get; internal set; }

    public UnionTypeVariant(ITypeSymbol typeSymbol, string? alias, int order)
    {
        Alias = alias ?? GetAlias(typeSymbol);
        Order = order;

        TypeFullName = typeSymbol.GetFullyQualifiedName();
        FieldName = $"_{ToStartLowerCase(Alias)}";
        IsPropertyName = $"Is{Alias}";
        AsPropertyName = $"As{Alias}";
        IsValueType = typeSymbol.IsValueType;
        IdConstName = $"{Alias}Id";
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
            ITypeSymbol argument = type.TypeArguments[argumentIndex];
            if (argumentIndex > 0)
            {
                sb.Append("And");
            }
            sb.Append(ToStartUpperCase(argument.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }

        return sb.ToString();
    }

    private static string ToStartLowerCase(string value)
    {
        if (char.IsLower(value[0]))
        {
            return value;
        }

        return char.ToLower(value[0]) + value.Substring(1);
    }

    private static string ToStartUpperCase(string value)
    {
        if (char.IsUpper(value[0]))
        {
            return value;
        }

        return char.ToUpper(value[0]) + value.Substring(1);
    }
}