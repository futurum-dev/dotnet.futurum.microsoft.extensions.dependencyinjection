using Microsoft.Extensions.DependencyInjection;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Tests;

public class ServiceCollectionDescriptorExtensionsReplaceEquatableKeyedTests
{
    public class ReplaceEquatableKeyedScoped
    {
        [Fact]
        public void check_registration()
        {
            var serviceCollection = new ServiceCollection();

            var serviceKey = "key";
            serviceCollection.ReplaceEquatableKeyedScoped(typeof(IService), serviceKey, typeof(Service1));
            serviceCollection.ReplaceEquatableKeyedScoped(typeof(IService), serviceKey, typeof(Service2));

            var serviceDescriptor = serviceCollection
                .Where(x => x.IsKeyedService &&
                            x.ServiceKey.Equals(serviceKey) &&
                            x.ServiceType == typeof(IService));

            serviceDescriptor.Count().Should().Be(1);
            serviceDescriptor.First().ServiceType.Should().Be(typeof(IService));
            serviceDescriptor.First().KeyedImplementationType.Should().Be(typeof(Service2));
            serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
        }
    }

    public class ReplaceEquatableKeyedSingleton
    {
        [Fact]
        public void check_registration()
        {
            var serviceCollection = new ServiceCollection();

            var serviceKey = "key";
            serviceCollection.ReplaceEquatableKeyedSingleton(typeof(IService), serviceKey, typeof(Service1));
            serviceCollection.ReplaceEquatableKeyedSingleton(typeof(IService), serviceKey, typeof(Service2));

            var serviceDescriptor = serviceCollection
                .Where(x => x.IsKeyedService &&
                            x.ServiceKey.Equals(serviceKey) &&
                            x.ServiceType == typeof(IService));

            serviceDescriptor.Count().Should().Be(1);
            serviceDescriptor.First().ServiceType.Should().Be(typeof(IService));
            serviceDescriptor.First().KeyedImplementationType.Should().Be(typeof(Service2));
            serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Singleton);
        }
    }

    public class ReplaceEquatableKeyedTransient
    {
        [Fact]
        public void check_registration()
        {
            var serviceCollection = new ServiceCollection();

            var serviceKey = "key";
            serviceCollection.ReplaceEquatableKeyedTransient(typeof(IService), serviceKey, typeof(Service1));
            serviceCollection.ReplaceEquatableKeyedTransient(typeof(IService), serviceKey, typeof(Service2));

            var serviceDescriptor = serviceCollection
                .Where(x => x.IsKeyedService &&
                            x.ServiceKey.Equals(serviceKey) &&
                            x.ServiceType == typeof(IService));

            serviceDescriptor.Count().Should().Be(1);
            serviceDescriptor.First().ServiceType.Should().Be(typeof(IService));
            serviceDescriptor.First().KeyedImplementationType.Should().Be(typeof(Service2));
            serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Transient);
        }
    }

    public interface IService
    {
    }

    public class Service1 : IService
    {
    }

    public class Service2 : IService
    {
    }
}
