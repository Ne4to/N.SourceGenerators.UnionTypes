﻿//HintName: JsonPolymorphicUnionAttribute.g.cs
// <auto-generated>
//   This code was generated by https://github.com/Ne4to/N.SourceGenerators.UnionTypes
//   Feel free to open an issue
// </auto-generated>
#if NETCOREAPP3_1_OR_GREATER
#nullable enable
#endif
using System;
using System.Runtime.CompilerServices;

namespace N.SourceGenerators.UnionTypes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    internal sealed class JsonPolymorphicUnionAttribute : Attribute
    {
#if NETCOREAPP3_1_OR_GREATER
        public string? TypeDiscriminatorPropertyName { get; set; }
#else
        public string TypeDiscriminatorPropertyName { get; set; }
#endif    
    }
}