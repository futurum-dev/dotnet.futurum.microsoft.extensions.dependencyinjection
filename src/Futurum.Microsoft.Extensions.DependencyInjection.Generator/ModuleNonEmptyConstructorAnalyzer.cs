using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ModuleNonEmptyConstructorAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create(DiagnosticDescriptors.ModuleNonEmptyConstructor);

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

        if (!ModuleDiagnostics.HasAttribute(methodSymbol))
            return;

        var classSymbol = context.Compilation.GetTypeByMetadataName(methodSymbol.ContainingType.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat));

        if (classSymbol == null)
            return;

        var diagnostics = ModuleDiagnostics.NonEmptyConstructor.Check(methodSymbol, methodSymbol.ContainingType);

        foreach (var diagnostic in diagnostics)
        {
            context.ReportDiagnostic(diagnostic);
        }
    }
}