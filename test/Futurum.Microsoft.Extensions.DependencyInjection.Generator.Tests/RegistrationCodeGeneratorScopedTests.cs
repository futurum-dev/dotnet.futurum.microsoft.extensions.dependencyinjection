using System.Collections.Generic;
using System.Threading.Tasks;

using VerifyXunit;

using Xunit;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests;

[UsesVerify]
public class RegistrationCodeGeneratorScopedTests
{
    [Fact]
    public Task Scoped_Try()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Scoped,
                DuplicateRegistrationStrategy.Try)
        };

        var result = RegistrationWriter.Write(nameof(RegistrationCodeGeneratorScopedTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task Scoped_Add()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Scoped,
                DuplicateRegistrationStrategy.Add)
        };

        var result = RegistrationWriter.Write(nameof(RegistrationCodeGeneratorScopedTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task Scoped_Replace()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                RegistrationLifetime.Scoped,
                DuplicateRegistrationStrategy.Replace)
        };

        var result = RegistrationWriter.Write(nameof(RegistrationCodeGeneratorScopedTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}