using System.Linq;

using Microsoft.Extensions.DependencyInjection;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Tests;

public class RegistrationScopedTests
{
    public class Default
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

        [RegisterAsScoped]
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

            var serviceDescriptor = serviceCollection.Where(x => x.ImplementationType == typeof(Service));

            serviceDescriptor.Count().Should().Be(1);
            serviceDescriptor.First().ServiceType.Should().Be(typeof(IService2));
            serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [RegisterAsScoped.As<IService2>]
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

            var serviceDescriptor = serviceCollection.Where(x => x.ImplementationType == typeof(Service));

            serviceDescriptor.Count().Should().Be(2);
            serviceDescriptor.First(x => x.ServiceType == typeof(IService1)).Lifetime.Should().Be(ServiceLifetime.Scoped);
            serviceDescriptor.First(x => x.ServiceType == typeof(IService2)).Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [RegisterAsScoped.AsImplementedInterfaces]
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

            var serviceDescriptor = serviceCollection.Where(x => x.ImplementationType == typeof(Service));

            serviceDescriptor.Count().Should().Be(3);
            serviceDescriptor.First(x => x.ServiceType == typeof(Service)).Lifetime.Should().Be(ServiceLifetime.Scoped);
            serviceDescriptor.First(x => x.ServiceType == typeof(IService1)).Lifetime.Should().Be(ServiceLifetime.Scoped);
            serviceDescriptor.First(x => x.ServiceType == typeof(IService2)).Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [RegisterAsScoped.AsImplementedInterfacesAndSelf]
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

            var serviceDescriptor = serviceCollection.Where(x => x.ImplementationType == typeof(Service));

            serviceDescriptor.Count().Should().Be(1);
            serviceDescriptor.First(x => x.ServiceType == typeof(Service)).Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [RegisterAsScoped.AsSelf]
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

                var serviceDescriptor = serviceCollection.Where(x => x.ServiceType == typeof(IService));

                serviceDescriptor.Count().Should().Be(1);
                serviceDescriptor.First().ImplementationType.Should().Be(typeof(Service1));
                serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
            }

            [RegisterAsScoped(DuplicateRegistrationStrategy = DependencyInjection.DuplicateRegistrationStrategy.Try)]
            public class Service1 : IService
            {
            }

            [RegisterAsScoped(DuplicateRegistrationStrategy = DependencyInjection.DuplicateRegistrationStrategy.Try)]
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

                var serviceDescriptor = serviceCollection.Where(x => x.ServiceType == typeof(IService));

                serviceDescriptor.Count().Should().Be(1);
                serviceDescriptor.First().ImplementationType.Should().Be(typeof(Service2));
                serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
            }

            [RegisterAsScoped(DuplicateRegistrationStrategy = DependencyInjection.DuplicateRegistrationStrategy.Replace)]
            public class Service1 : IService
            {
            }

            [RegisterAsScoped(DuplicateRegistrationStrategy = DependencyInjection.DuplicateRegistrationStrategy.Replace)]
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

                var serviceDescriptor = serviceCollection.Where(x => x.ServiceType == typeof(IService));

                serviceDescriptor.Count().Should().Be(2);
                serviceDescriptor.First().ImplementationType.Should().Be(typeof(Service1));
                serviceDescriptor.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
                serviceDescriptor.Skip(1).First().ImplementationType.Should().Be(typeof(Service2));
                serviceDescriptor.Skip(1).First().Lifetime.Should().Be(ServiceLifetime.Scoped);
            }

            [RegisterAsScoped(DuplicateRegistrationStrategy = DependencyInjection.DuplicateRegistrationStrategy.Add)]
            public class Service1 : IService
            {
            }

            [RegisterAsScoped(DuplicateRegistrationStrategy = DependencyInjection.DuplicateRegistrationStrategy.Add)]
            public class Service2 : IService
            {
            }

            public interface IService
            {
            }
        }
    }
}