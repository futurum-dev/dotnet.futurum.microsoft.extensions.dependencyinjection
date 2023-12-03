using System.Collections.Generic;
using System.Threading.Tasks;

using VerifyXunit;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Writer.Tests;

[UsesVerify]
public class RegistrationWriterSingletonTests
{
    [Fact]
    public Task Try()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Singleton,
                DuplicateRegistrationStrategy.Try,
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1")
        };

        var result = RegistrationWriter.Write(nameof(RegistrationWriterSingletonTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task Add()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Singleton,
                DuplicateRegistrationStrategy.Add,
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1")
        };

        var result = RegistrationWriter.Write(nameof(RegistrationWriterSingletonTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task Replace()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Singleton,
                DuplicateRegistrationStrategy.Replace,
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1")
        };

        var result = RegistrationWriter.Write(nameof(RegistrationWriterSingletonTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}