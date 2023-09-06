namespace N.SourceGenerators.UnionTypes.Extensions;

internal static class StringExtensions
{
    public static string ToStartLowerCase(this string value)
    {
        if (value.Length == 0)
        {
            return string.Empty;
        } 
        
        if (char.IsLower(value[0]))
        {
            return value;
        }

        return char.ToLower(value[0]) + value.Substring(1);
    }

    public static string ToStartUpperCase(this string value)
    {
        if (value.Length == 0)
        {
            return string.Empty;
        } 
        
        if (char.IsUpper(value[0]))
        {
            return value;
        }

        return char.ToUpper(value[0]) + value.Substring(1);
    }
}