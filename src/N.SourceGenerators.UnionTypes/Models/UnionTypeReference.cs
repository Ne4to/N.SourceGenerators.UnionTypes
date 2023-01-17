namespace N.SourceGenerators.UnionTypes.Models;

internal record UnionTypeReference(UnionType Type, UnionReference Reference)
{
    public UnionType Type { get; } = Type;
    public UnionReference Reference { get; } = Reference;
}