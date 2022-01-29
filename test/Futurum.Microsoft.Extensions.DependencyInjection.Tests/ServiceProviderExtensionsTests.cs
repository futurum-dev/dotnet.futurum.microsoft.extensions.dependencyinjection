using Futurum.Test.Result;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Tests;

public class ServiceProviderExtensionsTests
{
    public class TryGetService
    {
        [Fact]
        public void success()
        {
            var serviceInstance = new TestService();

            var services = new ServiceCollection();
            services.AddSingleton<ITestService>(serviceInstance);

            var serviceProvider = services.BuildServiceProvider();

            var result = serviceProvider.TryGetService<ITestService>();

            result.ShouldBeSuccessWithValue(serviceInstance as ITestService);
        }

        [Fact]
        public void failure()
        {
            var services = new ServiceCollection();

            var serviceProvider = services.BuildServiceProvider();

            var result = serviceProvider.TryGetService<ITestService>();

            result.ShouldBeFailureWithError($"Failed to resolve Type : '{typeof(ITestService).FullName}'");
        }
    }

    public class TryGetServiceWithType
    {
        [Fact]
        public void success()
        {
            var serviceInstance = new TestService();

            var services = new ServiceCollection();
            services.AddSingleton<ITestService>(serviceInstance);

            var serviceProvider = services.BuildServiceProvider();

            var result = serviceProvider.TryGetService<ITestService>(typeof(ITestService));

            result.ShouldBeSuccessWithValue(serviceInstance as ITestService);
        }

        [Fact]
        public void failure()
        {
            var services = new ServiceCollection();

            var serviceProvider = services.BuildServiceProvider();

            var result = serviceProvider.TryGetService<ITestService>(typeof(ITestService));

            result.ShouldBeFailureWithError($"Failed to resolve Type : '{typeof(ITestService).FullName}'");
        }
    }

    public class TryGetServiceWithoutGenericWithType
    {
        [Fact]
        public void success()
        {
            var serviceInstance = new TestService();

            var services = new ServiceCollection();
            services.AddSingleton<ITestService>(serviceInstance);

            var serviceProvider = services.BuildServiceProvider();

            var result = serviceProvider.TryGetService(typeof(ITestService));

            result.ShouldBeSuccessWithValue(serviceInstance as ITestService);
        }

        [Fact]
        public void failure()
        {
            var services = new ServiceCollection();

            var serviceProvider = services.BuildServiceProvider();

            var result = serviceProvider.TryGetService(typeof(ITestService));

            result.ShouldBeFailureWithError($"Failed to resolve Type : '{typeof(ITestService).FullName}'");
        }
    }

    public interface ITestService
    {
    }

    public class TestService : ITestService
    {
    }
}