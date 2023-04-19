using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public class StartableSourceGenerator
{
    public static bool SyntacticPredicate(SyntaxNode syntaxNode, CancellationToken cancellationToken) =>
        syntaxNode switch
        {
            MemberDeclarationSyntax memberDeclaration
                when memberDeclaration.AttributeLists.Any() && !memberDeclaration.Modifiers.Any(SyntaxKind.AbstractKeyword) => true,
            _ => false
        };

    public static StartableContext? SemanticTransform(GeneratorSyntaxContext context, CancellationToken cancellationToken) =>
        context.Node switch
        {
            MethodDeclarationSyntax => SemanticTransformMethod(context),
            _                       => null
        };

    private static StartableContext? SemanticTransformMethod(GeneratorSyntaxContext context)
    {
        if (context.Node is not MethodDeclarationSyntax methodDeclaration)
            return null;

        var methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration);
        if (methodSymbol is null)
            return null;

        if (!StartableDiagnostics.HasAttribute(methodSymbol))
            return null;

        var startableDatum = new StartableDatum(methodSymbol.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                                                methodSymbol.Name,
                                                methodSymbol.IsStatic);

        return new StartableContext(startableData: new[] { startableDatum });
    }

    public static void ExecuteGeneration(SourceProductionContext sourceContext, ImmutableArray<StartableContext> startableContexts, string assemblyName)
    {
        var startableData = startableContexts
                            .SelectMany(startableContext => startableContext.StartableData)
                            .Where(startableDatum => startableDatum is not null)
                            .ToArray();

        var methodName = Regex.Replace(assemblyName, "\\W", "");

        var codeText = StartableWriter.Write(methodName, startableData);

        sourceContext.AddSource("Startable.g.cs", SourceText.From(codeText, Encoding.UTF8));
    }
}