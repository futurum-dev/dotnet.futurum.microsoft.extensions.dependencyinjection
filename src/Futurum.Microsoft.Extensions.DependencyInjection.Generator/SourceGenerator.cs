using System.Text;
using System.Text.RegularExpressions;

using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Core;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

[Generator]
public class SourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var assemblyName = context.CompilationProvider
                                  .Select(static (c, _) => c.AssemblyName);

        Generator(context, assemblyName);

        Module(context, assemblyName);

        Registration(context, assemblyName);

        Startable(context, assemblyName);
    }

    private static void Generator(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<string?> assemblyName)
    {
        context.RegisterSourceOutput(assemblyName,
                                     static (productionContext, assemblyName) => ExecuteGeneration(productionContext, assemblyName));

        static void ExecuteGeneration(SourceProductionContext context, string assemblyName)
        {
            var methodName = Regex.Replace(assemblyName, "\\W", "");

            var codeBuilder = SourceGeneratorWriter.Write(methodName);

            context.AddSource("Generator.g.cs", SourceText.From(codeBuilder, Encoding.UTF8));
        }
    }

    private static void Module(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<string?> assemblyName)
    {
        var pipeline = context.SyntaxProvider.CreateSyntaxProvider(ModuleSourceGenerator.SyntacticPredicate, ModuleSourceGenerator.SemanticTransform)
                              .Where(static moduleContext => moduleContext is not null)
                              .Select(static (moduleContext, _) => moduleContext!);

        var diagnostics = pipeline
                          .Select(static (moduleContext, _) => moduleContext.Diagnostics)
                          .Where(static diagnostics => diagnostics.Any());

        context.RegisterSourceOutput(diagnostics, ReportDiagnostic);

        var moduleContexts = pipeline
                             .Where(static moduleContext => moduleContext.ModuleData.Any())
                             .Collect();

        var generation = moduleContexts.Combine(assemblyName);

        context.RegisterSourceOutput(generation, (productionContext, tuple) => ModuleSourceGenerator.ExecuteGeneration(productionContext, tuple.Left, tuple.Right));
    }

    private static void Registration(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<string?> assemblyName)
    {
        var pipeline = context.SyntaxProvider.CreateSyntaxProvider(RegistrationSourceGenerator.SyntacticPredicate, RegistrationSourceGenerator.SemanticTransform)
                              .Where(static registrationContext => registrationContext is not null)
                              .Select(static (registrationContext, _) => registrationContext!);

        var diagnostics = pipeline
                          .Select(static (registrationContext, _) => registrationContext.Diagnostics)
                          .Where(static diagnostics => diagnostics.Any());

        context.RegisterSourceOutput(diagnostics, ReportDiagnostic);

        var registrationContexts = pipeline
                                   .Where(static registrationContext => registrationContext.RegistrationData.Any())
                                   .Collect();

        var generation = registrationContexts.Combine(assemblyName);

        context.RegisterSourceOutput(generation, (productionContext, tuple) => RegistrationSourceGenerator.ExecuteGeneration(productionContext, tuple.Left, tuple.Right));
    }

    private static void Startable(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<string?> assemblyName)
    {
        var pipeline = context.SyntaxProvider.CreateSyntaxProvider(StartableSourceGenerator.SyntacticPredicate, StartableSourceGenerator.SemanticTransform)
                              .Where(static startableContext => startableContext is not null)
                              .Select(static (startableContext, _) => startableContext!);

        var diagnostics = pipeline
                          .Select(static (startableContext, _) => startableContext.Diagnostics)
                          .Where(static diagnostics => diagnostics.Any());

        context.RegisterSourceOutput(diagnostics, ReportDiagnostic);

        var startableContexts = pipeline
                             .Where(static startableContext => startableContext.StartableData.Any())
                             .Collect();

        var generation = startableContexts.Combine(assemblyName);

        context.RegisterSourceOutput(generation, (productionContext, tuple) => StartableSourceGenerator.ExecuteGeneration(productionContext, tuple.Left, tuple.Right));
    }

    private static void ReportDiagnostic(SourceProductionContext context, EquatableArray<Diagnostic> diagnostics)
    {
        foreach (var diagnostic in diagnostics)
            context.ReportDiagnostic(diagnostic);
    }
}