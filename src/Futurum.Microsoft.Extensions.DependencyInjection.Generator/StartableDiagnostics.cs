using Microsoft.CodeAnalysis;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public static class StartableDiagnostics
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