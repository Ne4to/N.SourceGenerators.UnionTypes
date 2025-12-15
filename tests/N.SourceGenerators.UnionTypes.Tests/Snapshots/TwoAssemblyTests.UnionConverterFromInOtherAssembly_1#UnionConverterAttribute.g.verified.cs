//HintName: UnionConverterAttribute.g.cs
#if NETCOREAPP3_1_OR_GREATER
#nullable enable
#endif
using Attribute = global::System.Attribute;
using AttributeUsageAttribute = global::System.AttributeUsageAttribute;
using AttributeTargets = global::System.AttributeTargets;
using Type = global::System.Type;

namespace N.SourceGenerators.UnionTypes
{
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    sealed class UnionConverterAttribute : Attribute
    {
        public Type FromType { get; }
        public Type ToType { get; }
#if NETCOREAPP3_1_OR_GREATER
        public string? MethodName { get; }
#else
        public string MethodName { get; }
#endif        

#if NETCOREAPP3_1_OR_GREATER
        public UnionConverterAttribute(Type fromType, Type toType, string? methodName = null)
#else
        public UnionConverterAttribute(Type fromType, Type toType, string methodName = null)
#endif
        {
            FromType = fromType;
            ToType = toType;
            MethodName = methodName;
        }
    }
}