using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Core;
using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Extensions;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public static class StartableWriter
{
    public static string Write(string methodName, IEnumerable<StartableDatum> startableData, bool skipVersion = false) =>
        WrapperSourceGeneratorWriter.Write(methodName, "RegisterStartables",
                                           codeBuilder => Write(startableData, codeBuilder),
                                           true,
                                           skipVersion);

    private static void Write(IEnumerable<StartableDatum> startableData, IndentedStringBuilder codeBuilder)
    {
        foreach (var startableDatum in startableData)
        {
            if (startableDatum.IsStatic)
            {
                WriteStaticStartable(codeBuilder, startableDatum);
            }
            else
            {
                WriteInstanceStartable(codeBuilder, startableDatum);
            }
        }
    }

    private static void WriteStaticStartable(IndentedStringBuilder codeBuilder, StartableDatum startableDatum)
    {
        codeBuilder
            .Append("global::Futurum.Microsoft.Extensions.DependencyInjection.StartableExtensions.AddStartable(serviceCollection, ")
            .Append("new ")
            .Append("global::Futurum.Microsoft.Extensions.DependencyInjection.StartableFunctionWrapper")
            .Append("(")
            .AppendIf("global::", !startableDatum.ClassName.StartsWith("global::"))
            .Append(startableDatum.ClassName)
            .Append('.')
            .Append(startableDatum.MethodName)
            .AppendLine("));");
    }

    private static void WriteInstanceStartable(IndentedStringBuilder codeBuilder, StartableDatum startableDatum)
    {
        codeBuilder
            .Append("global::Futurum.Microsoft.Extensions.DependencyInjection.StartableExtensions.AddStartable(serviceCollection, ")
            .Append("new ")
            .Append("global::Futurum.Microsoft.Extensions.DependencyInjection.StartableFunctionWrapper")
            .Append("(")
            .Append("new ")
            .AppendIf("global::", !startableDatum.ClassName.StartsWith("global::"))
            .Append(startableDatum.ClassName)
            .Append("()")
            .Append(".")
            .Append(startableDatum.MethodName)
            .AppendLine("));");
    }
}