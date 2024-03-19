//HintName: UnionConverterAttribute.g.cs
#if NETCOREAPP3_1_OR_GREATER
#nullable enable
#endif
using System;
using System.Runtime.CompilerServices;

namespace N.SourceGenerators.UnionTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
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