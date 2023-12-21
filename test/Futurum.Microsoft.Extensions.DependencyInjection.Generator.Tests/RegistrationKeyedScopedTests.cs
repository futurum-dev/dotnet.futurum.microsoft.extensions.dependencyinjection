using System.Linq;

using Microsoft.Extensions.DependencyInjection;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests;

public class RegistrationKeyedScopedTests
{
    public class Default
    {
        [Fact]
        public void check()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDependencyInjectionForFuturumMicrosoftExtensionsDependencyInjectionGeneratorTests();

            var serviceDescriptor = serviceCollection
                .Where(x => x.IsKeyedService &&
                            x.ServiceKey.Equals("ServiceKey") &&
                            x.KeyedImplementationType == typeof(Service));

            serviceDescriptor.Count().Should().Be(1);
            serviceDescriptor.First().ServiceType.Should().Be(typeof(IService1));
            serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [RegisterAsKeyedScoped("ServiceKey")]
        public class Service : IService1
        {
        }

        public interface IService1
        {
        }
    }

    public class As
    {
        [Fact]
        public void check()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDependencyInjectionForFuturumMicrosoftExtensionsDependencyInjectionGeneratorTests();

            var serviceDescriptor = serviceCollection.Where(x =>
                                                                x.IsKeyedService &&
                                                                x.ServiceKey.Equals("ServiceKey") &&
                                                                x.KeyedImplementationType == typeof(Service));

            serviceDescriptor.Count().Should().Be(1);
            serviceDescriptor.First().ServiceType.Should().Be(typeof(IService2));
            serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [RegisterAsScoped.AsKeyed<IService2>("ServiceKey")]
        public class Service : IService1, IService2
        {
        }

        public interface IService1
        {
        }

        public interface IService2
        {
        }
    }

    public class AsImplementedInterfaces
    {
        [Fact]
        public void check()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDependencyInjectionForFuturumMicrosoftExtensionsDependencyInjectionGeneratorTests();

            var serviceDescriptor = serviceCollection.Where(x => x.IsKeyedService &&
                                                                 x.ServiceKey.Equals("ServiceKey") &&
                                                                 x.KeyedImplementationType == typeof(Service));

            serviceDescriptor.Count().Should().Be(2);
            serviceDescriptor.First(x => x.ServiceType == typeof(IService1)).Lifetime.Should().Be(ServiceLifetime.Scoped);
            serviceDescriptor.First(x => x.ServiceType == typeof(IService2)).Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [RegisterAsScoped.AsKeyedImplementedInterfaces("ServiceKey")]
        public class Service : IService1, IService2
        {
        }

        public interface IService1
        {
        }

        public interface IService2
        {
        }
    }

    public class AsImplementedInterfacesAndSelf
    {
        [Fact]
        public void check()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDependencyInjectionForFuturumMicrosoftExtensionsDependencyInjectionGeneratorTests();

            var serviceDescriptor = serviceCollection.Where(x => x.IsKeyedService &&
                                                                 x.ServiceKey.Equals("ServiceKey") &&
                                                                 x.KeyedImplementationType == typeof(Service));

            serviceDescriptor.Count().Should().Be(3);
            serviceDescriptor.First(x => x.ServiceType == typeof(Service)).Lifetime.Should().Be(ServiceLifetime.Scoped);
            serviceDescriptor.First(x => x.ServiceType == typeof(IService1)).Lifetime.Should().Be(ServiceLifetime.Scoped);
            serviceDescriptor.First(x => x.ServiceType == typeof(IService2)).Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [RegisterAsScoped.AsKeyedImplementedInterfacesAndSelf("ServiceKey")]
        public class Service : IService1, IService2
        {
        }

        public interface IService1
        {
        }

        public interface IService2
        {
        }
    }

    public class AsSelf
    {
        [Fact]
        public void check()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDependencyInjectionForFuturumMicrosoftExtensionsDependencyInjectionGeneratorTests();

