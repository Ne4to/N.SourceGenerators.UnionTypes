﻿//HintName: DataAccessResultToConverter.g.cs
// <auto-generated>
//   This code was generated by https://github.com/Ne4to/N.SourceGenerators.UnionTypes
//   Feel free to open an issue
// </auto-generated>
#pragma warning disable
#nullable enable
namespace MyApp
{
    partial class DataAccessResult
    {
        public static implicit operator BusinessLogicResult(global::MyApp.DataAccessResult value)
        {
            return value.Match<BusinessLogicResult>(x => x, x => x);
        }
    }
}