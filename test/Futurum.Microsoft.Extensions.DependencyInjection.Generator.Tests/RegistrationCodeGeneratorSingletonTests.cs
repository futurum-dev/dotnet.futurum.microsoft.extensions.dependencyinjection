using System.Collections.Generic;
using System.Threading.Tasks;

using VerifyXunit;

using Xunit;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests;

[UsesVerify]
public class RegistrationCodeGeneratorSingletonTests
{
    [Fact]
    public Task Single_Interface()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new(new[] { "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1" },
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Singleton, 
                DuplicateRegistrationStrategy.Try,
                InterfaceRegistrationStrategy.SelfWithInterfaces)
        };

        var result = RegistrationWriter.Write(nameof(RegistrationCodeGeneratorSingletonTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task Multiple_Interface()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new(new[]
                {
                    "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service",
                    "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                    "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService2",
                },
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service",
                RegistrationLifetime.Singleton, 
                DuplicateRegistrationStrategy.Try,
                InterfaceRegistrationStrategy.SelfWithInterfaces)
        };

        var result = RegistrationWriter.Write(nameof(RegistrationCodeGeneratorSingletonTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task Self()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new(new[] { "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1" },
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Singleton,
                DuplicateRegistrationStrategy.Try, 
                InterfaceRegistrationStrategy.Self)
        };

        var result = RegistrationWriter.Write(nameof(RegistrationCodeGeneratorSingletonTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task Single_Interface_Append()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new(new[] { "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1" },
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Singleton,
                DuplicateRegistrationStrategy.Add, 
                InterfaceRegistrationStrategy.SelfWithInterfaces)
        };

        var result = RegistrationWriter.Write(nameof(RegistrationCodeGeneratorSingletonTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task Single_Interface_Replace()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new(new[] { "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1" },
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Singleton, 
                DuplicateRegistrationStrategy.Replace,
                InterfaceRegistrationStrategy.SelfWithInterfaces)
        };

        var result = RegistrationWriter.Write(nameof(RegistrationCodeGeneratorSingletonTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}