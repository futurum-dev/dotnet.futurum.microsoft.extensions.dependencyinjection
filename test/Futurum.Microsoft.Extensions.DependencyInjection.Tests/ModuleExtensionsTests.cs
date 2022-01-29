using Futurum.Test.Result;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Tests;

public class ModuleExtensionsTests
{
    [Fact]
    public void RegisterModule_instance()
    {
        var services = new ServiceCollection();
        services.RegisterModule(new TestModule());

        var serviceProvider = services.BuildServiceProvider();

        var result = serviceProvider.TryGetService<ITestService>();

        result.ShouldBeSuccess();
    }

    [Fact]
    public void RegisterModule_generic()
    {
        var services = new ServiceCollection();
        services.RegisterModule<TestModule>();

        var serviceProvider = services.BuildServiceProvider();

        var result = serviceProvider.TryGetService<ITestService>();

        result.ShouldBeSuccess();
    }

    public class TestModule : IModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddSingleton<ITestService, TestService>();
        }
    }

    public interface ITestService
    {
    }

    public class TestService : ITestService
    {
    }
}