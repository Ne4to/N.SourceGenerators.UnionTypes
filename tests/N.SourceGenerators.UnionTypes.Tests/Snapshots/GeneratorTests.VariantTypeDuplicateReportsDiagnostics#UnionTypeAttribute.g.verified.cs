//HintName: UnionTypeAttribute.g.cs
#nullable enable
using System;
using System.Runtime.CompilerServices;

namespace N.SourceGenerators.UnionTypes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
    internal sealed class UnionTypeAttribute : Attribute
    {
        public Type Type { get; }
        public string? Alias { get; }
        public int Order { get; }

        public UnionTypeAttribute(Type type, string? alias = null, [CallerLineNumber] int order = 0)
        {
            Type = type;
            Alias = alias;
            Order = order;
        }
    }
}