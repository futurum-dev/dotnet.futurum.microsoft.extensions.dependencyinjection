using Microsoft.Extensions.DependencyInjection;

namespace Futurum.Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionDescriptorExtensions
{
    public static void TryAddEquatableKeyedScoped(this IServiceCollection collection, Type service, object? serviceKey, Type implementationType)
    {
        var descriptor = ServiceDescriptor.KeyedScoped(service, serviceKey, implementationType);
        collection.TryAddEquatableKeyed(descriptor);
    }

    public static void TryAddEquatableKeyedSingleton(this IServiceCollection collection, Type service, object? serviceKey, Type implementationType)
    {
        var descriptor = ServiceDescriptor.KeyedSingleton(service, serviceKey, implementationType);
        collection.TryAddEquatableKeyed(descriptor);
    }

    public static void TryAddEquatableKeyedTransient(this IServiceCollection collection, Type service, object? serviceKey, Type implementationType)
    {
        var descriptor = ServiceDescriptor.KeyedTransient(service, serviceKey, implementationType);
        collection.TryAddEquatableKeyed(descriptor);
    }

    public static IServiceCollection TryAddEquatableKeyed(this IServiceCollection collection, ServiceDescriptor descriptor)
    {
        var count = collection.Count;
        for (var index = 0; index < count; ++index)
        {
            var serviceKey = collection[index].ServiceKey;
            if (serviceKey != null && collection[index].ServiceType == descriptor.ServiceType && serviceKey.Equals(descriptor.ServiceKey))
                return collection;
        }

        collection.Add(descriptor);

        return collection;
    }
}
