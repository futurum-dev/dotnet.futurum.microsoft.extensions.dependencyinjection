using System.Linq;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests;

public class ModuleTests
{
    public class Instance
    {
        [Fact]
        public void check()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDependencyInjectionForFuturumMicrosoftExtensionsDependencyInjectionGeneratorTests();

            var serviceDescriptor = serviceCollection.Where(x => x.ImplementationType == typeof(Service));

            serviceDescriptor.Count().Should().Be(1);
            serviceDescriptor.First().ServiceType.Should().Be(typeof(IService1));
            serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        public class Module
        {
            [RegisterAsDependencyInjectionModule]
            public void Load(IServiceCollection services)
            {
                services.AddScoped<IService1, Service>();
            }
        }

        public class Service : IService1
        {
        }

        public interface IService1
        {
        }
    }

    public class Static
    {
        [Fact]
        public void check()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDependencyInjectionForFuturumMicrosoftExtensionsDependencyInjectionGeneratorTests();

            var serviceDescriptor = serviceCollection.Where(x => x.ImplementationType == typeof(Service));

            serviceDescriptor.Count().Should().Be(1);
            serviceDescriptor.First().ServiceType.Should().Be(typeof(IService1));
            serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        public static class Module
        {
            [RegisterAsDependencyInjectionModule]
            public static void Load(IServiceCollection services)
            {
                services.AddScoped<IService1, Service>();
            }
        }

        public class Service : IService1
        {
        }

        public interface IService1
        {
        }
    }
}