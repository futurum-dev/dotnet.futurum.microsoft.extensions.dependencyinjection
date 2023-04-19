using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public class ModuleSourceGenerator
{
    public static bool SyntacticPredicate(SyntaxNode syntaxNode, CancellationToken cancellationToken) =>
        syntaxNode switch
        {
            MemberDeclarationSyntax memberDeclaration
                when memberDeclaration.AttributeLists.Any() && !memberDeclaration.Modifiers.Any(SyntaxKind.AbstractKeyword) => true,
            _ => false
        };

    public static ModuleContext? SemanticTransform(GeneratorSyntaxContext context, CancellationToken cancellationToken) =>
        context.Node switch
        {
            MethodDeclarationSyntax => SemanticTransformMethod(context),
            _                       => null
        };

    private static ModuleContext? SemanticTransformMethod(GeneratorSyntaxContext context)
    {
        if (context.Node is not MethodDeclarationSyntax methodDeclaration)
            return null;

        var methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration);
        if (methodSymbol is null)
            return null;

        if (!ModuleDiagnostics.HasAttribute(methodSymbol))
            return null;

        var diagnostics = ModuleDiagnostics.InvalidParameter.Check(methodSymbol);
        if (diagnostics.Any())
            return new ModuleContext(diagnostics: diagnostics);

        var moduleData = new ModuleDatum(methodSymbol.ContainingType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                                         methodSymbol.Name,
                                         methodSymbol.IsStatic);

        return new ModuleContext(moduleData: new[] { moduleData });
    }

    public static void ExecuteGeneration(SourceProductionContext sourceContext, ImmutableArray<ModuleContext> moduleContexts, string assemblyName)
    {
        var moduleData = moduleContexts
                         .SelectMany(moduleContext => moduleContext.ModuleData)
                         .Where(moduleDatum => moduleDatum is not null)
                         .ToArray();

        var methodName = Regex.Replace(assemblyName, "\\W", "");

        var codeText = ModuleWriter.Write(methodName, moduleData);

        sourceContext.AddSource("Module.g.cs", SourceText.From(codeText, Encoding.UTF8));
    }
}