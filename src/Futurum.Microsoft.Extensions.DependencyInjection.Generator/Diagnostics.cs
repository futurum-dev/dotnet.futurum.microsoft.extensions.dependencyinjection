using Microsoft.CodeAnalysis;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public static class Diagnostics
{
    public static class Registration
    {
        public static IEnumerable<AttributeData> GetAttributes(INamedTypeSymbol classSymbol)
        {
            return classSymbol.GetAttributes().Where(IsRegistrationAttribute);

            static bool IsRegistrationAttribute(AttributeData attribute)
            {
                var attributeClass = attribute.AttributeClass;

                if (attributeClass == null)
                    return false;

                return attributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsTransient") ||
                       attributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsScoped") ||
                       attributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsSingleton");
            }
        }
        
        public static bool IsDefaultAttribute(AttributeData attribute)
        {
            var attributeAttributeClass = attribute.AttributeClass;
            if (attributeAttributeClass == null)
            {
                return false;
            }

            return attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsScopedAttribute") ||
                   attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsSingletonAttribute") ||
                   attributeAttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsTransientAttribute");
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

        public static class RegistrationDefaultMustHaveOneInterfaceOnly
        {
            public static IEnumerable<Diagnostic> Check(INamedTypeSymbol classSymbol, AttributeData attributeData)
            {
                if(!IsDefaultAttribute(attributeData))
                    yield break;

                if(classSymbol.Interfaces.Length == 1)
                    yield break;

                yield return Diagnostic.Create(DiagnosticDescriptors.RegistrationDefaultMustHaveOneInterfaceOnly,
                                               attributeData.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                                               classSymbol.Name,
                                               classSymbol.AllInterfaces.Length);
            }
        }
    }

    public static class Module
    {
        public static bool HasAttribute(IMethodSymbol methodSymbol)
        {
            return methodSymbol.GetAttributes().Any(IsModuleAttribute);

            static bool IsModuleAttribute(AttributeData attribute) =>
                attribute.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                         .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsDependencyInjectionModuleAttribute")
                ?? false;
        }

        public static class InvalidParameter
        {
            public static IEnumerable<Diagnostic> Check(IMethodSymbol methodSymbol)
            {
                var hasServiceCollection = false;
                var methodName = methodSymbol.Name;

                foreach (var parameterSymbol in methodSymbol.Parameters)
                {
                    var parameterIsServiceCollection = IsParameterTypeServiceCollection(parameterSymbol);

                    if (parameterIsServiceCollection)
                    {
                        hasServiceCollection = true;
                        continue;
                    }

                    yield return Diagnostic.Create(DiagnosticDescriptors.ModuleInvalidParameter,
                                                   methodSymbol.Locations.First(),
                                                   parameterSymbol.Name,
                                                   methodName);
                }

                if (hasServiceCollection)
                    yield break;

                yield return Diagnostic.Create(DiagnosticDescriptors.ModuleMissingParameter,
                                               methodSymbol.Locations.First(),
                                               methodName);
            }

            private static bool IsParameterTypeServiceCollection(IParameterSymbol? parameterSymbol) =>
                parameterSymbol?.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::Microsoft.Extensions.DependencyInjection.IServiceCollection";
        }

        public static class NonEmptyConstructor
        {
            public static IEnumerable<Diagnostic> Check(IMethodSymbol methodSymbol, INamedTypeSymbol classSymbol)
            {
                if (classSymbol.IsStatic)
                    yield break;

                foreach (var classSymbolConstructor in classSymbol.Constructors)
                {
                    var emptyConstructor = !classSymbolConstructor.Parameters.Any();

                    if (!emptyConstructor)
                    {
                        yield return Diagnostic.Create(DiagnosticDescriptors.ModuleNonEmptyConstructor,
                                                       methodSymbol.Locations.First(),
                                                       classSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat));
                    }
                }
            }
        }

        public static class NonVoidReturn
        {
            public static IEnumerable<Diagnostic> Check(IMethodSymbol methodSymbol)
            {
                if (methodSymbol.ReturnsVoid)
                    yield break;

                yield return Diagnostic.Create(DiagnosticDescriptors.ModuleNonVoidReturn,
                                               methodSymbol.Locations.First(),
                                               methodSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat));
            }
        }
    }

    public static class Startable
    {
        public static bool HasAttribute(IMethodSymbol methodSymbol)
        {
            return methodSymbol.GetAttributes().Any(IsStartableAttribute);

            static bool IsStartableAttribute(AttributeData attribute) =>
                attribute.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                         .StartsWith("global::Futurum.Microsoft.Extensions.DependencyInjection.RegisterAsDependencyInjectionStartableAttribute")
                ?? false;
        }

        public static class NonEmptyConstructor
        {
            public static IEnumerable<Diagnostic> Check(IMethodSymbol methodSymbol, INamedTypeSymbol classSymbol)
            {
                if (classSymbol.IsStatic)
                    yield break;

                foreach (var classSymbolConstructor in classSymbol.Constructors)
                {
                    var emptyConstructor = !classSymbolConstructor.Parameters.Any();

                    if (!emptyConstructor)
                    {
                        yield return Diagnostic.Create(DiagnosticDescriptors.StartableNonEmptyConstructor,
                                                       methodSymbol.Locations.First(),
                                                       classSymbol.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat));
                    }
                }
            }
        }

        public static class StartableNonAsyncMethod
        {
            public static IEnumerable<Diagnostic> Check(IMethodSymbol methodSymbol)
            {
                if (methodSymbol.IsAsync)
                    yield break;

                if (methodSymbol.ReturnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::System.Threading.Tasks.Task"))
                    yield break;

                yield return Diagnostic.Create(DiagnosticDescriptors.StartableNonAsyncMethod,
                                               methodSymbol.Locations.First(),
                                               methodSymbol.Name);
            }
        }
    }
}