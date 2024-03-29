﻿//HintName: UnionConverterAttribute.g.cs
#nullable enable
using System;
using System.Runtime.CompilerServices;

namespace N.SourceGenerators.UnionTypes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class UnionConverterAttribute : Attribute
    {
        public Type FromType { get; }
        public Type ToType { get; }
        public string? MethodName { get; }

        public UnionConverterAttribute(Type fromType, Type toType, string? methodName = null)
        {
            FromType = fromType;
            ToType = toType;
            MethodName = methodName;
        }
    }
}