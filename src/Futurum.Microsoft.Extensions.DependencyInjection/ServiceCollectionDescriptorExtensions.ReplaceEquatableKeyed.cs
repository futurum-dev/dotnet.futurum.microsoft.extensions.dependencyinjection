using Microsoft.Extensions.DependencyInjection;

namespace Futurum.Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionDescriptorExtensions
{
    public static void ReplaceEquatableKeyedScoped(this IServiceCollection collection, Type service, object? serviceKey, Type implementationType)
    {
        var descriptor = ServiceDescriptor.KeyedScoped(service, serviceKey, implementationType);
        collection.ReplaceEquatableKeyed(descriptor);
    }

    public static void ReplaceEquatableKeyedSingleton(this IServiceCollection collection, Type service, object? serviceKey, Type implementationType)
    {
        var descriptor = ServiceDescriptor.KeyedSingleton(service, serviceKey, implementationType);
        collection.ReplaceEquatableKeyed(descriptor);
    }

    public static void ReplaceEquatableKeyedTransient(this IServiceCollection collection, Type service, object? serviceKey, Type implementationType)
    {
        var descriptor = ServiceDescriptor.KeyedTransient(service, serviceKey, implementationType);
        collection.ReplaceEquatableKeyed(descriptor);
    }

    public static IServiceCollection ReplaceEquatableKeyed(this IServiceCollection collection, ServiceDescriptor descriptor)
    {
        var count = collection.Count;
        for (var index = 0; index < count; ++index)
        {
            var serviceKey = collection[index].ServiceKey;
            if (serviceKey != null && collection[index].ServiceType == descriptor.ServiceType && serviceKey.Equals(descriptor.ServiceKey))
            {
                collection.RemoveAt(index);
                break;
            }
        }

        collection.Add(descriptor);

        return collection;
    }
}
