using N.SourceGenerators.UnionTypes.Extensions;
using N.SourceGenerators.UnionTypes.Models;

namespace N.SourceGenerators.UnionTypes;

public partial class UnionTypesGenerator
{
    private static readonly DiagnosticDescriptor TypeIsNotPartialError = new(
        id: "UTGEN001",
        title: "Union type must be partial",
        messageFormat: "Union type '{0}' must be partial",
        category: "UnionTypesGenerator",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static void ReportTypeIsNotPartialError(
        IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<UnionReference> unionReferences)
    {
        var diagnostics = unionReferences
            .Where(static r => !r.Syntax.IsPartial())
            .Select(static (r, _) =>
                Diagnostic.Create(
                    TypeIsNotPartialError,
                    Location.Create(r.Syntax.SyntaxTree, r.Syntax.GetLocation().SourceSpan),
                    r.Symbol.Name)
            );

        context.ReportDiagnostics(diagnostics);
    }
}