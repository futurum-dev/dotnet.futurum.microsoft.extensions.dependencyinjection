using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Core;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public static class SourceGeneratorWriter
{
    public static string Write(string methodName, bool skipVersion = false) =>
        WrapperSourceGeneratorWriter.Write(methodName, $"AddDependencyInjectionFor{methodName}",
                                           Write,
                                           false,
                                           skipVersion);

    private static void Write(IndentedStringBuilder codeBuilder)
    {
        codeBuilder.AppendLine("serviceCollection.RegisterModules();");
        codeBuilder.AppendLine("serviceCollection.RegisterRegistrations();");
        codeBuilder.AppendLine("serviceCollection.RegisterStartables();");
    }
}