﻿//HintName: UnionTypeAttribute.g.cs
// <auto-generated>
//   This code was generated by https://github.com/Ne4to/N.SourceGenerators.UnionTypes
//   Feel free to open an issue
// </auto-generated>
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
        public bool AllowNull { get; set; }
        public object? TypeDiscriminator { get; set; }

        public UnionTypeAttribute(Type type, string? alias = null, [CallerLineNumber] int order = 0)
        {
            Type = type;
            Alias = alias;
            Order = order;
        }
    }
}