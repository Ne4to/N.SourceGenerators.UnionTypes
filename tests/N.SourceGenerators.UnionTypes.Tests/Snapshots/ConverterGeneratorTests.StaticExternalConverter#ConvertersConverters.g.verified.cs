﻿//HintName: ConvertersConverters.g.cs
// <auto-generated>
//   This code was generated by https://github.com/Ne4to/N.SourceGenerators.UnionTypes
//   Feel free to open an issue
// </auto-generated>
#pragma warning disable
#nullable enable
namespace MyApp
{
    static partial class Converters
    {
        public static global::MyApp.BusinessLogicResult Convert(this global::MyApp.DataAccessResult value)
        {
            return value.Match<BusinessLogicResult>(x => x, x => x);
        }
    }
}