using Futurum.Test.Result;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Tests;

public class ModuleExtensionsTests
{
    [Fact]
    public void AddModule_instance()
    {
        var services = new ServiceCollection();
        services.AddModule(new TestModule());

        var serviceProvider = services.BuildServiceProvider();

        var result = serviceProvider.TryGetService<ITestService>();

        result.ShouldBeSuccess();
    }

    [Fact]
    public void AddModule_generic()
    {
        var services = new ServiceCollection();
        services.AddModule<TestModule>();

        var serviceProvider = services.BuildServiceProvider();

        var result = serviceProvider.TryGetService<ITestService>();

        result.ShouldBeSuccess();
    }

    [Fact]
    public void ModuleFunctionWrapper()
    {
        var services = new ServiceCollection();
        services.AddModule(new ModuleFunctionWrapper(new TestWrapperModule().Load));

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

    public class TestWrapperModule : IModule
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