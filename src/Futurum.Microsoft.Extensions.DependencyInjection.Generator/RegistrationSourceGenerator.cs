using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Extensions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public class RegistrationSourceGenerator
{
    public static bool SyntacticPredicate(SyntaxNode syntaxNode, CancellationToken cancellationToken) =>
        syntaxNode switch
        {
            ClassDeclarationSyntax classDeclaration
                when classDeclaration.AttributeLists.Any() &&
                     !classDeclaration.Modifiers.Any(SyntaxKind.AbstractKeyword) &&
                     !classDeclaration.Modifiers.Any(SyntaxKind.StaticKeyword) => true,
            _ => false
        };

    public static RegistrationContext? SemanticTransform(GeneratorSyntaxContext context, CancellationToken cancellationToken) =>
        context.Node switch
        {
            ClassDeclarationSyntax => SemanticTransformClass(context),
            _                      => null
        };

    private static RegistrationContext? SemanticTransformClass(GeneratorSyntaxContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classSyntax)
            return null;

        var classSymbol = context.SemanticModel.GetDeclaredSymbol(classSyntax);
        if (classSymbol is null)
            return null;

        var attributes = classSymbol.GetAttributes();

        var registrationData = attributes
                               .Select(attribute => CreateRegistrationDatum(classSymbol, attribute))
                               .Where(registrationDatum => registrationDatum != null)
                               .ToArray();

        if (registrationData.Length == 0)
            return null;

        return new RegistrationContext(registrationData: registrationData);
    }

    public static void ExecuteGeneration(SourceProductionContext sourceContext, ImmutableArray<RegistrationContext> registrationContexts, string assemblyName)
    {
        var registrationData = registrationContexts
                               .SelectMany(registrationContext => registrationContext.RegistrationData)
                               .Where(registrationDatum => registrationDatum is not null)
                               .ToArray();

        var methodName = Regex.Replace(assemblyName, "\\W", "");

        var codeText = RegistrationWriter.Write(methodName, registrationData);

        sourceContext.AddSource("Registration.g.cs", SourceText.From(codeText, Encoding.UTF8));
    }

    private static RegistrationDatum? CreateRegistrationDatum(INamedTypeSymbol classSymbol, AttributeData attribute)
    {
        if (!IsRegisterAsAttribute(attribute, out var serviceLifetime))
            return null;

        var (serviceTypes, implementationType, duplicateRegistrationStrategy, interfaceRegistrationStrategy) = GetValuesFromAttribute(attribute);

        if (IsInterfaceRegistrationStrategyReallySelfWithInterfaces(interfaceRegistrationStrategy, implementationType, serviceTypes))
        {
            interfaceRegistrationStrategy = InterfaceRegistrationStrategy.SelfWithInterfaces;
        }

        if (IsImplementationTypeMissing(implementationType))
        {
            implementationType = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        }

        if (IsRegisterAllInterfaces(interfaceRegistrationStrategy))
        {
            foreach (var interfaceName in classSymbol.AllInterfaces.Select(implementedInterface => implementedInterface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)))
            {
                serviceTypes.Add(interfaceName);
            }
        }

        if (IsSelfRegistration(interfaceRegistrationStrategy, serviceTypes))
        {
            serviceTypes.Add(implementationType);
        }

        return new RegistrationDatum(serviceTypes,
                                     implementationType,
                                     serviceLifetime,
                                     duplicateRegistrationStrategy ?? DuplicateRegistrationStrategy.Try,
                                     interfaceRegistrationStrategy ?? InterfaceRegistrationStrategy.SelfWithInterfaces);
    }

    private static
        (HashSet<string> serviceTypes, string? implementationType, DuplicateRegistrationStrategy? duplicateRegistrationStrategy, InterfaceRegistrationStrategy? interfaceRegistrationStrategy)
        GetValuesFromAttribute(AttributeData attribute)
    {
        var serviceTypes = new HashSet<string>();
        string? implementationType = null;
        DuplicateRegistrationStrategy? duplicateRegistrationStrategy = null;
        InterfaceRegistrationStrategy? interfaceRegistrationStrategy = null;

        foreach (var parameter in attribute.NamedArguments)
        {
            var name = parameter.Key;
            var value = parameter.Value.Value;

            if (string.IsNullOrEmpty(name) || value == null)
                continue;

            switch (name)
            {
                case "ServiceType":
                    serviceTypes.Add(value.ToString());
                    break;
                case "ImplementationType":
                    implementationType = value.ToString();
                    break;
                case "Duplicate":
                    duplicateRegistrationStrategy = ParseEnum<DuplicateRegistrationStrategy>(value);
                    break;
                case "Registration":
                    interfaceRegistrationStrategy = ParseEnum<InterfaceRegistrationStrategy>(value);
                    break;
            }
        }

        return (serviceTypes, implementationType, duplicateRegistrationStrategy, interfaceRegistrationStrategy);
    }

    private static TEnum? ParseEnum<TEnum>(object value)
        where TEnum : struct, Enum =>
        value switch
        {
            int numberValue    => Enum.IsDefined(typeof(TEnum), numberValue) ? (TEnum)Enum.ToObject(typeof(TEnum), numberValue) : null,
            string stringValue => Enum.TryParse<TEnum>(stringValue, out var strategy) ? strategy : null,
            _                  => null
        };

    private static bool IsInterfaceRegistrationStrategyReallySelfWithInterfaces(InterfaceRegistrationStrategy? interfaceRegistrationStrategy, string? implementationType, IEnumerable<string> serviceTypes) =>
        interfaceRegistrationStrategy == null && implementationType == null && !serviceTypes.Any();

    private static bool IsImplementationTypeMissing(string? implementationType) =>
        implementationType.IsNullOrWhiteSpace();

    private static bool IsRegisterAllInterfaces(InterfaceRegistrationStrategy? interfaceRegistrationStrategy) =>
        interfaceRegistrationStrategy is InterfaceRegistrationStrategy.ImplementedInterfaces or InterfaceRegistrationStrategy.SelfWithInterfaces;

    private static bool IsSelfRegistration(InterfaceRegistrationStrategy? interfaceRegistrationStrategy, ICollection<string> serviceTypes) =>
        interfaceRegistrationStrategy is InterfaceRegistrationStrategy.Self or InterfaceRegistrationStrategy.SelfWithInterfaces || serviceTypes.Count == 0;

    private static bool IsRegisterAsAttribute(AttributeData attribute, out RegistrationLifetime registrationLifetime)
    {
        if (IsTransientAttribute(attribute))
        {
            registrationLifetime = RegistrationLifetime.Transient;
            return true;
        }

        if (IsScopedAttribute(attribute))
        {
            registrationLifetime = RegistrationLifetime.Scoped;
            return true;
        }

        if (IsSingletonAttribute(attribute))
        {
            registrationLifetime = RegistrationLifetime.Singleton;
            return true;
        }

        registrationLifetime = RegistrationLifetime.Transient;
        
        return false;
    }

    private static bool IsTransientAttribute(AttributeData attribute) =>
        attribute.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                 .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsTransientAttribute")
        ?? false;

    private static bool IsScopedAttribute(AttributeData attribute) =>
        attribute.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                 .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsScopedAttribute")
        ?? false;

    private static bool IsSingletonAttribute(AttributeData attribute) =>
        attribute.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                 .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsSingletonAttribute")
        ?? false;
}