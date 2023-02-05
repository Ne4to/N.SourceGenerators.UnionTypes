﻿using System.Text;

using Microsoft.CodeAnalysis.Text;

namespace N.SourceGenerators.UnionTypes;

public partial class UnionTypesGenerator
{
    private const string UnionTypeAttributeName = "N.SourceGenerators.UnionTypes.UnionTypeAttribute";
    private const string UnionConverterFromAttributeName = "N.SourceGenerators.UnionTypes.UnionConverterFromAttribute";
    private const string UnionConverterToAttributeName = "N.SourceGenerators.UnionTypes.UnionConverterToAttribute";
    private const string UnionConverterAttributeName = "N.SourceGenerators.UnionTypes.UnionConverterAttribute";

    private const string UnionTypeAttributeText = $$"""
        {{AutoGeneratedComment}}
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
        """;

    private const string UnionConverterFromAttributeText = $$"""
        {{AutoGeneratedComment}}
        #nullable enable
        using System;
        using System.Runtime.CompilerServices;
        
        namespace N.SourceGenerators.UnionTypes
        {
            [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
            sealed class UnionConverterFromAttribute : Attribute
            {
                public Type FromType { get; }

                public UnionConverterFromAttribute(Type fromType)
                {
                    FromType = fromType;
                }
            }
        }
        """;
    
    private const string UnionConverterToAttributeText = $$"""
        {{AutoGeneratedComment}}
        #nullable enable
        using System;
        using System.Runtime.CompilerServices;
        
        namespace N.SourceGenerators.UnionTypes
        {
            [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
            sealed class UnionConverterToAttribute : Attribute
            {
                public Type ToType { get; }

                public UnionConverterToAttribute(Type toType)
                {
                    ToType = toType;
                }
            }
        }
        """;
        
    private const string UnionConverterAttributeText = """
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
        """;

    private static void AddAttributesSource(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource("UnionTypeAttribute.g.cs", SourceText.From(UnionTypeAttributeText, Encoding.UTF8));
            ctx.AddSource("UnionConverterFromAttribute.g.cs", SourceText.From(UnionConverterFromAttributeText, Encoding.UTF8));
            ctx.AddSource("UnionConverterToAttribute.g.cs", SourceText.From(UnionConverterToAttributeText, Encoding.UTF8));
            ctx.AddSource("UnionConverterAttribute.g.cs", SourceText.From(UnionConverterAttributeText, Encoding.UTF8));
        });
    }
}