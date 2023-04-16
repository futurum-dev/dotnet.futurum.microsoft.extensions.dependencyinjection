using FluentAssertions;

using Futurum.Test.Result;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Xunit;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Tests;

public class StartableTests
{
    public class AddStartable_with_BuildServiceProviderWithStartables
    {
        [Fact]
        public void instance()
        {
            var services = new ServiceCollection();

            var singletonList = new SingletonList();

            services.AddSingleton(singletonList);

            services.AddStartable(new TestStartable(singletonList));

            var serviceProvider = services.BuildServiceProviderWithStartables();

            var result = serviceProvider.TryGetService<SingletonList>();

            result.ShouldBeSuccessWithValueEquivalentTo(x => x.Numbers, Enumerable.Range(0, 10));
        }

        [Fact]
        public void generic()
        {
            var services = new ServiceCollection();

            services.AddSingleton<SingletonList>();

            services.AddStartable<TestStartable>();

            var serviceProvider = services.BuildServiceProviderWithStartables();

            var result = serviceProvider.TryGetService<SingletonList>();

            result.ShouldBeSuccessWithValueEquivalentTo(x => x.Numbers, Enumerable.Range(0, 10));
        }
    }

    public class AddStartable_with_StartableHostedService
    {
        [Fact]
        public async Task instance()
        {
            var services = new ServiceCollection();

            var singletonList = new SingletonList();

            services.AddSingleton(singletonList);

            services.AddStartable(new TestStartable(singletonList));

            var serviceProvider = services.BuildServiceProvider();

            var startableHostedService = serviceProvider.GetService<IHostedService>();
            await startableHostedService.StartAsync(CancellationToken.None);

            var result = serviceProvider.TryGetService<SingletonList>();

            result.ShouldBeSuccessWithValueEquivalentTo(x => x.Numbers, Enumerable.Range(0, 10));

            await startableHostedService.StopAsync(CancellationToken.None);
        }

        [Fact]
        public async Task generic()
        {
            var services = new ServiceCollection();

            services.AddSingleton<SingletonList>();

            services.AddStartable<TestStartable>();

            var serviceProvider = services.BuildServiceProvider();

            var startableHostedService = serviceProvider.GetService<IHostedService>();
            await startableHostedService.StartAsync(CancellationToken.None);

            var result = serviceProvider.TryGetService<SingletonList>();

            result.ShouldBeSuccessWithValueEquivalentTo(x => x.Numbers, Enumerable.Range(0, 10));

            await startableHostedService.StopAsync(CancellationToken.None);
        }
    }

    [Fact]
    public async Task StartableFunctionWrapper()
    {
        var services = new ServiceCollection();

        services.AddSingleton<SingletonList>();

        services.AddStartable(new StartableFunctionWrapper(new TestWrapperStartable().Start));

        var serviceProvider = services.BuildServiceProvider();

        var startableHostedService = serviceProvider.GetService<IHostedService>();
        await startableHostedService.StartAsync(CancellationToken.None);

        TestWrapperStartable.SingletonList.Numbers.Should().BeEquivalentTo(Enumerable.Range(0, 10));

        await startableHostedService.StopAsync(CancellationToken.None);
    }

    public class SingletonList
    {
        public List<int> Numbers { get; } = new();

        public void Add(int number)
        {
            Numbers.Add(number);
        }
    }

    internal class TestStartable : IStartable
    {
        private readonly SingletonList _singletonList;

        public TestStartable(SingletonList singletonList)
        {
            _singletonList = singletonList;
        }

        public void Start()
        {
            var numbers = Enumerable.Range(0, 10);

            foreach (var number in numbers)
            {
                _singletonList.Add(number);
            }
        }
    }

    internal class TestWrapperStartable : IStartable
    {
        public static readonly SingletonList SingletonList = new();
        
        public void Start()
        {
            var numbers = Enumerable.Range(0, 10);

            foreach (var number in numbers)
            {
                SingletonList.Add(number);
            }
        }
    }
}