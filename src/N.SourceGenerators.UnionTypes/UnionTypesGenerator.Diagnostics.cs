using N.SourceGenerators.UnionTypes.Extensions;
using N.SourceGenerators.UnionTypes.Models;

namespace N.SourceGenerators.UnionTypes;

public partial class UnionTypesGenerator
{
    private static readonly DiagnosticDescriptor TypeIsNotPartialWarning = new(
        id: "UTGEN001",
        title: "Union type must be partial",
        messageFormat: "Union type '{0}' must be partial",
        category: "UnionTypesGenerator",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor VariantDuplicateTypeWarning = new(
        id: "UTGEN002",
        title: "Union type variant's type must be unique",
        messageFormat: "Union type '{0}' has variants with duplicate type '{1}'",
        category: "UnionTypesGenerator",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor VariantDuplicateAliasWarning = new(
        id: "UTGEN003",
        title: "Union type variant's alias must be unique",
        messageFormat: "Union type '{0}' has variants with duplicate alias '{1}'",
        category: "UnionTypesGenerator",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor VariantDuplicateOrderWarning = new(
        id: "UTGEN004",
        title: "Union type variant's order must be unique",
        messageFormat: "Union type '{0}' has variants with duplicate order '{1}'",
        category: "UnionTypesGenerator",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static IncrementalValuesProvider<UnionTypeDiagnostics> GetUnionTypeDiagnostics(
        IncrementalValuesProvider<UnionTypeReference> unionTypeReferences)
    {
        return unionTypeReferences
            .Select((utr, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                List<Diagnostic> diagnostics = new();

                AddTypeIsNotPartialDiagnostic(utr, diagnostics);

                diagnostics.AddRange(GetDiagnostics(utr, v => v.TypeFullName, VariantDuplicateTypeWarning));
                diagnostics.AddRange(GetDiagnostics(utr, v => v.Alias, VariantDuplicateAliasWarning));
                diagnostics.AddRange(GetDiagnostics(utr, v => v.Order, VariantDuplicateOrderWarning));

                return new UnionTypeDiagnostics(utr.Type, diagnostics);
            });

        void AddTypeIsNotPartialDiagnostic(UnionTypeReference utr, List<Diagnostic> diagnostics)
        {
            if (utr.Reference.Syntax.IsPartial())
            {
                return;
            }

            Diagnostic typeIsNotPartialDiagnostic = Diagnostic.Create(
                TypeIsNotPartialWarning,
                GetLocation(utr.Reference),
                utr.Reference.Symbol.Name);

            diagnostics.Add(typeIsNotPartialDiagnostic);
        }
    }

    private static IEnumerable<Diagnostic> GetDiagnostics<T>(
        UnionTypeReference utr,
        Func<UnionTypeVariant, T> selector,
        DiagnosticDescriptor descriptor)
    {
        return utr
            .Type
            .Variants
            .GroupBy(selector)
            .Where(grp => grp.Count() > 1)
            .Select(grp => Diagnostic.Create(
                descriptor,
                GetLocation(utr.Reference),
                utr.Reference.Symbol.Name,
                grp.Key
            ));
    }

    private static void ReportDiagnostics(
        IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<UnionTypeDiagnostics> unionTypeDiagnostics)
    {
        context.ReportDiagnostics(
            unionTypeDiagnostics
                .Where(utd => utd.Diagnostics.Count != 0)
                .SelectMany((utd, _) => utd.Diagnostics)
        );
    }

    private static Location GetLocation(UnionReference r)
    {
        return Location.Create(r.Syntax.SyntaxTree, r.Syntax.GetLocation().SourceSpan);
    }
}