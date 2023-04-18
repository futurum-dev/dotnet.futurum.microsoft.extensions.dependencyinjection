using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

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

        var registrationAttributes = Diagnostics.Registration.GetAttributes(classSymbol);

        if (!registrationAttributes.Any())
            return null;

        var registrationData = GetRegistrationData(registrationAttributes, classSymbol);

        if (!registrationData.Any())
            return null;

        return new RegistrationContext(registrationData: registrationData);
    }

    private static IEnumerable<RegistrationDatum> GetRegistrationData(IEnumerable<AttributeData> registrationAttributes, INamedTypeSymbol classSymbol)
    {
        var implementedInterfaceNames = classSymbol.Interfaces.Select(implementedInterface => implementedInterface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

        foreach (var registrationAttribute in registrationAttributes)
        {
            if (Diagnostics.Registration.IsDefaultAttribute(registrationAttribute))
            {
                yield return Default(classSymbol, implementedInterfaceNames.First(), registrationAttribute);
                continue;
            }

            if (IsAsSelfAttribute(registrationAttribute))
            {
                yield return AsSelf(classSymbol, registrationAttribute);
                continue;
            }

            if (IsAsGenericTypeAttribute(registrationAttribute))
            {
                yield return As(classSymbol, registrationAttribute);
                continue;
            }

            if (IsAsImplementedInterfacesAttribute(registrationAttribute))
            {
                foreach (var registrationDatum in AsImplementedInterfaces(classSymbol, implementedInterfaceNames, registrationAttribute))
                {
                    yield return registrationDatum;
                }

                continue;
            }

            if (IsAsImplementedInterfacesAndSelfAttribute(registrationAttribute))
            {
                foreach (var registrationDatum in AsImplementedInterfacesAndSelf(classSymbol, implementedInterfaceNames, registrationAttribute))
                {
                    yield return registrationDatum;
                }

                continue;
            }

            if (IsAsOpenGenericAttribute(registrationAttribute))
            {
                yield return AsOpenGeneric(classSymbol, registrationAttribute);
                continue;
            }
        }
    }

    private static RegistrationDatum Default(INamedTypeSymbol classSymbol, string implementedInterfaceName, AttributeData registrationAttribute)
    {
        var classTypeName = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var registrationLifetime = GetRegistrationLifetime(registrationAttribute);

        var duplicateRegistrationStrategy = GetDuplicateRegistrationStrategyFromAttribute(registrationAttribute) ?? DuplicateRegistrationStrategy.Try;

        return new RegistrationDatum(implementedInterfaceName,
                                     classTypeName,
                                     registrationLifetime,
                                     duplicateRegistrationStrategy);
    }

    private static RegistrationDatum AsSelf(INamedTypeSymbol classSymbol, AttributeData registrationAttribute)
    {
        var classTypeName = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var registrationLifetime = GetRegistrationLifetime(registrationAttribute);

        var duplicateRegistrationStrategy = GetDuplicateRegistrationStrategyFromAttribute(registrationAttribute) ?? DuplicateRegistrationStrategy.Try;

        return new RegistrationDatum(classTypeName,
                                     classTypeName,
                                     registrationLifetime,
                                     duplicateRegistrationStrategy);
    }

    private static RegistrationDatum As(INamedTypeSymbol classSymbol, AttributeData registrationAttribute)
    {
        var classTypeName = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var serviceType = GetServiceTypeFromGenericTypes(registrationAttribute);

        var registrationLifetime = GetRegistrationLifetime(registrationAttribute);

        var duplicateRegistrationStrategy = GetDuplicateRegistrationStrategyFromAttribute(registrationAttribute) ?? DuplicateRegistrationStrategy.Try;

        return new RegistrationDatum(serviceType,
                                     classTypeName,
                                     registrationLifetime,
                                     duplicateRegistrationStrategy);
    }

    private static IEnumerable<RegistrationDatum> AsImplementedInterfaces(INamedTypeSymbol classSymbol, IEnumerable<string> implementedInterfaceNames, AttributeData registrationAttribute)
    {
        var classTypeName = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var registrationLifetime = GetRegistrationLifetime(registrationAttribute);

        var duplicateRegistrationStrategy = GetDuplicateRegistrationStrategyFromAttribute(registrationAttribute) ?? DuplicateRegistrationStrategy.Try;

        foreach (var implementedInterfaceName in implementedInterfaceNames)
        {
            yield return new RegistrationDatum(implementedInterfaceName,
                                               classTypeName,
                                               registrationLifetime,
                                               duplicateRegistrationStrategy);
        }
    }

    private static IEnumerable<RegistrationDatum> AsImplementedInterfacesAndSelf(INamedTypeSymbol classSymbol, IEnumerable<string> implementedInterfaceNames, AttributeData registrationAttribute)
    {
        var classTypeName = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var registrationLifetime = GetRegistrationLifetime(registrationAttribute);

        var duplicateRegistrationStrategy = GetDuplicateRegistrationStrategyFromAttribute(registrationAttribute) ?? DuplicateRegistrationStrategy.Try;

        yield return new RegistrationDatum(classTypeName,
                                           classTypeName,
                                           registrationLifetime,
                                           duplicateRegistrationStrategy);

        foreach (var implementedInterfaceName in implementedInterfaceNames)
        {
            yield return new RegistrationDatum(implementedInterfaceName,
                                               classTypeName,
                                               registrationLifetime,
                                               duplicateRegistrationStrategy);
        }
    }

    private static RegistrationDatum AsOpenGeneric(INamedTypeSymbol classSymbol, AttributeData registrationAttribute)
    {
        var (serviceType, implementationType) = GetOpenGenericFromAttribute(registrationAttribute);

        var registrationLifetime = GetRegistrationLifetime(registrationAttribute);

        var duplicateRegistrationStrategy = GetDuplicateRegistrationStrategyFromAttribute(registrationAttribute) ?? DuplicateRegistrationStrategy.Try;

        return new RegistrationDatum(serviceType,
                                     implementationType,
                                     registrationLifetime,
                                     duplicateRegistrationStrategy);
    }

    private static (string? serviceType, string? implementationType) GetOpenGenericFromAttribute(AttributeData attribute)
    {
        string? serviceType = null;
        string? implementationType = null;

        foreach (var parameter in attribute.NamedArguments)
        {
            var name = parameter.Key;
            var value = parameter.Value.Value;

            if (string.IsNullOrEmpty(name) || value == null)
                continue;

            if (name == "ServiceType")
            {
                serviceType = value.ToString();
            }
            else if (name == "ImplementationType")
            {
                implementationType = value.ToString();
            }
        }

        return (serviceType, implementationType);
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

    private static TEnum? ParseEnum<TEnum>(object value)
        where TEnum : struct, Enum =>
        value switch
        {
            int numberValue    => Enum.IsDefined(typeof(TEnum), numberValue) ? (TEnum)Enum.ToObject(typeof(TEnum), numberValue) : null,
            string stringValue => Enum.TryParse<TEnum>(stringValue, out var strategy) ? strategy : null,
            _                  => null
        };

    private static RegistrationLifetime GetRegistrationLifetime(AttributeData attribute)
    {
        if (IsTransientAttribute(attribute))
        {
            return RegistrationLifetime.Transient;
        }

        if (IsScopedAttribute(attribute))
        {
            return RegistrationLifetime.Scoped;
        }

        if (IsSingletonAttribute(attribute))
        {
            return RegistrationLifetime.Singleton;
        }

        return RegistrationLifetime.Transient;
    }

    private static DuplicateRegistrationStrategy? GetDuplicateRegistrationStrategyFromAttribute(AttributeData attribute)
    {
        DuplicateRegistrationStrategy? duplicateRegistrationStrategy = null;

        foreach (var parameter in attribute.NamedArguments)
        {
            var name = parameter.Key;
            var value = parameter.Value.Value;

            if (string.IsNullOrEmpty(name) || value == null)
                continue;

            if (name == "DuplicateRegistrationStrategy")
            {
                duplicateRegistrationStrategy = ParseEnum<DuplicateRegistrationStrategy>(value);
            }
        }

        return duplicateRegistrationStrategy;
    }

    private static string? GetServiceTypeFromGenericTypes(AttributeData attribute)
    {
        var attributeClass = attribute.AttributeClass;

        if (attributeClass?.IsGenericType == true && attributeClass.TypeArguments.Length == attributeClass.TypeParameters.Length)
        {
            for (var index = 0; index < attributeClass.TypeParameters.Length; index++)
            {
                var typeParameter = attributeClass.TypeParameters[index];
                var typeArgument = attributeClass.TypeArguments[index];

                switch (typeParameter.Name)
                {
                    case "TService":
                        return typeArgument.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                        break;
                }
            }
        }

        return null;
    }

    private static bool IsTransientAttribute(AttributeData attribute)
    {
        var attributeAttributeClass = attribute.AttributeClass;
        if (attributeAttributeClass == null)
        {
            return false;
        }

        return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsTransient") &&
               attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).EndsWith("Attribute");
    }

    private static bool IsScopedAttribute(AttributeData attribute)
    {
        var attributeAttributeClass = attribute.AttributeClass;
        if (attributeAttributeClass == null)
        {
            return false;
        }

        return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsScoped") &&
               attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).EndsWith("Attribute");
    }

    private static bool IsSingletonAttribute(AttributeData attribute)
    {
        var attributeAttributeClass = attribute.AttributeClass;
        if (attributeAttributeClass == null)
        {
            return false;
        }

        return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsSingleton") &&
               attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).EndsWith("Attribute");
    }



    private static bool IsAsSelfAttribute(AttributeData attribute)
    {
        var attributeAttributeClass = attribute.AttributeClass;
        if (attributeAttributeClass == null)
        {
            return false;
        }

        return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).EndsWith("AsSelfAttribute");
    }

    private static bool IsAsGenericTypeAttribute(AttributeData attribute)
    {
        var attributeAttributeClass = attribute.AttributeClass;
        if (attributeAttributeClass == null)
        {
            return false;
        }

        return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                      .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsTransient.AsAttribute") ||
               attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsScoped.AsAttribute") ||
               attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsSingleton.AsAttribute");
    }

    private static bool IsAsImplementedInterfacesAttribute(AttributeData attribute)
    {
        var attributeAttributeClass = attribute.AttributeClass;
        if (attributeAttributeClass == null)
        {
            return false;
        }

        return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).EndsWith("AsImplementedInterfacesAttribute");
    }

    private static bool IsAsImplementedInterfacesAndSelfAttribute(AttributeData attribute)
    {
        var attributeAttributeClass = attribute.AttributeClass;
        if (attributeAttributeClass == null)
        {
            return false;
        }

        return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).EndsWith("AsImplementedInterfacesAndSelfAttribute");
    }

    private static bool IsAsOpenGenericAttribute(AttributeData attribute)
    {
        var attributeAttributeClass = attribute.AttributeClass;
        if (attributeAttributeClass == null)
        {
            return false;
        }

        return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).EndsWith("AsOpenGenericAttribute");
    }
}