            var serviceDescriptor = serviceCollection.Where(x => x.IsKeyedService &&
                                                                 x.ServiceKey.Equals("ServiceKey") &&
                                                                 x.KeyedImplementationType == typeof(Service));

            serviceDescriptor.Count().Should().Be(1);
            serviceDescriptor.First(x => x.ServiceType == typeof(Service)).Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [RegisterAsScoped.AsKeyedSelf("ServiceKey")]
        public class Service : IService1, IService2
        {
        }

        public interface IService1
        {
        }

        public interface IService2
        {
        }
    }

    public class DuplicateRegistrationStrategy
    {
        public class Try
        {
            [Fact]
            public void check()
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddDependencyInjectionForFuturumMicrosoftExtensionsDependencyInjectionGeneratorTests();

                var serviceDescriptor = serviceCollection.Where(x =>
                                                                    x.IsKeyedService &&
                                                                    x.ServiceKey != null &&
                                                                    x.ServiceKey.Equals("ServiceKey") &&
                                                                    x.ServiceType == typeof(IService));

                serviceDescriptor.Count().Should().Be(1);
                serviceDescriptor.First().KeyedImplementationType.Should().Be(typeof(Service1));
                serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
            }

            [RegisterAsKeyedScoped("ServiceKey", DuplicateRegistrationStrategy = DependencyInjection.DuplicateRegistrationStrategy.Try)]
            public class Service1 : IService
            {
            }

            [RegisterAsKeyedScoped("ServiceKey", DuplicateRegistrationStrategy = DependencyInjection.DuplicateRegistrationStrategy.Try)]
            public class Service2 : IService
            {
            }

            public interface IService
            {
            }
        }

        public class Replace
        {
            [Fact]
            public void check()
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddDependencyInjectionForFuturumMicrosoftExtensionsDependencyInjectionGeneratorTests();

                var serviceDescriptor = serviceCollection.Where(x =>
                                                                    x.IsKeyedService &&
                                                                    x.ServiceKey != null &&
                                                                    x.ServiceKey.Equals("ServiceKey") &&
                                                                    x.ServiceType == typeof(IService));

                serviceDescriptor.Count().Should().Be(1);
                serviceDescriptor.First().KeyedImplementationType.Should().Be(typeof(Service2));
                serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
            }

            [RegisterAsKeyedScoped("ServiceKey", DuplicateRegistrationStrategy = DependencyInjection.DuplicateRegistrationStrategy.Replace)]
            public class Service1 : IService
            {
            }

            [RegisterAsKeyedScoped("ServiceKey", DuplicateRegistrationStrategy = DependencyInjection.DuplicateRegistrationStrategy.Replace)]
            public class Service2 : IService
            {
            }

            public interface IService
            {
            }
        }

        public class Add
        {
            [Fact]
            public void check()
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddDependencyInjectionForFuturumMicrosoftExtensionsDependencyInjectionGeneratorTests();

                var serviceDescriptor = serviceCollection.Where(x =>
                                                                    x.IsKeyedService &&
                                                                    x.ServiceKey != null &&
                                                                    x.ServiceKey.Equals("ServiceKey") &&
                                                                    x.ServiceType == typeof(IService));

                serviceDescriptor.Count().Should().Be(2);
                serviceDescriptor.First().KeyedImplementationType.Should().Be(typeof(Service1));
                serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
                serviceDescriptor.Skip(1).First().KeyedImplementationType.Should().Be(typeof(Service2));
                serviceDescriptor.Skip(1).First().Lifetime.Should().Be(ServiceLifetime.Scoped);
            }

            [RegisterAsKeyedScoped("ServiceKey", DuplicateRegistrationStrategy = DependencyInjection.DuplicateRegistrationStrategy.Add)]
            public class Service1 : IService
            {
            }

            [RegisterAsKeyedScoped("ServiceKey", DuplicateRegistrationStrategy = DependencyInjection.DuplicateRegistrationStrategy.Add)]
            public class Service2 : IService
            {
            }

            public interface IService
            {
            }
        }
    }
}
