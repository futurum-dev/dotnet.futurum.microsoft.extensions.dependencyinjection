using Futurum.Test.Result;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Tests;

public class StartableTests
{
    [Fact]
    public async Task RegisterModule_instance()
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
    public async Task RegisterModule_generic()
    {
        var services = new ServiceCollection();

        services.AddSingleton<SingletonList>();

        services.AddStartable<TestStartable>();

        var serviceProvider = services.BuildServiceProviderWithStartables();

        var result = serviceProvider.TryGetService<SingletonList>();

        result.ShouldBeSuccessWithValueEquivalentTo(x => x.Numbers, Enumerable.Range(0, 10));
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
}