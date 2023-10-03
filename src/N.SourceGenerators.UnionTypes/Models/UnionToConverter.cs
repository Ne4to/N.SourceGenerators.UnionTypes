using System.Collections;
using System.Collections.Immutable;

namespace N.SourceGenerators.UnionTypes.Models;

internal record UnionToConverter(UnionType ContainerType, ImmutableArray<UnionType> ToTypes)
    : IUnionConverter
{
    public IEnumerator<Converter> GetEnumerator()
    {
        foreach (UnionType toType in ToTypes)
        {
            yield return new Converter(ContainerType, toType);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public string SourceHintName { get; } = $"{ContainerType.SourceCodeFileName}ToConverter.g.cs";
}