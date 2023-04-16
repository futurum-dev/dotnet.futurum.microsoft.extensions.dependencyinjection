using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Core;
using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Extensions;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public static class RegistrationWriter
{
    public static string Write(string methodName, IEnumerable<RegistrationDatum> registrationData, bool skipVersion = false) =>
        WrapperSourceGeneratorWriter.Write(methodName, "RegisterRegistrations",
                                           codeBuilder => Write(registrationData, codeBuilder),
                                           true,
                                           skipVersion);

    private static void Write(IEnumerable<RegistrationDatum> registrationData, IndentedStringBuilder codeBuilder)
    {
        foreach (var registrationDatum in registrationData)
        {
            WriteRegistration(codeBuilder, registrationDatum);
        }
    }

    private static void WriteRegistration(IndentedStringBuilder codeBuilder, RegistrationDatum registrationDatum)
    {
        var serviceCollectionRegistrationMethod = GetServiceCollectionRegistrationMethod(registrationDatum.DuplicateRegistrationStrategy);

        foreach (var serviceType in registrationDatum.ServiceTypes)
        {
            if (serviceType.IsNullOrWhiteSpace())
                continue;

            WriteServiceType(codeBuilder, registrationDatum, serviceCollectionRegistrationMethod, serviceType);
        }
    }

    private static void WriteServiceType(IndentedStringBuilder codeBuilder, RegistrationDatum registrationDatum, string serviceCollectionRegistrationMethod, string serviceType)
    {
        codeBuilder
            .Append("global::Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.")
            .Append(serviceCollectionRegistrationMethod)
            .AppendLine("(")
            .IncrementIndent()
            .AppendLine("serviceCollection,")
            .AppendLine("global::Microsoft.Extensions.DependencyInjection.ServiceDescriptor.Describe(")
            .IncrementIndent()
            .Append("typeof(")
            .AppendIf("global::", !serviceType.StartsWith("global::"))
            .Append(serviceType)
            .AppendLine("),");

        if (registrationDatum.ImplementationType.HasValue())
        {
            codeBuilder
                .Append("typeof(")
                .AppendIf("global::", !registrationDatum.ImplementationType.StartsWith("global::"))
                .Append(registrationDatum.ImplementationType)
                .Append(')');
        }
        else
        {
            codeBuilder
                .Append("typeof(")
                .AppendIf("global::", !serviceType.StartsWith("global::"))
                .Append(serviceType)
                .Append(')');
        }

        var lifetime = GetMicrosoftExtensionsDependencyInjectionServiceLifetime(registrationDatum.Lifetime);
        
        codeBuilder
            .AppendLine(", ")
            .Append("global::")
            .Append(lifetime)
            .AppendLine()
            .DecrementIndent()
            .AppendLine(")")
            .DecrementIndent()
            .AppendLine(");")
            .AppendLine();
    }

    private static string GetServiceCollectionRegistrationMethod(DuplicateRegistrationStrategy duplicateRegistrationStrategy) =>
        duplicateRegistrationStrategy switch
        {
            DuplicateRegistrationStrategy.Try     => "TryAdd",
            DuplicateRegistrationStrategy.Replace => "Replace",
            DuplicateRegistrationStrategy.Add     => "Add",
            _                                     => "TryAdd"
        };

    private static string GetMicrosoftExtensionsDependencyInjectionServiceLifetime(RegistrationLifetime registrationLifetime) =>
        registrationLifetime switch
        {
            RegistrationLifetime.Transient => "Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient",
            RegistrationLifetime.Scoped    => "Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped",
            RegistrationLifetime.Singleton => "Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton",
            _                              => "Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient"
        };
}