using System.Collections.Generic;
using System.Threading.Tasks;

using VerifyXunit;

using Xunit;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests;

[UsesVerify]
public class RegistrationCodeGeneratorSingletonTests
{
    [Fact]
    public Task Singleton_Try()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Singleton,
                DuplicateRegistrationStrategy.Try)
        };

        var result = RegistrationWriter.Write(nameof(RegistrationCodeGeneratorSingletonTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task Singleton_Add()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Singleton,
                DuplicateRegistrationStrategy.Add)
        };

        var result = RegistrationWriter.Write(nameof(RegistrationCodeGeneratorSingletonTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task Singleton_Replace()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Singleton,
                DuplicateRegistrationStrategy.Replace)
        };

        var result = RegistrationWriter.Write(nameof(RegistrationCodeGeneratorSingletonTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}