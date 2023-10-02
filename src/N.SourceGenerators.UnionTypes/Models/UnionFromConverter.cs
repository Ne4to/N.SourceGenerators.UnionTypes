using System.Collections;
using System.Collections.Immutable;

namespace N.SourceGenerators.UnionTypes.Models;

internal record UnionFromConverter(UnionType ContainerType, ImmutableArray<UnionType> FromTypes)
    : IUnionConverter
{
    public IEnumerator<Converter> GetEnumerator()
    {
        foreach (UnionType fromType in FromTypes)
        {
            yield return new Converter(fromType, ContainerType);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public string SourceHintName { get; } = $"{ContainerType.SourceCodeFileName}FromConverter.g.cs";
}