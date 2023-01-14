using System.Runtime.CompilerServices;

namespace N.SourceGenerators.UnionTypes.Tests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Enable();
    }
}