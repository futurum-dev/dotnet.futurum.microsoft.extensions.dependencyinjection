using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ModuleParameterAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create(DiagnosticDescriptors.ModuleInvalidParameter,
                                DiagnosticDescriptors.ModuleMissingParameter);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSymbolAction(Execute, SymbolKind.Method);
    }

    private static void Execute(SymbolAnalysisContext context)
    {
        if (context.Symbol is not IMethodSymbol methodSymbol)
            return;

        if (!Diagnostics.Module.HasAttribute(methodSymbol))
            return;
        
        var diagnostics = Diagnostics.Module.InvalidParameter.Check(methodSymbol);

        foreach (var diagnostic in diagnostics)
        {
            context.ReportDiagnostic(diagnostic);
        }
    }
}