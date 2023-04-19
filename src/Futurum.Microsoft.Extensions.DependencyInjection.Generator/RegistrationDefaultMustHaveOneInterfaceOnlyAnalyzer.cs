using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class RegistrationDefaultMustHaveOneInterfaceOnlyAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create(DiagnosticDescriptors.RegistrationDefaultMustHaveOneInterfaceOnly);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSymbolAction(Execute, SymbolKind.NamedType);
    }

    private static void Execute(SymbolAnalysisContext context)
    {
        if (context.Symbol is not INamedTypeSymbol classSymbol)
            return;

        var attributes = RegistrationDiagnostics.GetAttributes(classSymbol);
        if (!attributes.Any())
            return;

        foreach (var attribute in attributes)
        {
            var diagnostics = RegistrationDiagnostics.DefaultMustHaveOneInterfaceOnly.Check(classSymbol, attribute);

            foreach (var diagnostic in diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }   
        }
    }
}