using System.Collections.Generic;
using System.Threading.Tasks;

using VerifyXunit;

using Xunit;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests;

[UsesVerify]
public class StartableCodeGeneratorTests
{
    [Fact]
    public Task Static()
    {
        var startableData = new List<StartableDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Startable1",
                "Startable1Execute",
                true)
        };

        var result = StartableWriter.Write(nameof(StartableCodeGeneratorTests), startableData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
    
    [Fact]
    public Task Instance()
    {
        var startableData = new List<StartableDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Startable1",
                "Startable1Execute",
                false)
        };

        var result = StartableWriter.Write(nameof(StartableCodeGeneratorTests), startableData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}