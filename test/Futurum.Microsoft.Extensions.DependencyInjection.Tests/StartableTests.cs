using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Tests;

public class StartableTests
{
    public class AddStartable_with_BuildServiceProviderWithStartables
    {
        [Fact]
        public async Task instance()
        {
            var services = new ServiceCollection();

            var singletonList = new SingletonList();

            services.AddSingleton(singletonList);

            services.AddStartable(new TestStartable(singletonList));

            var serviceProvider = await services.BuildServiceProviderWithStartablesAsync();

            var result = serviceProvider.GetService<SingletonList>();

            result.Numbers.Should().BeEquivalentTo(Enumerable.Range(0, 10));
        }

        [Fact]
        public async Task generic()
        {
            var services = new ServiceCollection();

            services.AddSingleton<SingletonList>();

            services.AddStartable<TestStartable>();

            var serviceProvider = await services.BuildServiceProviderWithStartablesAsync();

            var result = serviceProvider.GetService<SingletonList>();

            result.Numbers.Should().BeEquivalentTo(Enumerable.Range(0, 10));
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

            var result = serviceProvider.GetService<SingletonList>();

            result.Numbers.Should().BeEquivalentTo(Enumerable.Range(0, 10));

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

            var result = serviceProvider.GetService<SingletonList>();

            result.Numbers.Should().BeEquivalentTo(Enumerable.Range(0, 10));

            await startableHostedService.StopAsync(CancellationToken.None);
        }
    }

    [Fact]
    public async Task StartableFunctionWrapper()
    {
        var services = new ServiceCollection();

        services.AddSingleton<SingletonList>();

        services.AddStartable(new StartableFunctionWrapper(new TestWrapperStartable().StartAsync));

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

        public async Task StartAsync()
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

        public async Task StartAsync()
        {
            var numbers = Enumerable.Range(0, 10);

            foreach (var number in numbers)
            {
                SingletonList.Add(number);
            }
        }
    }
}
