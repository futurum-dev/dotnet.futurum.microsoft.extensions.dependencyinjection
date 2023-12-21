using Microsoft.Extensions.DependencyInjection;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Tests;

public class ServiceCollectionDescriptorExtensionsTryAddEquatableKeyedTests
{
    public class TryAddEquatableKeyedScoped
    {
        [Fact]
        public void check_registration()
        {
            var serviceCollection = new ServiceCollection();

            var serviceKey = "key";
            serviceCollection.TryAddEquatableKeyedScoped(typeof(IService), serviceKey, typeof(Service1));
            serviceCollection.TryAddEquatableKeyedScoped(typeof(IService), serviceKey, typeof(Service2));

            var serviceDescriptor = serviceCollection
                .Where(x => x.IsKeyedService &&
                            x.ServiceKey.Equals(serviceKey) &&
                            x.ServiceType == typeof(IService));

            serviceDescriptor.Count().Should().Be(1);
            serviceDescriptor.First().ServiceType.Should().Be(typeof(IService));
            serviceDescriptor.First().KeyedImplementationType.Should().Be(typeof(Service1));
            serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
        }
    }

    public class TryAddEquatableKeyedSingleton
    {
        [Fact]
        public void check_registration()
        {
            var serviceCollection = new ServiceCollection();

            var serviceKey = "key";
            serviceCollection.TryAddEquatableKeyedSingleton(typeof(IService), serviceKey, typeof(Service1));
            serviceCollection.TryAddEquatableKeyedSingleton(typeof(IService), serviceKey, typeof(Service2));

            var serviceDescriptor = serviceCollection
                .Where(x => x.IsKeyedService &&
                            x.ServiceKey.Equals(serviceKey) &&
                            x.ServiceType == typeof(IService));

            serviceDescriptor.Count().Should().Be(1);
            serviceDescriptor.First().ServiceType.Should().Be(typeof(IService));
            serviceDescriptor.First().KeyedImplementationType.Should().Be(typeof(Service1));
            serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Singleton);
        }
    }

    public class TryAddEquatableKeyedTransient
    {
        [Fact]
        public void check_registration()
        {
            var serviceCollection = new ServiceCollection();

            var serviceKey = "key";
            serviceCollection.TryAddEquatableKeyedTransient(typeof(IService), serviceKey, typeof(Service1));
            serviceCollection.TryAddEquatableKeyedTransient(typeof(IService), serviceKey, typeof(Service2));

            var serviceDescriptor = serviceCollection
                .Where(x => x.IsKeyedService &&
                            x.ServiceKey.Equals(serviceKey) &&
                            x.ServiceType == typeof(IService));

            serviceDescriptor.Count().Should().Be(1);
            serviceDescriptor.First().ServiceType.Should().Be(typeof(IService));
            serviceDescriptor.First().KeyedImplementationType.Should().Be(typeof(Service1));
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
