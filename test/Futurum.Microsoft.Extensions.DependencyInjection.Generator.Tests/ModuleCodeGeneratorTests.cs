using System.Collections.Generic;
using System.Threading.Tasks;

using VerifyXunit;

using Xunit;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests;

[UsesVerify]
public class ModuleCodeGeneratorTests
{
    [Fact]
    public Task Static()
    {
        var moduleData = new List<ModuleDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Module1",
                "Module1Execute",
                true)
        };

        var result = ModuleWriter.Write(nameof(ModuleCodeGeneratorTests), moduleData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
    
    [Fact]
    public Task Instance()
    {
        var moduleData = new List<ModuleDatum>
        {
            new("Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests.Module1",
                "Module1Execute",
                false)
        };

        var result = ModuleWriter.Write(nameof(ModuleCodeGeneratorTests), moduleData, true);

        return Verifier.Verify(result).UseDirectory("Snapshots");
    }
}