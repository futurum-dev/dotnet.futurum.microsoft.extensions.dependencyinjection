using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Core;
using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Extensions;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public static class ModuleWriter
{
    public static string Write(string methodName, IEnumerable<ModuleDatum> moduleData, bool skipVersion = false) =>
        WrapperSourceGeneratorWriter.Write(methodName, "RegisterModules",
                                           codeBuilder => Write(moduleData, codeBuilder),
                                           true,
                                           skipVersion);

    private static void Write(IEnumerable<ModuleDatum> moduleData, IndentedStringBuilder codeBuilder)
    {
        foreach (var moduleDatum in moduleData)
        {
            if (moduleDatum.IsStatic)
            {
                WriteStaticModule(codeBuilder, moduleDatum);
            }
            else
            {
                WriteInstanceModule(codeBuilder, moduleDatum);
            }
        }
    }

    private static void WriteStaticModule(IndentedStringBuilder codeBuilder, ModuleDatum moduleDatum)
    {
        codeBuilder
            .Append("global::Futurum.Microsoft.Extensions.DependencyInjection.ModuleExtensions.AddModule(serviceCollection, ")
            .Append("new ")
            .Append("global::Futurum.Microsoft.Extensions.DependencyInjection.ModuleFunctionWrapper")
            .Append("(")
            .AppendIf("global::", !moduleDatum.ClassName.StartsWith("global::"))
            .Append(moduleDatum.ClassName)
            .Append('.')
            .Append(moduleDatum.MethodName)
            .AppendLine("));");
    }

    private static void WriteInstanceModule(IndentedStringBuilder codeBuilder, ModuleDatum moduleDatum)
    {
        codeBuilder
            .Append("global::Futurum.Microsoft.Extensions.DependencyInjection.ModuleExtensions.AddModule(serviceCollection, ")
            .Append("new ")
            .Append("global::Futurum.Microsoft.Extensions.DependencyInjection.ModuleFunctionWrapper")
            .Append("(")
            .Append("new ")
            .AppendIf("global::", !moduleDatum.ClassName.StartsWith("global::"))
            .Append(moduleDatum.ClassName)
            .Append("()")
            .Append(".")
            .Append(moduleDatum.MethodName)
            .AppendLine("));");
    }
}