namespace N.SourceGenerators.UnionTypes.Models;

internal interface IUnionConverter : IEnumerable<Converter>
{
    UnionType ContainerType { get; }
    string SourceHintName { get; } 
}