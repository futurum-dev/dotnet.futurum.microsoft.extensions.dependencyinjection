using Microsoft.CodeAnalysis;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor ModuleInvalidParameter = new(
        id: "FMEDI0001",
        title: "Invalid Module Parameter",
        messageFormat: "Invalid parameter '{0}' on module registration method '{1}'. Module registration will not be processed.",
        category: "Futurum.Microsoft.Extensions.DependencyInjection.Generator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor ModuleMissingParameter = new(
        id: "FMEDI0002",
        title: "Missing Module Parameter",
        messageFormat:
        "A parameter of type 'global::Microsoft.Extensions.DependencyInjection.IServiceCollection' was not found on module registration method '{0}'. Module registration will be skipped.",
        category: "Futurum.Microsoft.Extensions.DependencyInjection.Generator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor ModuleNonEmptyConstructor = new(
        "FMEDI0003",
        "Non empty constructor found on Module",
        $"Module class '{{0}}' does not have an empty constructor.{Environment.NewLine} Module classes must have an empty constructor.",
        "Futurum.Microsoft.Extensions.DependencyInjection.Generator",
        DiagnosticSeverity.Error,
        true);

    public static readonly DiagnosticDescriptor StartableNonEmptyConstructor = new(
        "FMEDI0004",
        "Non empty constructor found on Startable",
        $"Startable class '{{0}}' does not have an empty constructor.{Environment.NewLine} Startable classes must have an empty constructor.",
        "Futurum.Microsoft.Extensions.DependencyInjection.Generator",
        DiagnosticSeverity.Error,
        true);

    public static readonly DiagnosticDescriptor StartableNonAsyncMethod = new(
        "FMEDI0005",
        "Non async method found on Startable",
        $"Startable method '{{0}}' is not async.{Environment.NewLine} Startable methods must be async.",
        "Futurum.Microsoft.Extensions.DependencyInjection.Generator",
        DiagnosticSeverity.Error,
        true);

    public static readonly DiagnosticDescriptor ModuleNonVoidReturn = new(
        "FMEDI0006",
        "Non void method found on Module",
        $"Module method '{{0}}' does not return void.{Environment.NewLine} Module methods must return void.",
        "Futurum.Microsoft.Extensions.DependencyInjection.Generator",
        DiagnosticSeverity.Error,
        true);

    public static readonly DiagnosticDescriptor RegistrationServiceTypeNotImplementedByClass = new(
        "FMEDI0007",
        "Register ServiceType not implemented by class",
        $"Class '{{0}}' does not implement ServiceType '{{1}}'.{Environment.NewLine} Class must implement the ServiceType.",
        "Futurum.Microsoft.Extensions.DependencyInjection.Generator",
        DiagnosticSeverity.Error,
        true);
}