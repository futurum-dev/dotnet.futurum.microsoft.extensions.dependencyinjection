using System.Collections.Generic;
using System.Threading.Tasks;

using VerifyXunit;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Writer.Tests;

[UsesVerify]
public class RegistrationWriterTransientTests
{
    [Fact]
    public Task Try()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                null,
                RegistrationLifetime.Transient,
                DuplicateRegistrationStrategy.Try,
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1")
        };

        var result = RegistrationWriter.Write(nameof(RegistrationWriterTransientTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task Add()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                null,
                RegistrationLifetime.Transient,
                DuplicateRegistrationStrategy.Add,
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1")
        };

        var result = RegistrationWriter.Write(nameof(RegistrationWriterTransientTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task Replace()
    {
        var registrationData = new List<RegistrationDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1",
                null,
                RegistrationLifetime.Transient,
                DuplicateRegistrationStrategy.Replace,
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.IService1",
                "Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Service1")
        };

        var result = RegistrationWriter.Write(nameof(RegistrationWriterTransientTests), registrationData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}
