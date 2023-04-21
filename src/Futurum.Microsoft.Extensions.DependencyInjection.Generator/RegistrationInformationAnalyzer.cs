using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class RegistrationInformationAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create(DiagnosticDescriptors.RegistrationInformation);

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

        var registrationAttributes = RegistrationDiagnostics.GetAttributes(classSymbol);
        if (!registrationAttributes.Any())
            return;

        var diagnostics = RegistrationDiagnostics.Information
                                                 .GetRegistrationData(registrationAttributes, classSymbol)
                                                 .Select(registrationDatum => Diagnostic.Create(DiagnosticDescriptors.RegistrationInformation,
                                                                                                classSymbol.Locations.First(),
                                                                                                registrationDatum.DebugServiceType,
                                                                                                registrationDatum.DebugImplementationType,
                                                                                                registrationDatum.DuplicateRegistrationStrategy.ToString(),
                                                                                                registrationDatum.Lifetime.ToString()));

        foreach (var diagnostic in diagnostics)
        {
            context.ReportDiagnostic(diagnostic);
        }
    }
}