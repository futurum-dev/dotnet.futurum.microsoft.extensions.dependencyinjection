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
        var serviceCollectionRegistrationMethod = GetServiceCollectionRegistrationMethod(registrationDatum.Key, registrationDatum.DuplicateRegistrationStrategy);

        WriteRegistration(codeBuilder, registrationDatum, serviceCollectionRegistrationMethod, registrationDatum.ServiceType);
    }

    private static void WriteRegistration(IndentedStringBuilder codeBuilder, RegistrationDatum registrationDatum, string serviceCollectionRegistrationMethod, string serviceType)
    {
        codeBuilder
            .Append(serviceCollectionRegistrationMethod)
            .AppendLine("(")
            .IncrementIndent()
            .AppendLine("serviceCollection,")
            .Append("global::Microsoft.Extensions.DependencyInjection.ServiceDescriptor.");

        if (!string.IsNullOrEmpty(registrationDatum.Key))
        {
            codeBuilder
                .Append("DescribeKeyed");
        }
        else
        {
            codeBuilder
                .Append("Describe");
        }

        codeBuilder
            .AppendLine("(")
            .IncrementIndent();

        // Service type
        codeBuilder
            .Append("typeof(")
            .AppendIf("global::", !serviceType.StartsWith("global::"))
            .Append(serviceType)
            .AppendLine("),");

        // Service key
        if (!string.IsNullOrEmpty(registrationDatum.Key))
        {
            codeBuilder
                .Append("\"")
                .Append(registrationDatum.Key)
                .Append("\"")
                .AppendLine(",");
        }

        // Implementation type
        codeBuilder
            .Append("typeof(")
            .AppendIf("global::", !registrationDatum.ImplementationType.StartsWith("global::"))
            .Append(registrationDatum.ImplementationType)
            .Append(')');

        // Lifetime
        var lifetime = GetMicrosoftExtensionsDependencyInjectionServiceLifetime(registrationDatum.Lifetime);

        codeBuilder
            .AppendLine(",")
            .Append("global::")
            .Append(lifetime);

        codeBuilder
            .AppendLine()
            .DecrementIndent()
            .AppendLine(")")
            .DecrementIndent()
            .AppendLine(");")
            .AppendLine();
    }

    private static string GetServiceCollectionRegistrationMethod(string? serviceKey, DuplicateRegistrationStrategy duplicateRegistrationStrategy)
    {
        if (serviceKey != null)
        {
            return duplicateRegistrationStrategy switch
            {
                DuplicateRegistrationStrategy.Try     => "global::Futurum.Microsoft.Extensions.DependencyInjection.ServiceCollectionDescriptorExtensions.TryAddEquatableKeyed",
                DuplicateRegistrationStrategy.Replace => "global::Futurum.Microsoft.Extensions.DependencyInjection.ServiceCollectionDescriptorExtensions.ReplaceEquatableKeyed",
                DuplicateRegistrationStrategy.Add     => "global::Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.Add",
                _                                     => "global::Futurum.Microsoft.Extensions.DependencyInjection.ServiceCollectionDescriptorExtensions.TryAddEquatableKeyed"
            };
        }

        return duplicateRegistrationStrategy switch
        {
            DuplicateRegistrationStrategy.Try     => "global::Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.TryAdd",
            DuplicateRegistrationStrategy.Replace => "global::Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.Replace",
            DuplicateRegistrationStrategy.Add     => "global::Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.Add",
            _                                     => "global::Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.TryAdd"
        };
    }

    private static string GetMicrosoftExtensionsDependencyInjectionServiceLifetime(RegistrationLifetime registrationLifetime) =>
        registrationLifetime switch
        {
            RegistrationLifetime.Transient => "Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient",
            RegistrationLifetime.Scoped    => "Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped",
            RegistrationLifetime.Singleton => "Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton",
            _                              => "Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient"
        };
}
