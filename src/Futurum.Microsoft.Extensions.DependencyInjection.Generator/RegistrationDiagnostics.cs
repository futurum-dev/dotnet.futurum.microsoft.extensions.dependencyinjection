using Microsoft.CodeAnalysis;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public static class RegistrationDiagnostics
{
    public static IEnumerable<AttributeData> GetAttributes(INamedTypeSymbol classSymbol)
    {
        return classSymbol.GetAttributes().Where(IsRegistrationAttribute);

        static bool IsRegistrationAttribute(AttributeData attribute)
        {
            var attributeClass = attribute.AttributeClass;

            if (attributeClass == null)
                return false;

            return attributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                 .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsTransient") ||
                   attributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                 .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsKeyedTransient") ||
                   attributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                 .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsScoped") ||
                   attributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                 .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsKeyedScoped") ||
                   attributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                 .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsSingleton") ||
                   attributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                 .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsKeyedSingleton");
        }
    }

    public static bool IsDefaultAttribute(AttributeData attribute)
    {
        var attributeAttributeClass = attribute.AttributeClass;
        if (attributeAttributeClass == null)
        {
            return false;
        }

        return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                      .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsScopedAttribute") ||
               attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                      .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsSingletonAttribute") ||
               attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                      .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsTransientAttribute");
    }

    public static bool IsKeyedDefaultAttribute(AttributeData attribute)
    {
        var attributeAttributeClass = attribute.AttributeClass;
        if (attributeAttributeClass == null)
        {
            return false;
        }

        return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                      .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsKeyedScopedAttribute") ||
               attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                      .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsKeyedSingletonAttribute") ||
               attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                      .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsKeyedTransientAttribute");
    }

    public static class ServiceTypeNotImplementedByClass
    {
        public static IEnumerable<Diagnostic> Check(INamedTypeSymbol classSymbol, AttributeData attributeData)
        {
            var serviceType = GetServiceTypeFromAttribute(attributeData);

            if (string.IsNullOrEmpty(serviceType))
                yield break;

            var classImplementsServiceType = classSymbol.AllInterfaces.Any(x => x.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == serviceType);

            if (!classImplementsServiceType)
                yield return Diagnostic.Create(DiagnosticDescriptors.RegistrationServiceTypeNotImplementedByClass,
                                               attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                                               classSymbol.Name,
                                               serviceType);
        }

        private static string? GetServiceTypeFromAttribute(AttributeData attribute)
        {
            var attributeClass = attribute.AttributeClass;

            string? serviceType = null;

            if (attributeClass?.IsGenericType == true && attributeClass.TypeArguments.Length == attributeClass.TypeParameters.Length)
            {
                for (var index = 0; index < attributeClass.TypeParameters.Length; index++)
                {
                    var typeParameter = attributeClass.TypeParameters[index];
                    var typeArgument = attributeClass.TypeArguments[index];

                    switch (typeParameter.Name)
                    {
                        case "TService":
                            serviceType = typeArgument.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                            break;
                    }
                }
            }

            return serviceType;
        }
    }

    public static class DefaultMustHaveOneInterfaceOnly
    {
        public static IEnumerable<Diagnostic> Check(INamedTypeSymbol classSymbol, AttributeData attributeData)
        {
            if (!IsDefaultAttribute(attributeData))
                yield break;

            if (classSymbol.Interfaces.Length == 1)
                yield break;

            yield return Diagnostic.Create(DiagnosticDescriptors.RegistrationDefaultMustHaveOneInterfaceOnly,
                                           attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                                           classSymbol.Name,
                                           classSymbol.AllInterfaces.Length);
        }
    }

    public static class Information
    {
        public static IEnumerable<RegistrationDatum> GetRegistrationData(IEnumerable<AttributeData> registrationAttributes, INamedTypeSymbol classSymbol)
        {
            var implementedInterfaceNames = classSymbol.Interfaces
                                                       .Select(implementedInterface => (fullyQualifiedFormat: implementedInterface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                                                                                        errorMessageFormat: implementedInterface.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));

            foreach (var registrationAttribute in registrationAttributes)
            {
                if (IsDefaultAttribute(registrationAttribute))
                {
                    yield return Default(classSymbol, implementedInterfaceNames.First(), registrationAttribute);
                    continue;
                }

                if (IsKeyedDefaultAttribute(registrationAttribute))
                {
                    yield return KeyedDefault(classSymbol, implementedInterfaceNames.First(), registrationAttribute);
                    continue;
                }

                if (IsAsSelfAttribute(registrationAttribute))
                {
                    yield return AsSelf(classSymbol, registrationAttribute);
                    continue;
                }

                if (IsAsKeyedSelfAttribute(registrationAttribute))
                {
                    yield return AsKeyedSelf(classSymbol, registrationAttribute);
                    continue;
                }

                if (IsAsGenericTypeAttribute(registrationAttribute))
                {
                    yield return As(classSymbol, registrationAttribute);
                    continue;
                }

                if (IsAsKeyedGenericTypeAttribute(registrationAttribute))
                {
                    yield return AsKeyed(classSymbol, registrationAttribute);
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

                if (IsAsKeyedImplementedInterfacesAttribute(registrationAttribute))
                {
                    foreach (var registrationDatum in AsKeyedImplementedInterfaces(classSymbol, implementedInterfaceNames, registrationAttribute))
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

                if (IsAsKeyedImplementedInterfacesAndSelfAttribute(registrationAttribute))
                {
                    foreach (var registrationDatum in AsKeyedImplementedInterfacesAndSelf(classSymbol, implementedInterfaceNames, registrationAttribute))
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

                if (IsAsKeyedOpenGenericAttribute(registrationAttribute))
                {
                    yield return AsKeyedOpenGeneric(classSymbol, registrationAttribute);
                    continue;
                }
            }
        }

        private static RegistrationDatum Default(INamedTypeSymbol classSymbol, (string fullyQualifiedFormat, string errorMessageFormat) implementedInterfaceName, AttributeData registrationAttribute)
        {
            var classTypeFullyQualifiedFormat = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var classTypeErrorMessageFormat = classSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);

            var registrationLifetime = GetRegistrationLifetime(registrationAttribute);

            var duplicateRegistrationStrategy = GetDuplicateRegistrationStrategyFromAttribute(registrationAttribute) ?? DuplicateRegistrationStrategy.Try;

            return new RegistrationDatum(implementedInterfaceName.fullyQualifiedFormat,
                                         classTypeFullyQualifiedFormat,
                                         null,
                                         registrationLifetime,
                                         duplicateRegistrationStrategy,
                                         implementedInterfaceName.errorMessageFormat,
                                         classTypeErrorMessageFormat);
        }

        private static RegistrationDatum KeyedDefault(INamedTypeSymbol classSymbol, (string fullyQualifiedFormat, string errorMessageFormat) implementedInterfaceName, AttributeData registrationAttribute)
        {
            var nonKeyed = Default(classSymbol, implementedInterfaceName, registrationAttribute);

            var key = GetKeyFromAttribute(registrationAttribute);

            return new RegistrationDatum(nonKeyed.ServiceType,
                                         nonKeyed.ImplementationType,
                                         key,
                                         nonKeyed.Lifetime,
                                         nonKeyed.DuplicateRegistrationStrategy,
                                         nonKeyed.ServiceType,
                                         nonKeyed.ImplementationType);
        }

        private static RegistrationDatum AsSelf(INamedTypeSymbol classSymbol, AttributeData registrationAttribute)
        {
            var classTypeFullyQualifiedFormat = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var classTypeErrorMessageFormat = classSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);

            var registrationLifetime = GetRegistrationLifetime(registrationAttribute);

            var duplicateRegistrationStrategy = GetDuplicateRegistrationStrategyFromAttribute(registrationAttribute) ?? DuplicateRegistrationStrategy.Try;

            return new RegistrationDatum(classTypeFullyQualifiedFormat,
                                         classTypeFullyQualifiedFormat,
                                         null,
                                         registrationLifetime,
                                         duplicateRegistrationStrategy,
                                         classTypeErrorMessageFormat,
                                         classTypeErrorMessageFormat);
        }

        private static RegistrationDatum AsKeyedSelf(INamedTypeSymbol classSymbol, AttributeData registrationAttribute)
        {
            var nonKeyed = AsSelf(classSymbol, registrationAttribute);

            var key = GetKeyFromAttribute(registrationAttribute);

            return new RegistrationDatum(nonKeyed.ServiceType,
                                         nonKeyed.ImplementationType,
                                         key,
                                         nonKeyed.Lifetime,
                                         nonKeyed.DuplicateRegistrationStrategy,
                                         nonKeyed.ServiceType,
                                         nonKeyed.ImplementationType);
        }

        private static RegistrationDatum As(INamedTypeSymbol classSymbol, AttributeData registrationAttribute)
        {
            var classTypeFullyQualifiedFormat = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var classTypeErrorMessageFormat = classSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);

            var (serviceTypeFullyQualifiedFormat, serviceTypeErrorMessageFormat) = GetServiceTypeFromGenericTypes(registrationAttribute);

            var registrationLifetime = GetRegistrationLifetime(registrationAttribute);

            var duplicateRegistrationStrategy = GetDuplicateRegistrationStrategyFromAttribute(registrationAttribute) ?? DuplicateRegistrationStrategy.Try;

            return new RegistrationDatum(serviceTypeFullyQualifiedFormat,
                                         classTypeFullyQualifiedFormat,
                                         null,
                                         registrationLifetime,
                                         duplicateRegistrationStrategy,
                                         serviceTypeErrorMessageFormat,
                                         classTypeErrorMessageFormat);
        }

        private static RegistrationDatum AsKeyed(INamedTypeSymbol classSymbol, AttributeData registrationAttribute)
        {
            var nonKeyed = As(classSymbol, registrationAttribute);

            var key = GetKeyFromAttribute(registrationAttribute);

            return new RegistrationDatum(nonKeyed.ServiceType,
                                         nonKeyed.ImplementationType,
                                         key,
                                         nonKeyed.Lifetime,
                                         nonKeyed.DuplicateRegistrationStrategy,
                                         nonKeyed.ServiceType,
                                         nonKeyed.ImplementationType);
        }

        private static IEnumerable<RegistrationDatum> AsImplementedInterfaces(INamedTypeSymbol classSymbol,
                                                                              IEnumerable<(string fullyQualifiedFormat, string errorMessageFormat)> implementedInterfaceNames,
                                                                              AttributeData registrationAttribute)
        {
            var classTypeFullyQualifiedFormat = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var classTypeErrorMessageFormat = classSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);

            var registrationLifetime = GetRegistrationLifetime(registrationAttribute);

            var duplicateRegistrationStrategy = GetDuplicateRegistrationStrategyFromAttribute(registrationAttribute) ?? DuplicateRegistrationStrategy.Try;

            foreach (var (serviceTypeFullyQualifiedFormat, serviceTypeErrorMessageFormat) in implementedInterfaceNames)
            {
                yield return new RegistrationDatum(serviceTypeFullyQualifiedFormat,
                                                   classTypeFullyQualifiedFormat,
                                                   null,
                                                   registrationLifetime,
                                                   duplicateRegistrationStrategy,
                                                   serviceTypeErrorMessageFormat,
                                                   classTypeErrorMessageFormat);
            }
        }

        private static IEnumerable<RegistrationDatum> AsKeyedImplementedInterfaces(INamedTypeSymbol classSymbol,
                                                                                   IEnumerable<(string fullyQualifiedFormat, string errorMessageFormat)> implementedInterfaceNames,
                                                                                   AttributeData registrationAttribute)
        {
            var nonKeyeds = AsImplementedInterfaces(classSymbol, implementedInterfaceNames, registrationAttribute);

            var key = GetKeyFromAttribute(registrationAttribute);

            foreach (var nonKeyed in nonKeyeds)
            {
                yield return new RegistrationDatum(nonKeyed.ServiceType,
                                                   nonKeyed.ImplementationType,
                                                   key,
                                                   nonKeyed.Lifetime,
                                                   nonKeyed.DuplicateRegistrationStrategy,
                                                   nonKeyed.ServiceType,
                                                   nonKeyed.ImplementationType);
            }
        }

        private static IEnumerable<RegistrationDatum> AsImplementedInterfacesAndSelf(INamedTypeSymbol classSymbol,
                                                                                     IEnumerable<(string fullyQualifiedFormat, string errorMessageFormat)> implementedInterfaceNames,
                                                                                     AttributeData registrationAttribute)
        {
            var classTypeFullyQualifiedFormat = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var classTypeErrorMessageFormat = classSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);

            var registrationLifetime = GetRegistrationLifetime(registrationAttribute);

            var duplicateRegistrationStrategy = GetDuplicateRegistrationStrategyFromAttribute(registrationAttribute) ?? DuplicateRegistrationStrategy.Try;

            yield return new RegistrationDatum(classTypeFullyQualifiedFormat,
                                               classTypeFullyQualifiedFormat,
                                               null,
                                               registrationLifetime,
                                               duplicateRegistrationStrategy,
                                               classTypeErrorMessageFormat,
                                               classTypeErrorMessageFormat);

            foreach (var (serviceTypeFullyQualifiedFormat, serviceTypeErrorMessageFormat) in implementedInterfaceNames)
            {
                yield return new RegistrationDatum(serviceTypeFullyQualifiedFormat,
                                                   classTypeFullyQualifiedFormat,
                                                   null,
                                                   registrationLifetime,
                                                   duplicateRegistrationStrategy,
                                                   serviceTypeErrorMessageFormat,
                                                   classTypeErrorMessageFormat);
            }
        }

        private static IEnumerable<RegistrationDatum> AsKeyedImplementedInterfacesAndSelf(INamedTypeSymbol classSymbol,
                                                                                          IEnumerable<(string fullyQualifiedFormat, string errorMessageFormat)> implementedInterfaceNames,
                                                                                          AttributeData registrationAttribute)
        {
            var nonKeyeds = AsImplementedInterfacesAndSelf(classSymbol, implementedInterfaceNames, registrationAttribute);

            var key = GetKeyFromAttribute(registrationAttribute);

            foreach (var nonKeyed in nonKeyeds)
            {
                yield return new RegistrationDatum(nonKeyed.ServiceType,
                                                   nonKeyed.ImplementationType,
                                                   key,
                                                   nonKeyed.Lifetime,
                                                   nonKeyed.DuplicateRegistrationStrategy,
                                                   nonKeyed.ServiceType,
                                                   nonKeyed.ImplementationType);
            }
        }

        private static RegistrationDatum AsOpenGeneric(INamedTypeSymbol classSymbol, AttributeData registrationAttribute)
        {
            var (serviceType, implementationType) = GetOpenGenericFromAttribute(registrationAttribute);

            var registrationLifetime = GetRegistrationLifetime(registrationAttribute);

            var duplicateRegistrationStrategy = GetDuplicateRegistrationStrategyFromAttribute(registrationAttribute) ?? DuplicateRegistrationStrategy.Try;

            return new RegistrationDatum(serviceType,
                                         implementationType,
                                         null,
                                         registrationLifetime,
                                         duplicateRegistrationStrategy,
                                         serviceType,
                                         implementationType);
        }

        private static RegistrationDatum AsKeyedOpenGeneric(INamedTypeSymbol classSymbol, AttributeData registrationAttribute)
        {
            var nonKeyed = AsOpenGeneric(classSymbol, registrationAttribute);

            var key = GetKeyFromAttribute(registrationAttribute);

            return new RegistrationDatum(nonKeyed.ServiceType,
                                         nonKeyed.ImplementationType,
                                         key,
                                         nonKeyed.Lifetime,
                                         nonKeyed.DuplicateRegistrationStrategy,
                                         nonKeyed.ServiceType,
                                         nonKeyed.ImplementationType);
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

        private static TEnum? ParseEnum<TEnum>(object value)
            where TEnum : struct, Enum =>
            value switch
            {
                int numberValue    => Enum.IsDefined(typeof(TEnum), numberValue) ? (TEnum)Enum.ToObject(typeof(TEnum), numberValue) : null,
                string stringValue => Enum.TryParse<TEnum>(stringValue, out var strategy) ? strategy : null,
                _                  => null
            };

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

        private static (string fullyQualifiedFormat, string errorMessageFormat) GetServiceTypeFromGenericTypes(AttributeData attribute)
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
                            return (typeArgument.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                                    typeArgument.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat));
                            break;
                    }
                }
            }

            return (string.Empty, string.Empty);
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

        private static bool IsAsKeyedSelfAttribute(AttributeData attribute)
        {
            var attributeAttributeClass = attribute.AttributeClass;
            if (attributeAttributeClass == null)
            {
                return false;
            }

            return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).EndsWith("AsKeyedSelfAttribute");
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
                   attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                          .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsScoped.AsAttribute") ||
                   attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                          .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsSingleton.AsAttribute");
        }

        private static bool IsAsKeyedGenericTypeAttribute(AttributeData attribute)
        {
            var attributeAttributeClass = attribute.AttributeClass;
            if (attributeAttributeClass == null)
            {
                return false;
            }

            return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                          .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsTransient.AsKeyedAttribute") ||
                   attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                          .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsScoped.AsKeyedAttribute") ||
                   attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                          .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsSingleton.AsKeyedAttribute");
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

        private static bool IsAsKeyedImplementedInterfacesAttribute(AttributeData attribute)
        {
            var attributeAttributeClass = attribute.AttributeClass;
            if (attributeAttributeClass == null)
            {
                return false;
            }

            return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).EndsWith("AsKeyedImplementedInterfacesAttribute");
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

        private static bool IsAsKeyedImplementedInterfacesAndSelfAttribute(AttributeData attribute)
        {
            var attributeAttributeClass = attribute.AttributeClass;
            if (attributeAttributeClass == null)
            {
                return false;
            }

            return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).EndsWith("AsKeyedImplementedInterfacesAndSelfAttribute");
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

        private static bool IsAsKeyedOpenGenericAttribute(AttributeData attribute)
        {
            var attributeAttributeClass = attribute.AttributeClass;
            if (attributeAttributeClass == null)
            {
                return false;
            }

            return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).EndsWith("AsKeyedOpenGenericAttribute");
        }

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

        private static bool IsTransientAttribute(AttributeData attribute)
        {
            var attributeAttributeClass = attribute.AttributeClass;
            if (attributeAttributeClass == null)
            {
                return false;
            }

            return (attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                          .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsTransient") &&
                   attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                          .Replace("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsTransient", string.Empty)
                                          .Contains("Attribute")) ||
                   (attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                           .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsKeyedTransient") &&
                    attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                           .Replace("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsKeyedTransient", string.Empty)
                                           .Contains("Attribute"));
        }

        private static bool IsScopedAttribute(AttributeData attribute)
        {
            var attributeAttributeClass = attribute.AttributeClass;
            if (attributeAttributeClass == null)
            {
                return false;
            }

            return (attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                          .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsScoped") &&
                   attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                          .Replace("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsScoped", string.Empty)
                                          .Contains("Attribute")) ||
                   (attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                           .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsKeyedScoped") &&
                    attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                           .Replace("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsKeyedScoped", string.Empty)
                                           .Contains("Attribute"));
        }

        private static bool IsSingletonAttribute(AttributeData attribute)
        {
            var attributeAttributeClass = attribute.AttributeClass;
            if (attributeAttributeClass == null)
            {
                return false;
            }

            return (attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                          .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsSingleton") &&
                   attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                          .Replace("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsSingleton", string.Empty)
                                          .Contains("Attribute")) ||
                   (attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                           .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsKeyedSingleton") &&
                    attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                           .Replace("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsKeyedSingleton", string.Empty)
                                           .Contains("Attribute"));
        }

        private static string? GetKeyFromAttribute(AttributeData attribute)
        {
            if (attribute.ConstructorArguments.Length == 1 && attribute.ConstructorArguments[0].Value is string key)
            {
                return key;
            }

            return null;
        }
    }
}
