using System.Collections.Generic;
using System.Threading.Tasks;

using VerifyXunit;

using Xunit;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests;

[UsesVerify]
public class RegistrationCodeGeneratorTransientTests
{
    [Fact]
    public Task Transient_Try()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Transient,
                DuplicateRegistrationStrategy.Try)
        };

        var result = RegistrationWriter.Write(nameof(RegistrationCodeGeneratorTransientTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task Transient_Add()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Transient,
                DuplicateRegistrationStrategy.Add)
        };

        var result = RegistrationWriter.Write(nameof(RegistrationCodeGeneratorTransientTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task Transient_Replace()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Transient,
                DuplicateRegistrationStrategy.Replace)
        };

        var result = RegistrationWriter.Write(nameof(RegistrationCodeGeneratorTransientTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}