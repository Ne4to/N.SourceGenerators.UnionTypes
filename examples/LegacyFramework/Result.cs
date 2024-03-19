using N.SourceGenerators.UnionTypes;

namespace LegacyFramework
{
    public class Foo
    {
    }

    public class Bar
    {
    }

    
    [UnionType(typeof(Foo))]
    [UnionType(typeof(Bar))]
    public partial class Result
    {
    }
    
    [UnionType(typeof(Foo))]
    [UnionType(typeof(Bar))]
    public partial struct StructResult
    {
    }
}