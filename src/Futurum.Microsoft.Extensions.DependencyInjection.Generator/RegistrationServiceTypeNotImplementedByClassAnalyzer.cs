using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class RegistrationServiceTypeNotImplementedByClassAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create(DiagnosticDescriptors.RegistrationServiceTypeNotImplementedByClass);

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

        var attributes = Diagnostics.Registration.HasAttribute(classSymbol);
        if (!attributes.Any())
            return;

        foreach (var attribute in attributes)
        {
            var diagnostics = Diagnostics.Registration.ServiceTypeNotImplementedByClass.Check(classSymbol, attribute);

            foreach (var diagnostic in diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }   
        }
    }
}