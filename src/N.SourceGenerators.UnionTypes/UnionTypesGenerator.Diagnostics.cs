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
    
    private static readonly DiagnosticDescriptor CantConvertWarning = new(
        id: "UTGEN005",
        title: "Union type cannot be converted to another",
        messageFormat: "Union type '{0}' cannot be converter to '{1}' because target type doesn't have '{2}' variation",
        category: "UnionTypesGenerator",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static IncrementalValuesProvider<ValueDiagnostics<UnionType>> GetUnionTypeDiagnostics(
        IncrementalValuesProvider<UnionType> unionTypes)
    {
        return unionTypes
            .Select((unionType, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                List<Diagnostic> diagnostics = new();

                AddTypeIsNotPartialDiagnostic(unionType, diagnostics);

                var duplicateDiagnostics =
                    GetDiagnostics(unionType, v => v.TypeFullName, VariantDuplicateTypeWarning)
                        .Concat(GetDiagnostics(unionType, v => v.Alias, VariantDuplicateAliasWarning))
                        .Concat(GetDiagnostics(unionType, v => v.Order, VariantDuplicateOrderWarning));

                diagnostics.AddRange(duplicateDiagnostics);

                return new ValueDiagnostics<UnionType>(unionType, diagnostics);
            });

        void AddTypeIsNotPartialDiagnostic(UnionType unionType, List<Diagnostic> diagnostics)
        {
            if (unionType.IsPartial)
            {
                return;
            }

            Diagnostic typeIsNotPartialDiagnostic = unionType.CreateDiagnostic(
                TypeIsNotPartialWarning,
                unionType.Name);

            diagnostics.Add(typeIsNotPartialDiagnostic);
        }
    }

    private static IEnumerable<Diagnostic> GetDiagnostics<T>(
        UnionType unionType,
        Func<UnionTypeVariant, T> selector,
        DiagnosticDescriptor descriptor)
    {
        return unionType
            .Variants
            .GroupBy(selector)
            .Where(grp => grp.Count() > 1)
            .Select(grp => unionType.CreateDiagnostic(
                descriptor,
                unionType.Name,
                grp.Key
            ));
    }

    private static void ReportDiagnostics<T>(
        IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<ValueDiagnostics<T>> valueDiagnostics)
    {
        context.ReportDiagnostics(
            valueDiagnostics
                .Where(utd => utd.Diagnostics.Count != 0)
                .SelectMany((utd, _) => utd.Diagnostics)
        );
    }
}