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

        var registrationAttributes = RegistrationDiagnostics.GetAttributes(classSymbol);

        if (!registrationAttributes.Any())
            return null;

        var registrationData = RegistrationDiagnostics.Information.GetRegistrationData(registrationAttributes, classSymbol);

        if (!registrationData.Any())
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
}